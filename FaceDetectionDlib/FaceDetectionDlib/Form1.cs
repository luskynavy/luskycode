using DlibDotNet;

namespace FaceDetectionDlib
{
    public partial class Form1 : Form
    {
        private DlibDotNet.Rectangle[] _rec;
        private OpenFileDialog _openFileDialog1;

        // Detect face (or body or upper body) on images
        //Left click to choose an image
        //Middle click to put rabbit ears and nose on detected face
        //Right click to zoom

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Load("Untitled.jpg");

            DetectAndDraw();
        }

        private void DetectAndDraw()
        {
            var img = Dlib.LoadImage<byte>(pictureBox1.ImageLocation);

            //Dlib.PyramidUp(img);

            var detector = Dlib.GetFrontalFaceDetector();

            _rec = detector.Operator(img);

            //the pens
            var pr = new Pen(Color.Red, 1);
            var pg = new Pen(Color.Green, 1);

            //draw rectangles around faces
            using var gr = Graphics.FromImage(pictureBox1.Image);
            foreach (var r in _rec)
            {
                var rect = new System.Drawing.Rectangle { X = r.Left, Y = r.Top, Width = (int)r.Width, Height = (int)r.Height };
                //draw the face
                gr.DrawRectangle(pr, rect);

                //draw the portrait around the face
                var b = GetPortrait(rect);
                gr.DrawRectangle(pg, b);
            }
        }

        private System.Drawing.Rectangle GetPortrait(System.Drawing.Rectangle r)
        {
            int newWidth = (int)(r.Height * 7 / 2 * .63) /*r.Width * 2*/;
            int newHeight = r.Height * 7 / 2;
            var b = new System.Drawing.Rectangle(r.X + r.Width / 2 - newWidth / 2, r.Y - r.Height / 4, newWidth, newHeight);
            return b;
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

                        var earRec = new System.Drawing.Rectangle((int)(rec.Left - rec.Width / 2), (int)(rec.Top - rec.Height * 4 / 4), (int)rec.Width * 2, (int)rec.Height * 2);
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
                var nb = new Bitmap(210, 330);
                var g = Graphics.FromImage(nb);
                var rect = new System.Drawing.Rectangle { X = _rec[0].Left, Y = _rec[0].Top, Width = (int)_rec[0].Width, Height = (int)_rec[0].Height };
                System.Drawing.Rectangle b = GetPortrait(rect);
                g.DrawImage(pictureBox1.Image, new System.Drawing.Rectangle(0, 0, 210, 330), b, GraphicsUnit.Pixel);
                pictureBox1.Image = nb;
            }
        }
    }
}