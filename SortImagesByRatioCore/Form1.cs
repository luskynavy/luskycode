using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using System.IO;
using ExifLib;

//using System.Windows.Media.Imaging.BitmapDecoder;

/***************************************************************************************
 * Determine if jpeg are best scaled in portrait or landscape (with 90 degree rotation)
 * desired width, size and path are parametrables
***************************************************************************************/

namespace SortImagesByRatioCore
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			//set label in vertical
			label4.Text = "N\no\nR\no\nt\na\nt\ne";
			label5.Text = "R\no\nt\na\nt\ne";

			//set default values
			desiredWidth.Value = 720;
			desiredHeight.Value = 1520;
			imagesPath.Text = @"D:\d\.wp\wp";
		}

		//resize from width * height to wantedWidth * wantedHeight keeping ratio
		private Size ResizeTo(int width, int height, int wantedWidth, int wantedHeight)
		{
			double scaledWidth, scaledHeight;
			Size wanted = new Size();
			//compute ratio
			double ratio = (double)width / height;

			//compute scaled sizes
			scaledHeight = (double)wantedWidth / ratio;
			scaledWidth = (double)wantedHeight * ratio;

			//if scaling in height is too big
			if (scaledHeight > wantedHeight)
			{
				wanted.Width = (int)scaledWidth;
				wanted.Height = wantedHeight;
			}
			else
			{
				wanted.Width = wantedWidth;
				wanted.Height = (int)scaledHeight;
			}

			return wanted;
		}

        /// <summary>
        /// Get height and width with ExifReader
        /// </summary>
        /// <param name="f"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        private static void GetExifReaderHeightWidth(FileInfo f, out int height, out int width)
        {
            //open file in readonly non locking file
            var stream = f.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            var exif = ExifReader.ReadJpeg(stream);
            height = exif.Height;
            width = exif.Width;

            stream.Close();
        }

        private void button1_Click(object sender, EventArgs e)
		{
			//get files list
			DirectoryInfo dir = new DirectoryInfo(imagesPath.Text);
			//FileInfo[] files = dir.GetFiles("*.jpg");
			IEnumerable<FileInfo> files = dir.GetFiles("*.jpeg").Union(dir.GetFiles("*.jpg")).Union(dir.GetFiles("*.png")).Union(dir.GetFiles("*.gif")).Union(dir.GetFiles("*.bmp"));

			//clear results
			richTextBoxResultsH.Clear();
			richTextBoxResultsW.Clear();
			//int scaledWidth, scaledHeight;

			//for each file
			foreach (FileInfo f in files)
			//foreach (string f in files)
			{
				try
				{
					//read the whole image to get the size, bad
					//Bitmap im = new Bitmap(f.FullName);

					Size im = new Size();

                    //ExifReader, faster than reading whole image but errors with some jpg
                    /*int height = 0, width = 0;
					GetExifReaderHeightWidth(f, out height, out width);
					im.Height = height;
					im.Width = width;*/

					//read only the header, no additionnal reference, faster
                    using (FileStream file = new FileStream(f.FullName, FileMode.Open, FileAccess.Read))
					{
						using (Image tif = Image.FromStream(stream: file,
															useEmbeddedColorManagement: false,
															validateImageData: false))
						{
							im.Width = (int)tif.PhysicalDimension.Width;
							im.Height = (int)tif.PhysicalDimension.Height;
						}
					}

					//read only the header, require additionnal wpf reference
					/*using (var imageStream = File.OpenRead(f.FullName))
					{
						var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile,
							BitmapCacheOption.Default);
						im.height = decoder.Frames[0].PixelHeight;
						im.width = decoder.Frames[0].PixelWidth;
					}*/

					//compute scaled size
					Size noRotate = ResizeTo(im.Width, im.Height, (int)desiredWidth.Value, (int)desiredHeight.Value);
					Size rotate = ResizeTo(im.Height, im.Width, (int)desiredWidth.Value, (int)desiredHeight.Value);

					/*scaledWidth = im.Width * (int)desiredHeight.Value / im.Height;
					scaledHeight = im.Height * (int)desiredWidth.Value / im.Width;
					*/
					//if scaled image don't fit in width, put in height result rich text box
					//if (scaledWidth > desiredWidth.Value)

					//if scaled image has more surface with no rotation
					if (noRotate.Width * noRotate.Height >= rotate.Width * rotate.Height)
					{
						richTextBoxResultsW.Text += f.Name //+ " " + im.Width + " x " + im.Height + " " + desiredWidth.Text + " x " + scaledHeight + " " + scaledWidth + " x " + desiredHeight.Text
							+ "\r\n";
					}
					//else in height result rich text box
					else
					{
						richTextBoxResultsH.Text += f.Name //+ " " + im.Width + " x " + im.Height + " " + desiredWidth.Text + " x " + scaledHeight + " " + scaledWidth + " x " + desiredHeight.Text
							+ "\r\n";
					}
				}
				//hum, continue ?
				catch
				{
				}
			}
		}

		//show the folder selector for image path
		private void buttonSelectPath_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = imagesPath.Text;

			DialogResult result = folderBrowserDialog.ShowDialog();

			if (result == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
			{
				imagesPath.Text = folderBrowserDialog.SelectedPath;
			}
		}
	}
}