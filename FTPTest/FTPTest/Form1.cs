using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;

namespace FTPTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listDir_Click(object sender, EventArgs e)
        {
            // Get the object used to communicate with the server.            
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.free.fr/tmp/");
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.free.fr/pub/support/DongleUSB80211n/");
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //-rw-r--r--    1 ftp      ftp       1339122 Jul 10  2008 Guide d'installation rapide.pdf\r\n
            //drwxr-xr-x    2 ftp      ftp          4096 Sep 19  2008 Linux\r\n
            //drwxr-xr-x    2 ftp      ftp          4096 Sep 19  2008 Mac\r\n
            //-rw-r--r--    1 ftp      ftp      30379728 Jul 10  2008 setup_windows.exe\r\n

            //request.Method = WebRequestMethods.Ftp.ListDirectory;         //Guide d'installation rapide.pdf\r\nLinux\r\nMac\r\nsetup_windows.exe\r\n

            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.free.fr/pub/support/DongleUSB80211n/setup_windows.exe");
            //request.Method = WebRequestMethods.Ftp.GetFileSize;		    //result in response.ContentLength	30379728	long
            //request.Method = WebRequestMethods.Ftp.GetDateTimestamp;      //result in response.LastModified	{10/07/2008 17:52:11}	System.DateTime

            // This example assumes the FTP site uses anonymous logon.            
            request.Credentials = new NetworkCredential("anonymous", "anonymous@free.fr");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            //textBox1.Text += reader.ReadToEnd();
            textBox1.Text = "";
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                textBox1.Text += line + "\r\n"; 
            }

            textBox1.Text += "Directory List Complete, status " + response.StatusDescription;            

            reader.Close();
            response.Close();
        }

        private void upload_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("ftp://ykalafatov.free.fr/test.txt");
            string text = "Time = 12:00am temperature = 60";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(text);

            client.Credentials = new NetworkCredential("ykalafatov", "*");
            client.UploadDataCompleted += UploadDataCallback;
            client.UploadFileCompleted += UploadFileCallback;
            client.UploadProgressChanged += UploadProgressChanged;
            //client.UploadDataAsync(uri, data); //time of file : upload date of server (GMT -1 ?)
            client.UploadFileAsync(new Uri("ftp://ykalafatov.free.fr/testFile.txt"), "testFile.txt"); //time of file : upload date of server (GMT -1 ?)
        }

        private void UploadProgressChanged(Object sender, UploadProgressChangedEventArgs e)
        {
            textBox1.Text += e.ProgressPercentage + "%\r\n";
        }

        private void UploadDataCallback(Object sender, UploadDataCompletedEventArgs e)
        {
            byte[] data = (byte[])e.Result;
            string reply = System.Text.Encoding.UTF8.GetString(data);            
            textBox1.Text += reply;            
        }

        private void UploadFileCallback(Object sender, UploadFileCompletedEventArgs e)
        {
            byte[] data = (byte[])e.Result;
            string reply = System.Text.Encoding.UTF8.GetString(data);
            textBox1.Text += reply;
        }       

    }
}
