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
        public int volume = 50;
        public int lastVolumeChange = 0;
        public bool noTitle = false;
        public Hooks hook;


        public Form1()
        {
            bool scriptErrorsOption = false;
            bool clearCacheOption = true;

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (System.String.Compare(arg, "-noerrors", true) == 0)
                    scriptErrorsOption = true;

                if (System.String.Compare(arg, "-noclearcache", true) == 0)
                    clearCacheOption = false;

                if (System.String.Compare(arg, "-notitle", true) == 0)
                    noTitle = true;
            }

            if (clearCacheOption)
                ClearCache();

            InitializeComponent();

            if (scriptErrorsOption)
                webBrowser1.ScriptErrorsSuppressed = true;

            hook = new Hooks();

            //hook.KeyPress += new KeyPressEventHandler(hook_KeyDown);
            hook.KeyDown += new KeyEventHandler(hook_KeyDown);
            //hook.KeyUp += new KeyEventHandler(hook_KeyDown);

            //webBrowser1.Refresh(System.Windows.Forms.WebBrowserRefreshOption.Normal);
        }

        void hook_KeyDown(object sender, KeyEventArgs e)
        //void hook_KeyDown(object sender, KeyPressEventArgs e)
        {
            //int key = e.GetHashCode();
            Keys key = e.KeyData;
            int now = Environment.TickCount;
                

            if (key == Keys.MediaPlayPause/*.GetHashCode()*/)
            {
                //_jp.ctrls.onPlayPause(); ; return false;
                //webBrowser1.Navigate("javascript:top.player.onPlayPause();");
                webBrowser1.Navigate("javascript:_jp.ctrls.onPlayPause();");
            }

            if (key == Keys.MediaNextTrack/*.GetHashCode()*/)
            {
                //_jp.ctrls.onSkip(); ; return false;
                //webBrowser1.Navigate("javascript:top.player.onSkip(true);");
                webBrowser1.Navigate("javascript:_jp.ctrls.onSkip();");
                //webBrowser1.Navigate("javascript:top.player.setTimeout('top.player.onSkip(true)',0);");
            }
            
            if (key == Keys.VolumeDown/*.GetHashCode()*/ || key == Keys.MediaStop/*.GetHashCode()*/)
            {
                if (now - lastVolumeChange > 100)
                {
                    volume -= 5;
                    if (volume < 0)
                    {
                        volume = 0;
                    }
                    lastVolumeChange = Environment.TickCount;
                    //webBrowser1.Navigate("javascript:top.player.onVolume(" + volume + ");");
                    webBrowser1.Navigate("javascript:_jp.ctrls.onVolume(" + volume + ");");
                }
            }

            if (key == Keys.VolumeUp/*.GetHashCode()*/ || key == Keys.MediaPreviousTrack/*.GetHashCode()*/)
            {
                if (now - lastVolumeChange > 100)
                {
                    volume += 5;
                    if (volume > 100)
                    {
                        volume = 100;
                    }
                    lastVolumeChange = Environment.TickCount;
                    webBrowser1.Navigate("javascript:top.player.onVolume(" + volume + ");");
                }
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

        private void update_Tick(object sender, EventArgs e)
        {
            //Update the title
            if (noTitle == false)
            {
                try
                {
                    Text = webBrowser1.Document.Title;
                }
                catch
                {
                }
            }
        }

        private void ReHook_Click(object sender, EventArgs e)
        {
            //remove the old hook
            hook.KeyDown -= new KeyEventHandler(hook_KeyDown);

            //Reinstall it
            hook = new Hooks();
            
            hook.KeyDown += new KeyEventHandler(hook_KeyDown);
        }
    }
}
