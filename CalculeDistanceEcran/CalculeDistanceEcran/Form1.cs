using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CalculeDistanceEcran
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                double diag = double.Parse(size.Text); //diagonal in inches
                double width = double.Parse(resolutionX.Text); //width in pixels
                double ratio;

                if (radioButton1610.Checked)
                {
                    ratio = 16.0 / 10;
                }
                else if (radioButton169.Checked)
                {
                    ratio = 16.0 / 9;
                }
                else
                {
                    ratio = 4.0 / 3;
                }

                double p = Math.Sqrt(diag*diag/(1+1/(ratio*ratio))) * 2.54 / width; //width of a pixel
                double d = p / 2 / Math.Tan(Math.PI / 180 / 60 / 2);
                distance.Text = (Math.Round(d, 2)).ToString() ;
            }
            catch
            {
            }
        }
    }
}
