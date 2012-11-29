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
        public bool mp3Present = false; //shown in title if mp3 is already dumped
        public bool m4aPresent = false; //shown in title if m4a is already dumped
        public string lastTitle;
        public Hooks hook;
        GeckoWebBrowser webBrowser1;
        public Config config;
        Keys pauseKey;
        Keys pauseKeyAlt;
        Keys nextKey;
        Keys nextKeyAlt;
        Keys volumeDownKey;
        Keys volumeUpKey;
        Keys dumpKey;
        Keys dumpKeyAlt;


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
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (System.String.Compare(arg, "-notitle", true) == 0)
                    noTitle = true;
            }

            Config configRead = DeserializeFromXML(configFilePath);
            if (configRead != null)
            {
                config = configRead;
            }

            if (config.XulrunnerPath == null)
                config.XulrunnerPath = @"xulrunner";
            if (config.HomeCommand == null)
                config.HomeCommand = "www.jango.com";
            //_jp.ctrls.onPlayPause(); ; return false;
            //"javascript:top.player.onPlayPause();"
            if (config.PauseCommand == null)
                config.PauseCommand = "javascript:_jp.ctrls.onPlayPause();";
            //_jp.ctrls.onSkip(); ; return false;
            //"javascript:top.player.onSkip(true);"
            //"javascript:top.player.setTimeout('top.player.onSkip(true)',0);"
            if (config.NextCommand == null)
                config.NextCommand = "javascript:_jp.ctrls.onSkip();";
            //"javascript:top.player.onVolume("{0}");
            //"javascript:_jp.ctrls.onVolume("{0}");
            if (config.VolumeCommand == null)
                config.VolumeCommand = "javascript:_jp.ctrls.onVolume({0});";
            if (config.PauseKey == null)
                config.PauseKey = "MediaPlayPause";
            if (config.PauseKeyAlt == null)
                config.PauseKeyAlt = "P";
            if (config.NextKey == null)
                config.NextKey = "MediaNextTrack";
            if (config.NextKeyAlt == null)
                config.NextKeyAlt = "N";
            if (config.VolumeDownKey == null)
                config.VolumeDownKey = "MediaStop";
            if (config.VolumeUpKey == null)
                config.VolumeUpKey = "MediaPreviousTrack";
            if (config.DumpKey == null)
                config.DumpKey = "LaunchMail";
            if (config.DumpKeyAlt == null)
                config.DumpKeyAlt = "D";
            if (config.DumpPath == null)
                config.DumpPath = "";


            //Get keys from key names
            pauseKey = (Keys)Enum.Parse(typeof(Keys), config.PauseKey);
            pauseKeyAlt = (Keys)Enum.Parse(typeof(Keys), config.PauseKeyAlt);
            nextKey = (Keys)Enum.Parse(typeof(Keys), config.NextKey);
            nextKeyAlt = (Keys)Enum.Parse(typeof(Keys), config.NextKeyAlt);
            volumeDownKey = (Keys)Enum.Parse(typeof(Keys), config.VolumeDownKey);
            volumeUpKey = (Keys)Enum.Parse(typeof(Keys), config.VolumeUpKey);
            dumpKey = (Keys)Enum.Parse(typeof(Keys), config.DumpKey);
            dumpKeyAlt = (Keys)Enum.Parse(typeof(Keys), config.DumpKeyAlt);

           
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

            //use socks param if found
            if (config.Socks != null)
            {
                GeckoPreferences.User["network.proxy.type"] = 1;

                //url and port are separated by a ':'
                string[] socks = config.Socks.Split(new Char[] { ':' });
                if (socks.Length == 2)
                {
                    GeckoPreferences.User["network.proxy.socks"] = socks[0];
                    GeckoPreferences.User["network.proxy.socks_port"] = Convert.ToInt32(socks[1]);
                }
            } 

            InitializeComponent();
            webBrowser1 = new GeckoWebBrowser();
            webBrowser1.Parent = this;
            webBrowser1.Dock = DockStyle.Fill;

            //if width/height not specified in config file, use the default values
            if (config.Width == 0)
                config.Width = this.Width;
            if (config.Height == 0)
                config.Height = this.Height;

            this.Width = config.Width;
            this.Height = config.Height;

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
            if (e.KeyData == pauseKey /*Keys.MediaPlayPause*/ || (e.Control && e.Alt && e.KeyCode == pauseKeyAlt /*Keys.P*/))
            {                             
                webBrowser1.Navigate(config.PauseCommand);
            }
            
            //Next track
            if (e.KeyData == nextKey /*Keys.MediaNextTrack*/ || (e.Control && e.Alt && e.KeyCode == nextKeyAlt /*Keys.N*/))
            {                                
                webBrowser1.Navigate(config.NextCommand);                
            }

            //Volume down
            if (e.KeyData == Keys.VolumeDown || e.KeyData == volumeDownKey /*Keys.MediaStop*/)
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
            if (e.KeyData == Keys.VolumeUp || e.KeyData == volumeUpKey /*Keys.MediaPreviousTrack*/)
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

            //Dump the current song
            if (e.KeyData == dumpKey /*Keys.LaunchMail*/  || (e.Control && e.Alt && e.KeyCode == dumpKeyAlt /*Keys.D*/))
            {
                DumpSong();
            }
        }

        private void update_Tick(object sender, EventArgs e)
        {
            //Update the title
            if (noTitle == false)
            {
                try
                {
                    //add MP3 or M4A to title if MP3 or M4A is present when title (song) change
                    if (lastTitle != webBrowser1.DocumentTitle)
                    {
                        lastTitle = webBrowser1.DocumentTitle;

                        string dst = GetSongFilename();

                        mp3Present = File.Exists(config.DumpPath + dst + ".mp3");
                        m4aPresent = File.Exists(config.DumpPath + dst + ".m4a");
                    }

                    Text = webBrowser1.DocumentTitle + (mp3Present ? " MP3": "")  + (m4aPresent ? " M4A" : "");
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
            try
            {
                hook.Stop();
                hook.Start();
            }
            catch (Win32Exception)
            {
                hook = new Hooks(); //some times hook become invalid, so it can't no be stopped, try this
            }

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

        //Dump the current song
        //Extract the name from title and get the last file from cache bigger than 1Mo
        private void Dump_Click(object sender, EventArgs e)
        {
            DumpSong();
        }

        //Get the song filename from the document title
        //string webBrowser1DocumentTitle = "artist: song - Jango";
        //build the file name: extract artist, song name and remove the " - Jango" at the end
        private string GetSongFilename()
        {
            int songStart = webBrowser1.DocumentTitle.IndexOf(":");
            string dst = webBrowser1.DocumentTitle.Substring(0, songStart)
                + " - "
                + webBrowser1.DocumentTitle.Substring(songStart + 2, webBrowser1.DocumentTitle.Length - 8 - (songStart + 2));

            //remove special chars
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                dst = dst.Replace(c, '_');
            }

            return dst;
        }

        //Dump the current song
        //Extract the name from title and get the last file from cache bigger than 1Mo
        private void DumpSong()
        {
            //clear the lastTitle to force title update (for MP3 and M4A)
            lastTitle = "";

            //Get cache directory files
            string pathSrc = Xpcom.ProfileDirectory + @"\Cache";
            DirectoryInfo dir = new DirectoryInfo(pathSrc);
            FileInfo[] files = null;
            try
            {
                files = dir.GetFiles();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Invalid directory: " + pathSrc);
            }

            //if directory exists and there are files
            if (files != null && files.Count() > 0)
            {
                DateTime lastOne = new DateTime();
                int fileIndexToCopy = -1;
                for (int i = 0; i < files.Count(); i++)
                {
                    //get a file bigger than 1Mo and skip the cache map and index
                    if ((files[i].Length > 1024 * 1024) &&
                        !files[i].Name.StartsWith("_CACHE_"))
                    {
                        //is it more recent than previous one ?
                        if (files[i].LastWriteTime.CompareTo(lastOne) > 0)
                        {
                            lastOne = files[i].LastWriteTime;
                            fileIndexToCopy = i;
                        }                            
                    }
                }

                //if found a file
                if (fileIndexToCopy != -1)
                {
                    //get the song file name
                    string dst = GetSongFilename();

                    //mp3 or m4a ?
                    if (IsM4a(pathSrc + "\\" + files[fileIndexToCopy].Name))
                    {
                        dst += ".m4a";
                    }
                    else
                    {
                        dst += ".mp3";
                    }

                    //copy the file but don't overwrite if exists
                    try
                    {
                        System.IO.File.Copy(pathSrc + "\\" + files[fileIndexToCopy].Name, config.DumpPath + dst, false);
                    }
                    catch (IOException)
                    {

                    }
                }
            }
        }

        //Test if the file is a M4A
        private bool IsM4a(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                int magic1 = reader.ReadInt32();
                int magic2 = reader.ReadInt32();
                int magic3 = reader.ReadInt32();

                //http://www.ftyps.com/
                if ((magic1 == 0x18000000) &&   //?
                    (magic2 == 0x70797466) &&   //"ftyp"
                    (magic3 == 0x2041344d))     //"M4A "
                {
                    return true;
                }
            }
            return false;
        }
    }

    //Data read from config file
    public class Config
    {
        public string   XulrunnerPath { get; set; }
        public string   HomeCommand { get; set; }
        public string   PauseCommand { get; set; }
        public string   NextCommand { get; set; }
        public string   VolumeCommand { get; set; }
        public string   PauseKey { get; set; }
        public string   PauseKeyAlt { get; set; }
        public string   NextKey { get; set; }
        public string   NextKeyAlt { get; set; }
        public string   VolumeDownKey { get; set; }
        public string   VolumeUpKey { get; set; }
        public string   DumpKey { get; set; }
        public string   DumpKeyAlt { get; set; }
        public string   DumpPath { get; set; }
        public int      Width { get; set; }
        public int      Height { get; set; }
        public string   Socks { get; set; }
    }
}
