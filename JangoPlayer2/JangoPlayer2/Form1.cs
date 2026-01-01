using System.Windows.Forms;

namespace JangoPlayer2
{
    public partial class Form1 : Form
    {
        public Hooks hook;

        public Form1()
        {
            InitializeComponent();


            hook = new Hooks();

            hook.KeyDown += new KeyEventHandler(hook_KeyDown);
        }

        //Manage hook keys
        void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //Pause/unpause
            if (e.KeyData == /*pauseKey*/ Keys.MediaPlayPause || (e.Control && e.Alt && e.KeyCode == /*pauseKeyAlt*/ Keys.P))
            {
                buttonPlay_Click(null, null);
            }

            //Next track
            if (e.KeyData == /*nextKey*/ Keys.MediaNextTrack || (e.Control && e.Alt && e.KeyCode == /*nextKeyAlt*/ Keys.N))
            {
                buttonNext_Click(null, null);
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            webView21.ExecuteScriptAsync("document.querySelector('[id=\"player-play-button\"]').click();");
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            webView21.ExecuteScriptAsync("document.querySelector('.fa-step-forwardn').parentNode.click();");
            //webView21.ExecuteScriptAsync("document.querySelector('[data-icon=\"step-forward\"]').parentNode.click();");
        }
    }
}
