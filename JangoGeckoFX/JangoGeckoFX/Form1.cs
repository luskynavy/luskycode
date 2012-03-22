using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Skybound.Gecko;
using System.Xml.Serialization;
using System.IO;

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
        public Config config;

        public Form1()
        {
            config = new Config();
            config.XulrunnerPath = @"xulrunner";
            config.HomeCommand = "www.jango.com";
            //_jp.ctrls.onPlayPause(); ; return false;   
            //"javascript:top.player.onPlayPause();"
            config.PauseCommand =  "javascript:_jp.ctrls.onPlayPause();";
            //_jp.ctrls.onSkip(); ; return false;
            //"javascript:top.player.onSkip(true);"
            //"javascript:top.player.setTimeout('top.player.onSkip(true)',0);"
            config.NextCommand = "javascript:_jp.ctrls.onSkip();";
            //"javascript:top.player.onVolume("{0}");
            //"javascript:_jp.ctrls.onVolume("{0}");
            config.VolumeCommand = "javascript:_jp.ctrls.onVolume({0});";

            string configFilePath = @"config.xml";

            //Get config file path from command line if not using default
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                configFilePath = Environment.GetCommandLineArgs()[1];
            }

            Config configRead = DeserializeFromXML(configFilePath);
            if (configRead != null)
            {
                config = configRead;
            }

            Xpcom.Initialize(config.XulrunnerPath);
            
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-1.9.0.13.en-US.win32\xulrunner"); //OK with Skybound.GeckoFX.bin.v1.9.1.0
            //Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-1.9.2.19.en-US.win32\xulrunner"); //OK with Skybound.GeckoFX.bin.v1.9.1.0
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-6.0.en-US.win32\xulrunner"); // OK with GeckoFx-Windows-6.0-0.3
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-7.0b1.en-US.win32\xulrunner"); // OK with GeckoFx-Windows-7.0-0.1
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-5.0.en-US.win32\xulrunner"); // FAIL: Le cast spécifié n'est pas valide.
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Users\Tigra\Downloads\xulrunner-2.0.en-US.win32\xulrunner"); // FAIL: Le cast spécifié n'est pas valide.
            //Skybound.Gecko.Xpcom.Initialize(@"C:\NVN\FirefoxPortable 4\App\Firefox"); // FAIL: Le cast spécifié n'est pas valide.
            //Skybound.Gecko.Xpcom.Initialize(@"C:\Program Files\Mozilla Firefox"); // FAIL: Le cast spécifié n'est pas valide.

            GeckoPreferences.User["browser.cache.memory.enable"] = false;

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
            webBrowser1.Navigate(config.HomeCommand);
        }

        //Manage hook keys
        void hook_KeyDown(object sender, KeyEventArgs e)
        {
            int now = Environment.TickCount;

            //Pause/unpause
            if (e.KeyData == Keys.MediaPlayPause || (e.Control && e.Alt && e.KeyCode == Keys.P))
            {                             
                webBrowser1.Navigate(config.PauseCommand);
            }

            //Next track
            if (e.KeyData == Keys.MediaNextTrack || (e.Control && e.Alt && e.KeyCode == Keys.N))
            {                                
                webBrowser1.Navigate(config.NextCommand);                
            }

            //Volume down
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
                    webBrowser1.Navigate(String.Format(config.VolumeCommand, volume));
                }
            }

            //Volume up
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
                    webBrowser1.Navigate(String.Format(config.VolumeCommand, volume));
                }
            }

            /* if (e.Control && e.Alt && e.KeyData == Keys.A)
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

        //Reset page, sometimes usefull if a command fails (multiples commands at the same time ?)
        private void Home_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(config.HomeCommand);
        }

        //Read the data from xml
        static Config DeserializeFromXML(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Config));
            Config config = null;
            try
            {
                TextReader textReader = new StreamReader(path);
                config = (Config)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            catch
            {
            }

            return config;
        }
    }

    //Data read from config file
    public class Config
    {
        public string XulrunnerPath { get; set; }
        public string HomeCommand   { get; set; }        
        public string PauseCommand  { get; set; }
        public string NextCommand   { get; set; }
        public string VolumeCommand { get; set; }
    }
}
