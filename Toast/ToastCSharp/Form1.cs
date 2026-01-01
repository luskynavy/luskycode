using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;
using System.Security.Policy;
using Windows.Foundation.Collections;

namespace ToastCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Listen to notification activation
            ToastNotificationManagerCompat.OnActivated += OnToastActivated;
        }

        private void OnToastActivated(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

            if (args.Contains("conversationId"))
            {
                // Show the corresponding content
                //MessageBox.Show("Toast activated. Args: " + toastArgs.Argument);

                // Fail in .net core, ok for .Net ?
                // System.Diagnostics.Process.Start("https://google.com");

                // Fail too in .net core
                /*ProcessStartInfo psInfo = new ProcessStartInfo
                {
                    FileName = "https://www.google.com",
                    UseShellExecute = true
                };*/

                // .Net core version
                Process.Start("explorer", "https://www.google.com");
            }
        }


        private void ButtonSendToast_Click(object sender, EventArgs e)
        {
            // Create toast with one minute expiration
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText("ToastC# update available")
                .AddText("Version 2026 is now avaliable")
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMinutes(1);
                }); // Not seeing the Show() method? Make sure you have version 7.0,
                         // and if you're using .NET 6 (or later),
                         // then your TFM must be net6.0-windows10.0.17763.0 or greater
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clear toast list on leaving
            ToastNotificationManagerCompat.Uninstall();
        }
    }
}
