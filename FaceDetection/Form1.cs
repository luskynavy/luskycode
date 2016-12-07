using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;



namespace FaceDetection
{
    public partial class Form1 : Form
    {
        private CascadeClassifier _cascadeClassifier;        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");

            pictureBox1.Load("test.jpg");

            DetectAndDraw();
        }

        private Rectangle[] DetectFaces()
        {
            var image = new Image<Gray, byte>(new Bitmap(pictureBox1.Image));
            Rectangle[] face = _cascadeClassifier.DetectMultiScale(image, 1.1, 10, Size.Empty, Size.Empty); //the actual face detection happens here

            return face;
        }

        private void DetectAndDraw()
        {
            //scale down big images to avoid pc freezes
            int maxHeight = 900;
            int maxWidth = 900;

            if (pictureBox1.Image.Height > maxHeight)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Image, new Size(pictureBox1.Image.Width * maxHeight / pictureBox1.Image.Height, maxHeight));
            }

            if (pictureBox1.Image.Width > maxWidth)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Image, new Size(maxWidth, pictureBox1.Image.Height * maxWidth / pictureBox1.Image.Width));
            }

            //detect the faces
            Rectangle[] rec = DetectFaces();

            //the pens
            Pen pr = new Pen(Color.Red, 1);
            Pen pg = new Pen(Color.Green, 1);

            //draw rectangles around faces
            using (Graphics gr = Graphics.FromImage(pictureBox1.Image))
            {
                foreach (var r in rec)
                {
                    //draw the face
                    gr.DrawRectangle(pr, r);

                    //draw the portrait around the face
                    int newWidth = (int)(r.Height * 7 / 2 * .63) /*r.Width * 2*/;
                    int newHeight = r.Height * 7 / 2;
                    Rectangle b = new Rectangle(r.X + r.Width / 2 - newWidth / 2, r.Y - r.Height / 4, newWidth, newHeight);
                    gr.DrawRectangle(pg, b);                                        
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "." ;
            openFileDialog1.Filter = "All files (*.*)|*.*|images (*.jpg;*.jpeg;*.bmp;*.gif;*.png)|*.jpg;*.jpeg;*.bmp;*.gif;*.png";
            openFileDialog1.FilterIndex = 2 ;
            //openFileDialog1.RestoreDirectory = true ;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);

                DetectAndDraw();
            }
        }
    }
}
