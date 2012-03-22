using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace HookTest
{
    public partial class Form1 : Form
    {
        private bool suspended = false;

        //private string BrowserExecutable = "C:\\Program Files (x86)\\Avant Browser\\avant.exe";
        //private string BrowserExecutable = "C:\\Program Files\\Avant Browser\\avant.exe";
        private string BrowserExecutable = "C:\\Program Files\\Orca Browser\\orca.exe";

        //private string BrowserName = "Avant";
        private string BrowserName = "Orca";

        public Form1()
        {
            //ClearCache();

            //"http://www.jango.com/profiles/1972516?c=1&l=0";
            //System.Threading.Thread.Suspend();

            //Help on SuspendThread/ResumeThread
            //http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/4368bf95-3fed-4c78-a821-2d26c162bcf9

            InitializeComponent();

            //Process JangoIE = Process.Start("IExplore.exe", "http://www.jango.com/profiles/1972516?c=1&l=0");
            //Process JangoIE = Process.Start("chrome.exe", "http://www.jango.com/profiles/1972516?c=1&l=0");
            //Process JangoIE = Process.Start("C:\\Program Files (x86)\\Crazy Browser\\Crazy Browser.exe", "http://www.jango.com/profiles/1972516?c=1&l=0");
            //Process JangoIE = Process.Start("C:\\Program Files (x86)\\Avant Browser\\avant.exe", "http://www.jango.com/profiles/1972516?c=1&l=0");
            
            //Process JangoIE = Process.Start("IExplore.exe");

            
            Process[] Browser = Process.GetProcessesByName(BrowserName);
            //if not launched
            if (Browser.GetLength(0) == 0)
            {
                StartBrowser();
            }
        
            Hooks hook = new Hooks();

            hook.KeyDown += new KeyEventHandler(hook_KeyDown);

            //webBrowser1.Refresh(System.Windows.Forms.WebBrowserRefreshOption.Normal);            
        }

        private void Pause()
        {
            Process[] Browser = Process.GetProcessesByName(BrowserName);
            if (Browser.GetLength(0) == 0)
            {
                StartBrowser();
                suspended = false;
            }

            Process JangoBrowser = Browser[0];
            //Process JangoBrowser = Process.GetProcessById(632);

            int ret = 0;

            if (JangoBrowser.ProcessName != "")
            {
                foreach (ProcessThread pT in JangoBrowser.Threads)
                {
                    IntPtr ptrOpenThread = Win32.OpenThread(Win32.ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                    if (ptrOpenThread != null)
                    {
                        if (suspended)
                        {
                            ret = Win32.ResumeThread(ptrOpenThread);
                        }
                        else
                        {
                            ret = Win32.SuspendThread(ptrOpenThread);
                        }
                    }
                }

                suspended = !suspended;
            }
        }

        void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.MediaPlayPause)
            {
                Pause();
                //webBrowser1.Navigate("javascript:top.player.onPlayPause();");            
                //MessageBox.Show("pause");
                //throw new NotImplementedException();
            }

            if (e.KeyData == Keys.MediaNextTrack)
            {
                CloseBrowser();

                StartBrowser();

                //webBrowser1.Navigate("javascript:top.player.onSkip(true);");
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

        private Process StartBrowser()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(BrowserExecutable);
            //startInfo.WindowStyle = ProcessWindowStyle.Minimized;

            return Process.Start(startInfo);             
        }

        private void CloseBrowser()
        {
            Process[] Browser = Process.GetProcessesByName(BrowserName);
            if (Browser.GetLength(0) == 0)
            {
                return;
            }

            Process JangoBrowser = Browser[0];

            if (suspended)
            {
                Pause();
            }
            JangoBrowser.CloseMainWindow();
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseBrowser();
            //webBrowser1.Navigate("about:blank");
            //webBrowser1.Stop();
        }
    }
}


