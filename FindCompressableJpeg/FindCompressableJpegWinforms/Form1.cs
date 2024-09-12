using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace FindCompressableJpegWinforms
{
	public partial class Form1 : Form
	{
		private System.ComponentModel.BackgroundWorker backgroundWorker1;

		public List<string[]> listRow;

		public Form1()
		{
			InitializeComponent();
			InitDataGrid();

			//Activate the double buffer to speed draw and resize
			typeof(DataGridView).InvokeMember(
			   "DoubleBuffered",
			   BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
			   null,
			   dataGridView1,
			   new object[] { true });

			InitializeBackgroundWorker();
		}

		private void InitDataGrid()
		{
			//Read values from ini
			var myIni = new IniFile();
			var pathIni = myIni.Read("Path");
			var recursiveIni = myIni.Read("Recursive");
			var sizeTresholdIni = myIni.Read("SizeTreshold");
			var ratioTresholdIni = myIni.Read("RatioTreshold");

			//Affect value from ini to interface
			if (!string.IsNullOrEmpty(recursiveIni))
			{
				recursive.Checked = recursiveIni == "True";
			}

			if (!string.IsNullOrEmpty(sizeTresholdIni))
			{
				sizeTreshold.Value = int.Parse(sizeTresholdIni);
			}

			if (!string.IsNullOrEmpty(ratioTresholdIni))
			{
				ratioTreshold.Value = int.Parse(ratioTresholdIni);
			}

			string[] args = Environment.GetCommandLineArgs();

			//First argument is program name then parameters
			//Use first parameter as directory
			if (args.Length <= 1)
			{
				//argument override path from ini
				if (string.IsNullOrEmpty(pathIni))
				{
					imagesPath.Text = Directory.GetCurrentDirectory();
				}
				else
				{
					imagesPath.Text = pathIni;
				}
			}
			else
			{
				imagesPath.Text = args[1];
			}

			//GetRatios();
		}

		// Set up the BackgroundWorker object by
		// attaching event handlers.
		private void InitializeBackgroundWorker()
		{
			backgroundWorker1 = new BackgroundWorker
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};

			backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		}

		// This event handler is where the actual,
		// potentially time-consuming work is done.
		private void backgroundWorker1_DoWork(object sender,
			DoWorkEventArgs e)
		{
			// Get the BackgroundWorker that raised this event.
			BackgroundWorker worker = sender as BackgroundWorker;

			// Assign the result of the computation
			// to the Result property of the DoWorkEventArgs
			// object. This is will be available to the
			// RunWorkerCompleted eventhandler.
			//e.Result = ComputeFibonacci((int)e.Argument, worker, e);
			GetRatiosBackground(worker, e);
		}

		private void GetRatiosBackground(BackgroundWorker worker, DoWorkEventArgs e)
		{
			// Abort the operation if the user has canceled.
			// Note that a call to CancelAsync may have set
			// CancellationPending to true just after the
			// last invocation of this method exits, so this
			// code will not have the opportunity to set the
			// DoWorkEventArgs.Cancel flag to true. This means
			// that RunWorkerCompletedEventArgs.Cancelled will
			// not be set to true in your RunWorkerCompleted
			// event handler. This is a race condition.

			if (worker.CancellationPending)
			{
				e.Cancel = true;
			}
			else
			{
				GetRatiosProgress(worker);
			}
		}

		// This event handler deals with the results of the
		// background operation.
		private void backgroundWorker1_RunWorkerCompleted(
			object sender, RunWorkerCompletedEventArgs e)
		{
			//In main thread
			// First, handle the case where an exception was thrown.
			if (e.Error != null)
			{
				MessageBox.Show(e.Error.Message);
			}
			else if (e.Cancelled)
			{
				// Next, handle the case where the user canceled
				// the operation.
				// Note that due to a race condition in
				// the DoWork event handler, the Cancelled
				// flag may not have been set, even though
				// CancelAsync was called.
			}
			else
			{
				// Finally, handle the case where the operation
				// succeeded.
			}

			GetRatiosEnd();
		}

		// This event handler updates the progress bar.
		private void backgroundWorker1_ProgressChanged(object sender,
			ProgressChangedEventArgs e)
		{
			//In main thread
			//this.progressBar1.Value = e.ProgressPercentage;
			progressBar1.SetProgressNoAnimation(e.ProgressPercentage);
		}

		private void GetRatios()
		{
			GetRatiosInit();

			GetRatiosProgress();

			GetRatiosEnd();
		}

		private void GetRatiosProgress(BackgroundWorker worker = null)
		{
			//Remove trailing \ if needed
			var path = imagesPath.Text.EndsWith('\\') ? imagesPath.Text.Substring(0, imagesPath.Text.Length - 1) : imagesPath.Text;
			var dir = new DirectoryInfo(path);
			FileInfo[] files;
			if (recursive.Checked)
			{
				files = dir.GetFiles("*.*", SearchOption.AllDirectories);
			}
			else
			{
				files = dir.GetFiles();
			}

			decimal minimalSize = sizeTreshold.Value * 1024;
			var nbFileDone = 0;

			listRow = new List<string[]>();

			foreach (FileInfo file in files)
			{
				if (FilterJpeg(file))
				{
					//Only show file size bigger than treshold
					if (file.Length >= minimalSize)
					{
						//Get dimensions
						int width, height;
						GetBitmapHeightWidth(file, out height, out width);

						//Compute ratio file size / 1024 pixels
						var nbPixels = height * width / 1024;
						var sizeFor1024Pixel = file.Length / (nbPixels != 0 ? nbPixels : 1);

						//Just for tests
						//Thread.Sleep(50);

						//Only show value higher than treshold
						if (sizeFor1024Pixel >= ratioTreshold.Value)
						{
							//filename with absolute path from images path
							var fileName = file.FullName.Substring(dir.FullName.Length + 1);
							string[] row = { fileName, file.Length.ToString(), sizeFor1024Pixel.ToString(), $"{width} x {height}", nbPixels.ToString() };
							listRow.Add(row);
						}

						//Exit if cancel is in progress
						if (worker != null && worker.CancellationPending)
						{
							break;
						}
					}
				}

				nbFileDone++;
				if (worker == null)
				{
					progressBar1.SetProgressNoAnimation(100 * nbFileDone / files.Length);
				}
				else
				{
					worker.ReportProgress(100 * nbFileDone / files.Length);
				}
				//modalProgress.progressBar1.SetProgressNoAnimation(100 * nbFileDone / files.Length);
			}
		}

		private void GetRatiosEnd()
		{
			if (listRow != null)
			{
				foreach (var row in listRow)
				{
					dataGridView1.Rows.Add(row);
				}

				//Sort descending on ratio
				dataGridView1.Sort(dataGridView1.Columns[2], System.ComponentModel.ListSortDirection.Descending);
			}

			progressBar1.Visible = false;
			//modalProgress.Close();

			GetRatiosButton.Text = "Get Ratios";
		}

		private void GetRatiosInit()
		{
			dataGridView1.Rows.Clear();

			progressBar1.Visible = true;
			progressBar1.SetProgressNoAnimation(0);

			/*var modalProgress = new ModalProgress();
            modalProgress.Show();

            //Center modal on main
            modalProgress.Location = new Point(
                this.Location.X + this.Width / 2 - modalProgress.ClientSize.Width / 2,
                this.Location.Y + this.Height / 2 - modalProgress.ClientSize.Height / 2);*/
		}

		/// <summary>
		/// Filter image name
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		private static bool FilterJpeg(FileInfo f)
		{
			return f.Name.ToLower().EndsWith(".jpg") || f.Name.ToLower().EndsWith(".jpeg") || f.Name.ToLower().EndsWith(".jfif");
		}

		/// <summary>
		/// Get height and width with Bitmap
		/// </summary>
		/// <param name="f"></param>
		/// <param name="height"></param>
		/// <param name="width"></param>
		private static void GetBitmapHeightWidth(FileInfo f, out int height, out int width)
		{
			/*using (var img = new Bitmap(f.FullName))
            {
                if (img != null)
                {
                    height = img.Height;
                    width = img.Width;
                }
            }*/

			try
			{
				//read only the header, no additionnal reference, faster
				using (var file = new FileStream(f.FullName, FileMode.Open, FileAccess.Read))
				{
					using (Image img = Image.FromStream(stream: file,
														useEmbeddedColorManagement: false,
														validateImageData: false))
					{
						width = (int)img.PhysicalDimension.Width;
						height = (int)img.PhysicalDimension.Height;
					}
				}
			}
			catch
			{
				width = 0;
				height = 0;
			}
		}

		private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			//Open image on left mouse double click (but not on header)
			if (e.RowIndex >= 0 && e.Button == MouseButtons.Left)
			{
				var name = dataGridView1.CurrentRow.Cells[0].Value;

				var psi = new ProcessStartInfo(imagesPath.Text + "\\" + name)
				{
					UseShellExecute = true
				};
				Process.Start(psi);
			}
		}

		private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			//Sort columns file size and ratio numerically
			if (e.Column.Index == 1 || e.Column.Index == 2)
			{
				e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
				e.Handled = true;//pass by the default sorting
			}
			//Sort column dimensions
			else if (e.Column.Index == 3)
			{
				//Use hidden column nbPixels to sort numerically
				var nbPixelsCell1 = dataGridView1[4, e.RowIndex1].Value.ToString();
				var nbPixelsCell2 = dataGridView1[4, e.RowIndex2].Value.ToString();
				e.SortResult = int.Parse(nbPixelsCell1).CompareTo(int.Parse(nbPixelsCell2));
				e.Handled = true;//pass by the default sorting
			}
		}

		private void SelectPathButton_Click(object sender, EventArgs e)
		{
			var folderBrowserDialog = new FolderBrowserDialog
			{
				SelectedPath = imagesPath.Text
			};

			DialogResult result = folderBrowserDialog.ShowDialog();

			if (result == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
			{
				imagesPath.Text = folderBrowserDialog.SelectedPath;
			}
		}

		private void GetRatiosButton2_Click(object sender, EventArgs e)
		{
			//GetRatios();

			if (backgroundWorker1.IsBusy != true)
			{
				GetRatiosButton.Text = "Cancel";

				GetRatiosInit();

				// Start the asynchronous operation.
				backgroundWorker1.RunWorkerAsync();
			}
			else
			{
				// Cancel the asynchronous operation.
				backgroundWorker1.CancelAsync();
			}
		}

		private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
		{
			//Go to first row
			if (e.KeyCode == Keys.Home)
			{
				dataGridView1.CurrentCell = dataGridView1[0, 0];
				e.Handled = true;
			}

			//Go to last row with data (not the last empty row)
			if (e.KeyCode == Keys.End)
			{
				dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.RowCount - 2];
				e.Handled = true;
			}
		}
	}
}