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

//how to convert it with ImageMagick 7 and known offset and size
//convert clxE708.rgb -size 1680x1010+40 -channel bgra -depth 24 test.png

namespace ClipXView
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialog1;

        public Form1(string[] args)
        {
            InitializeComponent();
            openFileDialog1 = new OpenFileDialog();


            //LoadClipX(@"E:\Users\yvan.kalafatov\Downloads\t\clxE708.rgb");

            //save to jpeg all files from directory
            //string path = System.IO.Path.GetTempPath();
            //path = @"E:\Users\yvan.kalafatov\AppData\Local\Temp";
            string path = @"C:\Users\Lusky\AppData\Local\ClipX\Storage";
            if (args.Length >= 1)
            {
                path = args[0];
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles("clx*.tmp");
            foreach (FileInfo f in files)
            {
                LoadClipX(f.FullName);
                openFileDialog1.FileName = f.FullName;
                pictureBox1_RightClick(Application.StartupPath + "\\");
            }
        }

        private void LoadClipX(string fileClipX)
        {
            //load the file
            using (var filestream = File.Open(fileClipX, FileMode.Open))
            using (var binaryStream = new BinaryReader(filestream))
            {
                int offset = binaryStream.ReadInt32(); //read offset
                int width = binaryStream.ReadInt32(); //then width
                int height = -binaryStream.ReadInt32(); //then height
                filestream.Seek(offset, SeekOrigin.Begin); //move to offset

                //create the image buffer and load it
                byte[] b = new byte[filestream.Length - offset]; //
                filestream.Read(b, 0, b.Length);

                /*BitmapData myReadingBuffer = new BitmapData();
                myReadingBuffer.Scan0 = Marshal.UnsafeAddrOfPinnedArrayElement(b, 0);
                myReadingBuffer.Height = height;
                myReadingBuffer.Width = width;
                myReadingBuffer.PixelFormat = PixelFormat.Format48bppRgb;
                myReadingBuffer.Stride = width;*/

                //set the image
                pictureBox1.Image = new Bitmap(width, height,
                    width*4/*stride*/, 
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                    Marshal.UnsafeAddrOfPinnedArrayElement(b, 0));
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //right click save the image
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                pictureBox1_RightClick();
            }
            //left click ask a file and load it
            else
            {                
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

        private void pictureBox1_RightClick(string path = null)
        {
            //create a jpeg encoder
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            //param for jpeg with 90% quality
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;  
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            //save the image adding ".jpg" to original name
            if (path == null)
            {
                pictureBox1.Image.Save(openFileDialog1.FileName + ".jpg", jpgEncoder/*ImageFormat.Jpeg*/, myEncoderParameters);
            }
            else
            {
                pictureBox1.Image.Save(path + Path.GetFileName(openFileDialog1.FileName) + ".jpg", jpgEncoder/*ImageFormat.Jpeg*/, myEncoderParameters);
            }
            
        }

        //find the wanted encoder
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
