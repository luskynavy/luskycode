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
                double diag = double.Parse(size.Text);
                double width = double.Parse(resolutionX.Text);
                distance.Text = (diag).ToString() /* + radioButton1.Checked + radioButton2.Checked + radioButton3.Checked*/;
            }
            catch
            {
            }

        }
    }
}
