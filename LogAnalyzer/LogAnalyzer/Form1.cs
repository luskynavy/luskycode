using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

/*
 How to detect platform and player name

PS3
---
sceNpManagerGetAccountRegion
[GMLogin] eden121 signed in

PC
--
<UPnP> GetPortMapping
<edLive> TRACE : edLiveGroupHandler::AddUsersFromSave() _local_user: 1102776, "UnLuskyDePlus", 1102776 )
<edLive> TRACE : edLiveGroupHandler::AddFriendsFromLocalProfile( 0 ) _local_user: 1102776, "UnLuskyDePlus", 1102776 )

360
---
XNADDR
[GMLogin] edenFabEuro signed in

*/ 

namespace LogAnalyzer
{
    public partial class Form1 : Form
    {
        //string searchDir = @"C:\Users\YVANKA~1\AppData\Local\Temp\DWRCC Downloads\LEMON 2010-08-05 2010-08-06";
        string searchDir = Directory.GetCurrentDirectory();
        string filesFilter = "*.txt";

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = searchDir;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            int sizeToRead = 65535;

            //results.BeginUpdate();

            results.Clear();            

            results.Columns.Add("Name"/*, -1/*, -2, HorizontalAlignment.Left */);
            results.Columns.Add("Size"/*, -2, HorizontalAlignment.Right/*, -2, HorizontalAlignment.Right*/);
            results.Columns.Add("Infos"/*, -1/*, -2, HorizontalAlignment.Left*/);

            //string[] files = System.IO.Directory.GetFiles(searchDir, filesFilter);
            DirectoryInfo dir = new DirectoryInfo(searchDir);
            FileInfo[] files = dir.GetFiles(filesFilter);
            foreach (FileInfo f in files)
            {
                byte[] b = new byte[sizeToRead];
                UTF8Encoding temp = new UTF8Encoding(true);

                FileStream fs = File.OpenRead(f.FullName);                
                fs.Read(b, 0, b.Length);
                fs.Close();

                string text = temp.GetString(b);

                string type = "Unknown";
                bool is360 = (text.IndexOf("XNADDR") != -1);
                if (is360)
                {
                    type = "360";
                }

                bool isPC = false;
                if (!is360)
                {
                    isPC = (text.IndexOf("<UPnP> GetPortMapping") != -1);
                    if (isPC)
                    {
                        type = "PC";
                    }                    
                }

                bool isPS3 = false;
                if (!is360 && !isPC)
                {
                    isPS3 = (text.IndexOf("sceNpManagerGetAccountRegion") != -1);
                    if (!isPS3)
                    {
                        isPS3 = (text.IndexOf("CELL_SYSUTIL_") != -1);                        
                    }
                    if (isPS3)
                    {
                        type = "PS3";
                    }
                }                

                 ListViewItem item1 = new ListViewItem(f.Name);               
                 item1.SubItems.Add("" + f.Length);
                 item1.SubItems.Add(type);
                 results.Items.Add(item1);
            }

            results.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            //results.EndUpdate();
        }

        private void results_DoubleClick(object sender, EventArgs e)
        {
            string name = results.SelectedItems[0].Text;
            System.Diagnostics.Process.Start("explorer.exe", searchDir + "\\" + name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderFileDialog1 = new FolderBrowserDialog();
            folderFileDialog1.SelectedPath = searchDir;

            if (folderFileDialog1.ShowDialog() == DialogResult.OK)
            {
                searchDir = folderFileDialog1.SelectedPath;
                textBox1.Text = searchDir;
                //openFileDialog1.FileName;
            }

        }
    }
}
