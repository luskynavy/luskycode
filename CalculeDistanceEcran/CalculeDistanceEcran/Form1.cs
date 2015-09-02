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
                double diag = double.Parse(size.Text.Replace('.', ',')); //diagonal in inches and allow '.' for decimal separator
                double width = double.Parse(resolutionX.Text); //width in pixels
                double ratio;

                //get ratio from radioButtons
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

                double witdhInches = Math.Sqrt(diag * diag / (1 + 1 / (ratio * ratio))); //with in inches of screen
                double p = witdhInches * 2.54 / width; //width of a pixel
                double d = p / 2 / Math.Tan(Math.PI / 180 / 60 / 2); //optimal distance to screen
                distance.Text = (Math.Round(d, 2)).ToString(); //only 2 digits for decimal

                //diagonal size in cm
                sizeCm.Text = (Math.Round(double.Parse(size.Text) * 2.54, 2)).ToString() + " cm";
                //width in cm
                widthCm.Text = (Math.Round(witdhInches * 2.54, 2)).ToString() + " cm";
                //height in cm
                heightCm.Text = "* " + (Math.Round(witdhInches /ratio * 2.54, 2)).ToString() + " cm";
            }
            catch
            {
            }
        }                
    }
}
