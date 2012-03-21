using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;


//http://forum.codecall.net/csharp-tutorials/20420-tutorial-playing-mp3-files-c.html#post199646

namespace TestMP3
{
    public partial class Form1 : Form
    {
        bool playing = false;
        bool paused = false;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public Form1()
        {
            InitializeComponent();
        }

        private void play_Click(object sender, EventArgs e)
        {
            if (!playing)
            {
                long ret = mciSendString("open \"" + filenameSong.Text /*"D:/Data/Music/Jango/Celldweller - Switchback.mp3"*/ + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
                ret = mciSendString("play MediaFile", null, 0, IntPtr.Zero);

                playing = true;
                play.Text = "Stop";
                pause.Enabled = true;
            }
            else
            {
                mciSendString("close MediaFile", null, 0, IntPtr.Zero);

                playing = false;
                play.Text = "Play";
                pause.Enabled = false;
            }
        }

        private void pause_Click(object sender, EventArgs e)
        {
            if (!paused)
            {
                mciSendString("pause MediaFile", null, 0, IntPtr.Zero);

                paused = true;
                pause.Text = "Resume";
            }
            else
            {
                mciSendString("resume MediaFile", null, 0, IntPtr.Zero);

                paused = false;
                pause.Text = "Pause";
            }
        }

        private void browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filenameSong.Text = openFileDialog1.FileName;                
            }
        }
    }
}
