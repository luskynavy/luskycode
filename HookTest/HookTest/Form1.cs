using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HookTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //ClearCache();

            InitializeComponent();

            Hooks hook = new Hooks();

            hook.KeyDown += new KeyEventHandler(hook_KeyDown);

            //webBrowser1.Refresh(System.Windows.Forms.WebBrowserRefreshOption.Normal);            
        }

        void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.MediaPlayPause)
            {
                webBrowser1.Navigate("javascript:top.player.onPlayPause();");            
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

/*            if (e.Control && e.Alt && e.KeyData == Keys.A)
            {
                MessageBox.Show("ctrl+alt+a");
            }
*/            
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            webBrowser1.Navigate("about:blank");
            //webBrowser1.Stop();
        }
    }
}


