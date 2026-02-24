
using System.Timers;

namespace WinFormsTimers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            System.Threading.Timer timer2 = new System.Threading.Timer(timerCallback);
            timer2.Change(0, 1000);
        }


        // This is called on a background thread, so we need to marshal to the UI thread.
        private void timerCallback(object? state)
        {
            if (label4.IsHandleCreated)
            {
                if (label4.InvokeRequired)
                {
                    label4.Invoke(new Action(() => label4.Text = "System.Threading " + DateTime.Now.ToString("T")));
                }
                else
                {
                    label4.Text = "System.Threading " + DateTime.Now.ToString("T");
                }
            }
        }

        // This is called on a background thread, so we need to marshal to the UI thread.
        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            // This is called on a background thread, so we need to marshal to the UI thread.
            if (label3.InvokeRequired)
            {
                label3.Invoke(new Action(() => label3.Text = "System.Timers " + DateTime.Now.ToString("T")));
            }
            else
            {
                label3.Text = "System.Timers " + DateTime.Now.ToString("T");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "button 1 pushed";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "button 2 pushed";
        }

        // This is called on the UI thread, so we can update the label directly.
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = "Winforms Timer " + DateTime.Now.ToString("T");
        }
    }
}
