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
            label4.Text = "W\ni\nd\nt\nh";
            label5.Text = "H\ne\ni\ng\nh\nt";

            //set default values
            desiredWidth.Value = 240;
            desiredHeight.Value = 320;            
            imagesPath.Text = @"E:\Users\yvan.kalafatov\Downloads\t\ss.php5_fichiers";
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
            int scaledWidth, scaledHeight;

            //for each file
            foreach (FileInfo f in files)
            {
                //read image
                im = new Bitmap(f.FullName);

                //compute scaled size
                scaledWidth = im.Width * (int)desiredHeight.Value / im.Height;
                scaledHeight = im.Height * (int)desiredWidth.Value / im.Width;

                //if scaled image don't fit in width, put in height result rich text box
                if (scaledWidth > desiredWidth.Value)
                {
                    richTextBoxResultsW.Text += f.Name /*+ " " + im.Width + " x " + im.Height + " " + desiredWidth.Text + " x " + scaledHeight + " " + scaledWidth + " x " + desiredHeight.Text*/ + "\r\n";
                }
                //else in width result rich text box
                else
                {
                    richTextBoxResultsH.Text += f.Name /*+ " " + im.Width + " x " + im.Height + " " + desiredWidth.Text + " x " + scaledHeight + " " + scaledWidth + " x " + desiredHeight.Text*/ + "\r\n";
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
