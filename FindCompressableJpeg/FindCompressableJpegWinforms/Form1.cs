using System.Diagnostics;
using System.Reflection;

namespace FindCompressableJpegWinforms
{
    public partial class Form1 : Form
    {
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
        }

        private void InitDataGrid()
        {
            string[] args = Environment.GetCommandLineArgs();

            //First argument is program name then parameters
            //Use first parameter as directory
            if (args.Length <= 1)
            {
                imagesPath.Text = Directory.GetCurrentDirectory();
            }
            else
            {
                imagesPath.Text = args[1];
            }

            GetRatios();
        }

        private void GetRatios()
        {
            var dir = new DirectoryInfo(imagesPath.Text);
            FileInfo[] files = dir.GetFiles();

            dataGridView1.Rows.Clear();

            foreach (FileInfo file in files)
            {
                if (FilterJpeg(file))
                {
                    //Get dimensions
                    int width, height;
                    GetBitmapHeightWidth(file, out height, out width);

                    //Compute ratio file size / 1024 pixels
                    var nbPixels = height * width / 1024;
                    var sizeFor1024Pixel = file.Length / (nbPixels != 0 ? nbPixels : 1);

                    string[] row = { file.Name, file.Length.ToString(), sizeFor1024Pixel.ToString(), $"{width} x {height}" };
                    dataGridView1.Rows.Add(row);
                }
            }

            //Sort descending on ratio
            dataGridView1.Sort(dataGridView1.Columns[2], System.ComponentModel.ListSortDirection.Descending);
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

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Open image on left mouse double click
            if (e.Button == MouseButtons.Left)
            {
                var name = dataGridView1.CurrentRow.Cells[0].Value;

                ProcessStartInfo psi = new ProcessStartInfo(imagesPath.Text + "\\" + name);
                psi.UseShellExecute = true;
                Process.Start(psi);
            }
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //Sort file size and ratio numerically
            if (e.Column.Index == 1 || e.Column.Index == 2)
            {
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;//pass by the default sorting
            }
        }

        private void SelectPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = imagesPath.Text;

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
            {
                imagesPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void GetRatiosButton2_Click(object sender, EventArgs e)
        {
            GetRatios();
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