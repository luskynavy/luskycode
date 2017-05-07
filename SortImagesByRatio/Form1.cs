using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

/***************************************************************************************
 * Determine if jpeg are best scaled in portrait or landscape (with 90 degree rotation)
 * desired width, size and path are parametrables
***************************************************************************************/

namespace SortImagesByRatio
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
            desiredWidth.Value = 240;
            desiredHeight.Value = 320;
            imagesPath.Text = @"D:\Data\Music\portraits\c";
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

        private void button1_Click(object sender, EventArgs e)
        {
            //get files list
            DirectoryInfo dir = new DirectoryInfo(imagesPath.Text);
            FileInfo[] files = dir.GetFiles("*.jpg");
            Bitmap im;
            
            //clear results
            richTextBoxResultsH.Clear();
            richTextBoxResultsW.Clear();
            //int scaledWidth, scaledHeight;

            //for each file
            foreach (FileInfo f in files)
            {
                try
                {
                    //read image
                    im = new Bitmap(f.FullName);

                    //compute scaled size
                    Size noRotate = ResizeTo(im.Width, im.Height, (int)desiredWidth.Value, (int)desiredHeight.Value);
                    Size rotate = ResizeTo(im.Height, im.Width, (int)desiredWidth.Value, (int)desiredHeight.Value);

                    /*scaledWidth = im.Width * (int)desiredHeight.Value / im.Height;
                    scaledHeight = im.Height * (int)desiredWidth.Value / im.Width;
                    */
                    //if scaled image don't fit in width, put in height result rich text box
                    //if (scaledWidth > desiredWidth.Value)

                    //if scaled image has more surface with no rotation
                    if (noRotate.Width * noRotate.Height >+ rotate.Width * rotate.Height)
                    {
                        richTextBoxResultsW.Text += f.Name /*+ " " + im.Width + " x " + im.Height + " " + desiredWidth.Text + " x " + scaledHeight + " " + scaledWidth + " x " + desiredHeight.Text*/ + "\r\n";
                    }
                    //else in height result rich text box
                    else
                    {
                        richTextBoxResultsH.Text += f.Name /*+ " " + im.Width + " x " + im.Height + " " + desiredWidth.Text + " x " + scaledHeight + " " + scaledWidth + " x " + desiredHeight.Text*/ + "\r\n";
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
