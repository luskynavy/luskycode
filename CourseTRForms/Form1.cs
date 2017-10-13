using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;

namespace CourseTRForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            values.Text = "2,41; 3,24; 2,09; 2,56; 3,28; 3,88; 1,70; 4,93; 3,30";            
            wantedSum.Text = (2.41 + 2.09 + 3.3).ToString();
        }

        string subsetSet(double[] products, double wantedSum)
        {
            string res = "";

            for (int testVal = 0; testVal < Math.Pow(2, products.Length); testVal++)
            {
                double sum = 0;
                for (int choice = 0; choice < products.Length; choice++)
                {
                    if (((1 << choice) & testVal) == (1 << choice))
                    {
                        sum += products[choice];
                    }
                }

                //if (sum == 2.41 + 2.09 + 3.3)
                //if (sum == wantedSumDouble)
                if (Math.Abs(sum - wantedSum) < 1e-5)
                {
                    for (int j = 0; j < products.Length; j++)
                    {
                        if (((1 << j) & testVal) == (1 << j))
                        {
                            res += " " + products[j];
                        }
                    }
                    res += "\r\n";
                }
            }

            return res;
        }

        private void search_Click(object sender, EventArgs e)
        {            
            //double[] products = { 2.41, 3.24, 2.09, 2.56, 3.28, 3.88, 1.70, 4.93, 3.30 };
            /*product
            double[] products = new double[];

            int i = 0;
            foreach(string d in values.Text.Split(';'))
            {
                products[i] = double.Parse(d);
                i++;
            }*/

            Stopwatch stopWatch = Stopwatch.StartNew();

            double[] products = values.Text.Split(';').Select(Double.Parse).ToArray();

            double wantedSumDouble = double.Parse(wantedSum.Text);

            results.Text = subsetSet(products, wantedSumDouble);

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            //TimeSpan ts = stopWatch.Elapsed;
            long ts = stopWatch.ElapsedMilliseconds;

            // Format and display the TimeSpan value.
            /*string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);*/
            string elapsedTime = ((double)ts / 1000).ToString();

            results.Text += "Done for " + products.Length + " values  in " + elapsedTime + " s";
        }

       /*[Test]
        public void testSubsetSet()
        {
            double[] products = { 2.41, 3.24, 2.09, 2.56, 3.28, 3.88, 1.70, 4.93, 3.30 };
            string actualResult = subsetSet(products, 10);
            string expectedResult = "";

            Assert.AreEqual(expectedResult, actualResult);
        }*/
    }
   
}
