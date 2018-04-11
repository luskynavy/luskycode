using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

//from https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/creating-an-explorer-style-interface-with-the-listview-and-treeview

namespace WinFileTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PopulateTreeView();
        }

        private void PopulateTreeView()
        {
            TreeNode rootNode;

            //DirectoryInfo info = new DirectoryInfo(@"../..");
            DirectoryInfo info = new DirectoryInfo(@"c:\");
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                treeView1.Nodes.Add(rootNode);
                rootNode.Expand();
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                try
                {
                    subSubDirs = subDir.GetDirectories();

                    aNode = new TreeNode(subDir.Name, 0, 0);
                    aNode.Tag = subDir;
                    aNode.ImageKey = "folder";
                    
                    if (subSubDirs.Length != 0)
                    {
                        //GetDirectories(subSubDirs, aNode);
                        TreeNode emptyNode;
                        emptyNode = new TreeNode(subDir.Name, 0, 0);
                        emptyNode.Tag = subDir;
                        emptyNode.ImageKey = "folder";
                        aNode.Nodes.Add(emptyNode);
                    }
                    nodeToAddTo.Nodes.Add(aNode);
                }
                catch
                {
                }
            }
        }

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;       
            listView1.Items.Clear();
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            newSelected.Nodes.Clear();


            //DirectoryInfo[] nodeDirs = nodeDirInfo.GetDirectories();
            foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            //foreach (DirectoryInfo dir in nodeDirs)
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"), 
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);

                TreeNode aNode;
                aNode = new TreeNode(dir.Name, 0, 0);
                aNode.Tag = dir;
                aNode.ImageKey = "folder";
                newSelected.Nodes.Add(aNode);
                
                try
                {
                    DirectoryInfo[] subDirs;
                    subDirs = dir.GetDirectories();
                    if (subDirs.Length != 0)
                    {
                        //GetDirectories(subSubDirs, aNode);
                        TreeNode emptyNode;
                        emptyNode = new TreeNode(dir.Name, 0, 0);
                        emptyNode.Tag = dir;
                        emptyNode.ImageKey = "folder";
                        aNode.Nodes.Add(emptyNode);
                    }
                }
                catch
                {
                }
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                  { new ListViewItem.ListViewSubItem(item, "File"), 
                   new ListViewItem.ListViewSubItem(item, 
				file.LastAccessTime.ToShortDateString())};

                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}
