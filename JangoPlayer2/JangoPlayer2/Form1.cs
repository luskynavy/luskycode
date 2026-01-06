using System.Windows.Forms;
using System.Xml.Serialization;

namespace JangoPlayer2
{
    public partial class Form1 : Form
    {
        public Hooks hook;
        public Config config;
        Keys pauseKey;
        Keys pauseKeyAlt;
        Keys nextKey;
        Keys nextKeyAlt;

        public Form1()
        {
            config = new Config();

            string configFilePath = @"config.xml";

            //Get config file path from command line if not using default
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                if (System.String.Compare(Environment.GetCommandLineArgs()[1], "-notitle", true) != 0)
                {
                    configFilePath = Environment.GetCommandLineArgs()[1];
                }
            }
            else if (Environment.GetCommandLineArgs().Length == 3)
            {
                if (System.String.Compare(Environment.GetCommandLineArgs()[1], "-notitle", true) == 0)
                {
                    configFilePath = Environment.GetCommandLineArgs()[2];
                }
                else
                {
                    configFilePath = Environment.GetCommandLineArgs()[1];
                }
            }

            //if -notitle option found, title update is disabled
            /*foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (System.String.Compare(arg, "-notitle", true) == 0)
                    noTitle = true;
            }*/

            Config configRead = DeserializeFromXML(configFilePath);
            if (configRead != null)
            {
                config = configRead;
            }

            if (config.HomeCommand == null)
                config.HomeCommand = "www.jango.com";
            if (config.PauseCommand == null)
                config.PauseCommand = "document.querySelector('[id=\"player-play-button\"]').click();";
            if (config.NextCommand == null)
                config.NextCommand = "document.querySelector('[data-icon=\"step-forward\"]').parentNode.click();";
            //"javascript:top.player.onVolume("{0}");
            //"javascript:_jp.ctrls.onVolume("{0}");
            /*if (config.VolumeCommand == null)
                config.VolumeCommand = "javascript:_jp.ctrls.onVolume({0});";*/
            if (config.PauseKey == null)
                config.PauseKey = "MediaPlayPause";
            if (config.PauseKeyAlt == null)
                config.PauseKeyAlt = "P";
            if (config.NextKey == null)
                config.NextKey = "MediaNextTrack";
            if (config.NextKeyAlt == null)
                config.NextKeyAlt = "N";
            /*if (config.VolumeDownKey == null)
                config.VolumeDownKey = "MediaStop";
            if (config.VolumeUpKey == null)
                config.VolumeUpKey = "MediaPreviousTrack";
            if (config.DumpKey == null)
                config.DumpKey = "LaunchMail";
            if (config.DumpKeyAlt == null)
                config.DumpKeyAlt = "D";
            if (config.DumpPath == null)
                config.DumpPath = "";*/


            //Get keys from key names
            pauseKey = (Keys)Enum.Parse(typeof(Keys), config.PauseKey);
            pauseKeyAlt = (Keys)Enum.Parse(typeof(Keys), config.PauseKeyAlt);
            nextKey = (Keys)Enum.Parse(typeof(Keys), config.NextKey);
            nextKeyAlt = (Keys)Enum.Parse(typeof(Keys), config.NextKeyAlt);
            /*volumeDownKey = (Keys)Enum.Parse(typeof(Keys), config.VolumeDownKey);
            volumeUpKey = (Keys)Enum.Parse(typeof(Keys), config.VolumeUpKey);
            dumpKey = (Keys)Enum.Parse(typeof(Keys), config.DumpKey);
            dumpKeyAlt = (Keys)Enum.Parse(typeof(Keys), config.DumpKeyAlt);*/

            InitializeComponent();

            //if width/height not specified in config file, use the default values
            if (config.Width == 0)
                config.Width = this.Width;
            if (config.Height == 0)
                config.Height = this.Height;

            this.Width = config.Width;
            this.Height = config.Height;

            hook = new Hooks();

            hook.KeyDown += new KeyEventHandler(Hook_KeyDown);

            webView21.Source = new System.Uri(config.HomeCommand);
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

        //Manage hook keys
        void Hook_KeyDown(object? sender, KeyEventArgs e)
        {
            //Pause/unpause
            if (e.KeyData == pauseKey /*Keys.MediaPlayPause*/ || (e.Control && e.Alt && e.KeyCode == pauseKeyAlt /*Keys.P*/))
            {
                ButtonPlay_Click("", new EventArgs());
            }

            //Next track
            if (e.KeyData == nextKey /*Keys.MediaNextTrack*/ || (e.Control && e.Alt && e.KeyCode == nextKeyAlt /*Keys.N*/))
            {
                ButtonNext_Click("", new EventArgs());
            }
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            webView21.ExecuteScriptAsync(config.PauseCommand);
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            //webView21.ExecuteScriptAsync("document.querySelector('.fa-step-forwardn').parentNode.click();");
            webView21.ExecuteScriptAsync(config.NextCommand);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Update the window title with the page title
            try
            {
                Text = webView21.CoreWebView2.DocumentTitle;
            }
            catch
            {
            }
        }
    }
}
