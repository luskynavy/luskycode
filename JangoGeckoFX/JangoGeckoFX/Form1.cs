using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Skybound.Gecko;

// Need add references to C:\Users\Tigra\Downloads\Skybound.GeckoFX.bin.v1.9.1.0\bin\Skybound.Gecko.dll
// from http://code.google.com/p/geckofx/downloads/list Skybound.GeckoFX.bin.v1.9.1.0.zip

// works with ftp://ftp.mozilla.org/pub/xulrunner/releases/1.9.0.13/runtimes/xulrunner-1.9.0.13.en-US.win32.zip
// FAIL 6.0, 5.0, 2.0

namespace JangoGeckoFX
{
    public partial class Form1 : Form
    {
        public int volume = 50;
        public int lastVolumeChange = 0;
        public bool noTitle = false;
        public Hooks hook;
        GeckoWebBrowser webBrowser1;

        public Form1()
        {
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-1.9.0.13.en-US.win32\xulrunner"); //OK
            Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-1.9.2.19.en-US.win32\xulrunner"); //OK
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-6.0.en-US.win32\xulrunner"); // FAIL: Le cast spécifié n'est pas valide.
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-5.0.en-US.win32\xulrunner"); // FAIL: Le cast spécifié n'est pas valide.
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-2.0.en-US.win32\xulrunner"); // FAIL: Le cast spécifié n'est pas valide.
            //Skybound.Gecko.Xpcom.Initialize(@"C:\NVN\FirefoxPortable 4\App\Firefox"); // FAIL: Le cast spécifié n'est pas valide.
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Program Files\Mozilla Firefox"); // FAIL: Le cast spécifié n'est pas valide.

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (System.String.Compare(arg, "-notitle", true) == 0)
                    noTitle = true;
            }

            InitializeComponent();
            webBrowser1 = new GeckoWebBrowser();
            webBrowser1.Parent = this;
            webBrowser1.Dock = DockStyle.Fill;

            hook = new Hooks();

            //hook.KeyPress += new KeyPressEventHandler(hook_KeyDown);
            hook.KeyDown += new KeyEventHandler(hook_KeyDown);
            //hook.KeyUp += new KeyEventHandler(hook_KeyDown);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("www.jango.com");
        }

        //Manage hook keys
        void hook_KeyDown(object sender, KeyEventArgs e)
        {
            int now = Environment.TickCount;

            if (e.KeyData == Keys.MediaPlayPause || (e.Control && e.Alt && e.KeyCode == Keys.P)/*.GetHashCode()*/)
            {
                //_jp.ctrls.onPlayPause(); ; return false;
                //webBrowser1.Navigate("javascript:top.player.onPlayPause();");
                webBrowser1.Navigate("javascript:_jp.ctrls.onPlayPause();");
            }

            if (e.KeyData == Keys.MediaNextTrack || (e.Control && e.Alt && e.KeyCode == Keys.N))
            {
                //_jp.ctrls.onSkip(); ; return false;
                //webBrowser1.Navigate("javascript:top.player.onSkip(true);");
                webBrowser1.Navigate("javascript:_jp.ctrls.onSkip();");
                //webBrowser1.Navigate("javascript:top.player.setTimeout('top.player.onSkip(true)',0);");
            }

            if (e.KeyData == Keys.VolumeDown || e.KeyData == Keys.MediaStop)
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

            if (e.KeyData == Keys.VolumeUp || e.KeyData == Keys.MediaPreviousTrack)
            {
                if (now - lastVolumeChange > 100)
                {
                    volume += 5;
                    if (volume > 100)
                    {
                        volume = 100;
                    }
                    lastVolumeChange = Environment.TickCount;
                    //webBrowser1.Navigate("javascript:top.player.onVolume(" + volume + ");");
                    webBrowser1.Navigate("javascript:_jp.ctrls.onVolume(" + volume + ");");
                }
            }

            /*            if (e.Control && e.Alt && e.KeyData == Keys.A)
                        {
                            MessageBox.Show("ctrl+alt+a");
                        }
            */
        }

        private void update_Tick(object sender, EventArgs e)
        {
            //Update the title
            if (noTitle == false)
            {
                try
                {
                    /*if (webBrowser1.Document.Title.Length > 0)
                    {
                        //byte[] unicodeBytes = Encoding.Unicode.GetBytes(webBrowser1.Document.Title);
                        byte[] unicodeBytes1;
                        if (webBrowser1.Document.Title[webBrowser1.Document.Title.Length - 1] != '\0')
                        {
                            unicodeBytes1 = Encoding.ASCII.GetBytes(webBrowser1.Document.Title + '\0');
                        }
                        else
                        {
                            unicodeBytes1 = Encoding.ASCII.GetBytes(webBrowser1.Document.Title);
                        }
                        //Text = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);
                        Text = Encoding.Unicode.GetString(unicodeBytes1);
                    }*/
                    Text = webBrowser1.DocumentTitle;
                }
                catch
                {
                }
            }
        }

        private void ReHook_Click(object sender, EventArgs e)
        {
            //Remove the old hook
            hook.KeyDown -= new KeyEventHandler(hook_KeyDown);

            //Reinstall it
            //hook = new Hooks();
            hook.Stop();
            hook.Start();

            hook.KeyDown += new KeyEventHandler(hook_KeyDown);
        }
    }
}
