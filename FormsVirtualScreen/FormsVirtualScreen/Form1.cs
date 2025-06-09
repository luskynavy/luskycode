using System;
using System.Drawing;
using System.Windows.Forms;

namespace FormsVirtualScreen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Window position with window borders
            var rect = new Rectangle(Left, Top, Width, Height);

            //If real rectangle checkbox is enabled, compute real rectangle of window
            //without border but with window bar
            if (checkBoxRealReactangle.Checked)
            {
                //Border of left and right for resize
                int widthBorder = Width - ClientRectangle.Width;
                //int heightBorder = Height - ClientRectangle.Height;
                rect = new Rectangle(
                    Left + widthBorder / 2,
                    Top + widthBorder / 2,
                    Width - widthBorder,
                    Height - widthBorder);
            }

            var rectStatus = "";
            //Find monitor  with largest window part
            //or closest monitor if not on any monitor
            var screen = Screen.FromControl(this);

            //Check if rect is inside work area of monitor
            if (screen != null)
            {
                if (rect.Left >= screen.WorkingArea.Left &&
                    rect.Top >= screen.WorkingArea.Top &&
                    rect.Right <= screen.WorkingArea.Right &&
                    rect.Bottom <= screen.WorkingArea.Bottom)
                {
                    rectStatus = "fully inside one screen";
                } else
                {
                    rectStatus = "not fully inside one screen";
                }
            } else
            {
                rectStatus = "not on any screen";
            }

            infos.Text = $"VirtualScreen.Width {SystemInformation.VirtualScreen.Width}\r\n" +
                $"VirtualScreen.Height {SystemInformation.VirtualScreen.Height}\r\n" +
                $"VirtualScreen.Left {SystemInformation.VirtualScreen.Left}\r\n" +
                $"VirtualScreen.Top {SystemInformation.VirtualScreen.Top}\r\n" +
                $"Left {rect.Left}\r\n" +
                $"Top {rect.Top}\r\n" +
                $"Right {rect.Right}\r\n" +
                $"Bottom {rect.Bottom}\r\n" +
                $"{rectStatus}";
        }
    }
}
