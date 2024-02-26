using System;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection
{
    public partial class Form1 : Form
    {
        private CascadeClassifier _cascadeClassifier;
        private Rectangle[] _rec;
        private OpenFileDialog _openFileDialog1;

        // Detect face (or body or upper body) on images
        //Left click to choose an image
        //Middle click to put rabbit ears and nose on detected face
        //Right click to zoom

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
            //_cascadeClassifier = new CascadeClassifier("haarcascade_fullbody.xml");
            //_cascadeClassifier = new CascadeClassifier("haarcascade_upperbody.xml");

            pictureBox1.Load("test.jpg");

            DetectAndDraw();
        }

        private Rectangle[] DetectFaces()
        {
            var image = new Image<Gray, byte>(new Bitmap(pictureBox1.Image));
            Rectangle[] face = _cascadeClassifier.DetectMultiScale(image, 1.1, 10, new Size(20, 20), Size.Empty); //the actual face detection happens here
            //Rectangle[] face = _cascadeClassifier.DetectMultiScale(image, 1.1, 1, new Size(50, 100), Size.Empty); //the actual face detection happens here

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
            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            if (mouseEvent.Button == MouseButtons.Right)
            {
                pictureBox1_RightClick();
            }
            else if (mouseEvent.Button == MouseButtons.Middle)
            {
                //Add bunny ears and nose to each face found
                if (_rec.Length != 0)
                {
                    var ears = new Bitmap("bunny.png");

                    foreach (var rec in _rec)
                    {
                        var gr = Graphics.FromImage(pictureBox1.Image);

                        var earRec = new System.Drawing.Rectangle((int)(rec.Left - rec.Width / 2), rec.Top - rec.Height * 4 / 4, rec.Width * 2, rec.Height * 2);
                        gr.DrawImage(ears, earRec);
                    }

                    pictureBox1.Refresh();
                }
            }
            else
            {
                _openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = ".",
                    Filter = "All files (*.*)|*.*|images (*.jpg;*.jpeg;*.bmp;*.gif;*.png)|*.jpg;*.jpeg;*.bmp;*.gif;*.png",
                    FilterIndex = 2
                };
                //openFileDialog1.RestoreDirectory = true ;

                if (_openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Load(_openFileDialog1.FileName);

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