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

namespace ClipXView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            LoadClipX(@"E:\Users\yvan.kalafatov\Downloads\t\clxE708.rgb");

            /*DirectoryInfo dir = new DirectoryInfo(@"E:\Users\yvan.kalafatov\Downloads\t\");
            FileInfo[] files = dir.GetFiles("*.rgb");
            foreach (FileInfo f in files)
            {
                LoadClipX(f.FullName);
            }*/
        }

        private void LoadClipX(string fileClipX)
        {
            
            using (var filestream = File.Open(fileClipX, FileMode.Open))
            using (var binaryStream = new BinaryReader(filestream))
            {
                int offset = binaryStream.ReadInt32();
                int width = binaryStream.ReadInt32();
                int height = -binaryStream.ReadInt32();

                byte[] b = new byte[filestream.Length - offset];                    
                filestream.Read(b, 0, b.Length);

                /*BitmapData myReadingBuffer = new BitmapData();
                myReadingBuffer.Scan0 = Marshal.UnsafeAddrOfPinnedArrayElement(b, 0);
                myReadingBuffer.Height = height;
                myReadingBuffer.Width = width;
                myReadingBuffer.PixelFormat = PixelFormat.Format48bppRgb;
                myReadingBuffer.Stride = width;*/

                pictureBox1.Image = new Bitmap(width, height,
                    width*4/*stride*/, 
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                    Marshal.UnsafeAddrOfPinnedArrayElement(b, 0));
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1;
            /*OpenFileDialog*/
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = ".";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            //openFileDialog1.RestoreDirectory = true ;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadClipX(openFileDialog1.FileName);
            }
        }
    }
}
