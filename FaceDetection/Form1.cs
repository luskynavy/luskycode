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
        private Rectangle[] _rec;
        OpenFileDialog openFileDialog1;

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
            Rectangle[] face = _cascadeClassifier.DetectMultiScale(image, 1.1, 10, new Size(20,20), Size.Empty); //the actual face detection happens here

            return face;
        }

        private Rectangle GetPortrait(Rectangle r)
        {
            int newWidth = (int)(r.Height * 7 / 2 * .63) /*r.Width * 2*/;
            int newHeight = r.Height * 7 / 2;
            Rectangle b = new Rectangle(r.X + r.Width / 2 - newWidth / 2, r.Y - r.Height / 4, newWidth, newHeight);
            return b;
        }

        private void DetectAndDraw()
        {
            //scale down big images to avoid pc freezes
            int maxHeight = 1200;
            int maxWidth = 1200;

            if (pictureBox1.Image.Height > maxHeight)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Image, new Size(pictureBox1.Image.Width * maxHeight / pictureBox1.Image.Height, maxHeight));
            }

            if (pictureBox1.Image.Width > maxWidth)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Image, new Size(maxWidth, pictureBox1.Image.Height * maxWidth / pictureBox1.Image.Width));
            }

            //detect the faces
            _rec = DetectFaces();

            //the pens
            Pen pr = new Pen(Color.Red, 1);
            Pen pg = new Pen(Color.Green, 1);            

            //draw rectangles around faces
            using (Graphics gr = Graphics.FromImage(pictureBox1.Image))
            {
                foreach (var r in _rec)
                {
                    //draw the face
                    gr.DrawRectangle(pr, r);
                    

                    //draw the portrait around the face
                    Rectangle b = GetPortrait(r);
                    gr.DrawRectangle(pg, b);                                        
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                pictureBox1_RightClick();
            }
            else if (((MouseEventArgs)e).Button == MouseButtons.Middle)
            {
                //add bunny ears and nose to the first face found
                if (_rec.Length != 0)
                {
                    var ears = new Bitmap("bunny.png");

                    Graphics gr = Graphics.FromImage(pictureBox1.Image);

                    Rectangle earRec = new Rectangle(_rec[0].X - _rec[0].Width / 2, _rec[0].Y - _rec[0].Height * 4 / 4, _rec[0].Width * 2, _rec[0].Height * 2);
                    gr.DrawImage(ears, earRec);

                    pictureBox1.Refresh();
                }
            }
            else
            {
                /*OpenFileDialog*/ openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = ".";
                openFileDialog1.Filter = "All files (*.*)|*.*|images (*.jpg;*.jpeg;*.bmp;*.gif;*.png)|*.jpg;*.jpeg;*.bmp;*.gif;*.png";
                openFileDialog1.FilterIndex = 2;
                //openFileDialog1.RestoreDirectory = true ;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Load(openFileDialog1.FileName);

                    DetectAndDraw();
                }
            }
        }

        private void pictureBox1_RightClick()
        {
            if (_rec.Length != 0)
            {
                //pictureBox1.Load(openFileDialog1.FileName);
                Bitmap nb = new Bitmap(210, 330);
                Graphics g = Graphics.FromImage(nb);
                Rectangle b = GetPortrait(_rec[0]);
                g.DrawImage(pictureBox1.Image, new Rectangle(0, 0, 210, 330), b, GraphicsUnit.Pixel);
                pictureBox1.Image = nb;
            }
        }
    }
}
