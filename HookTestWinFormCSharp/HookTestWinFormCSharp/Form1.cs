using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HookTestWinFormCSharp
{
    public partial class Form1 : Form
    {
        public int volume = 50;

        public Form1()
        {
            InitializeComponent();

            Hooks hook = new Hooks();

            hook.KeyDown += new KeyEventHandler(hook_KeyDown);            
        }

        void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.MediaPlayPause)
            {
                webBrowser1.Navigate("javascript:top.player.onPlayPause();");
                //webBrowser1.Navigate("javascript:alert(top.player.song);top.player.onPlayPause();");
                //MessageBox.Show("pause");
                //throw new NotImplementedException();
            }

            if (e.KeyData == Keys.MediaNextTrack)
            {
                webBrowser1.Navigate("javascript:top.player.onSkip(true);");
                //webBrowser1.Navigate("javascript:top.player.setTimeout('top.player.onSkip(true)',0);");
                //MessageBox.Show("pause");
                //throw new NotImplementedException();
            }

            if (e.KeyData == Keys.VolumeUp)
            {
                volume += 10;
                if (volume > 100)
                {
                    volume = 100;
                }
                webBrowser1.Navigate("javascript:top.player.onVolume(" + volume + ");");
            }

            if (e.KeyData == Keys.VolumeDown)
            {
                volume -= 10;
                if (volume < 0)
                {
                    volume = 0;
                }
                webBrowser1.Navigate("javascript:top.player.onVolume(" + volume + ");");
            }

            /*            if (e.Control && e.Alt && e.KeyData == Keys.A)
                        {
                            MessageBox.Show("ctrl+alt+a");
                        }
            */
        }

    }
}
