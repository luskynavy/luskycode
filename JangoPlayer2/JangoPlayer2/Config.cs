namespace JangoPlayer2
{
    //Data read from config file
    public class Config
    {
        public string HomeCommand { get; set; }
        public string PauseCommand { get; set; }
        public string NextCommand { get; set; }
        //public string VolumeCommand { get; set; }
        public string PauseKey { get; set; }
        public string PauseKeyAlt { get; set; }
        public string NextKey { get; set; }
        public string NextKeyAlt { get; set; }
        //public string VolumeDownKey { get; set; }
        //public string VolumeUpKey { get; set; }
        //public string DumpKey { get; set; }
        //public string DumpKeyAlt { get; set; }
        //public string DumpPath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //public string Socks { get; set; }
    }
}
