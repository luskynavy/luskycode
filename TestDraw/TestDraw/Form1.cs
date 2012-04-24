using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace TestDraw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics formGraphics = this.CreateGraphics();
            formGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Pen myPen;
            myPen = new Pen(Color.Red, 2);
            
            formGraphics.DrawRectangle(myPen, new Rectangle(0, 0, 200, 300));
            
            formGraphics.FillRectangle(Brushes.Orange, new Rectangle(25, 50, 100, 200));

            myPen.Width = 1;
            myPen.DashStyle = DashStyle.DashDot;

            formGraphics.DrawRectangle(myPen, new Rectangle(1, 1, 50, 50));

            Brush myBrush;
            myBrush = new HatchBrush(HatchStyle.SolidDiamond,
               Color.Red, Color.Black);
            formGraphics.FillRectangle(myBrush, new Rectangle(50, 50, 100, 100));

            LinearGradientBrush gradient = new LinearGradientBrush(new Rectangle(0, 0, 100, 100),
                Color.Blue, Color.Yellow, LinearGradientMode.ForwardDiagonal);
            formGraphics.FillEllipse(gradient, new Rectangle(50, 150, 100, 100));

            //Pacman !!!!
            Brush myBrush2;
            myBrush2 = new SolidBrush(Color.Yellow);
            formGraphics.FillPie(myBrush2, new Rectangle(25, 0, 100, 100), 45, 270);
            
            Font drawFont = new Font("Arial", 16);
            formGraphics.DrawString("PACMAN", drawFont, gradient, 100, 250);


            drawFont.Dispose();
            myBrush2.Dispose();
            myBrush.Dispose();
            myPen.Dispose();
            formGraphics.Dispose();
        }
    }
}
