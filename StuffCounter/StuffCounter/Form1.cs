using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Xml;
using System.Web;
using System.Diagnostics;

namespace StuffCounter
{
    public partial class Form1 : Form
    {
        //5 seems to be the magic number when the armory is acting up.
        private const int RETRY_MAX = 5;
        string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
        private int sortColumn = -1;
        private bool guildMode = true;

        //id of t9 245
        int[] t9_245 = new int[] { 47984, 48483, 48225, 48378, 48257, 48134, 48287, 48577, 47778, 47754, 48165, 48164, 48346, 48538, 48641, 48223, 48450, 48484,
48610, 48226, 48379, 48078, 48542, 48227, 48454, 47753, 48482, 48608, 48224, 48377, 47782, 48163, 48317, 48576, 47983, 48350,
48637, 48258, 48288, 48541, 48638, 48446, 48256, 48286, 48539, 48640, 48452, 48430, 48080, 48578, 48166, 48481, 48607, 48376,
48316, 48318, 48609, 48210, 47780, 47755, 48135, 47985, 48349, 48319, 48212, 48167, 48081, 48079, 48485, 48611, 48380, 48347,
47781, 47757, 48208, 48320, 47987, 48211, 48133, 48077, 47779, 47756, 48136, 47986, 48259, 48137, 48289, 48579, 48209, 48255,
48285, 48575, 48348, 48540, 48639 };

        //id of t9 232
        int[] t9_232 = new int[] { 48605, 48220, 48373, 48073, 48535, 48221, 48448, 47752, 48480, 48603, 48222, 48375, 47783, 48162, 48312, 48574, 47982, 48345, 
47914, 48472, 48218, 48371, 48250, 48102, 48280, 48564, 47784, 47748, 48160, 48158, 48341, 48531, 48632, 48219, 48436, 48476, 
48636, 48252, 48282, 48533, 48635, 48445, 48254, 48284, 48537, 48633, 48449, 48429, 48075, 48568, 48159, 48474, 48602, 48372, 
48310, 48313, 48604, 48215, 47785, 47750, 48130, 47980, 48314, 48344, 48213, 48161, 48076, 48074, 48478, 48606, 48374, 48342, 
47787, 47751, 48217, 48315, 47981, 48214, 48132, 48072, 47786, 47749, 48129, 47936, 48253, 48131, 48283, 48572, 48216, 48251, 
48281, 48566, 48343, 48529, 48634 };

        //item class used by cache
        class item
        {
            public item(int _id, int _ilvl)
            {
                id = _id;
                ilvl = _ilvl;
            }

            public int id;
            public int ilvl;
        }

        Dictionary<int, item> itemCache;

        public Form1()
        {
            InitializeComponent();
            ReadItemCache();
            //GetStuff("Soufr");
        }

        //RAWR DOWNLOAD CODE BEGIN

        // This is used to prevent multiple attempts at network traffic when its not working and 
        // continuing to issue requests could cause serious problems for the user.
        // <param name="ex"></param>
        private void CheckExceptionForFatalError(Exception ex)
        {
            if (ex.Message.Contains("407") /*proxy auth required */
                || ex.Message.Contains("403") /*proxy info probably wrong, if we keep issuing requests, they will probably get locked out*/
                || ex.Message.Contains("timed out") /*either proxy required and firewall dropped the request, or armory is down*/
                //|| ex.Message.Contains("invalid content type") /*unexpected content type returned*/
                || ex.Message.Contains("The remote name could not be resolved") /* DNS problems*/
                )
            {
                _fatalError = ex;
            }
        }

        // Used to create a web client with all of the appropriote proxy/useragent/etc settings
        private WebClient CreateWebClient()
        {
            WebClient client = new WebClient() { Encoding = Encoding.UTF8 };
            client.Headers.Add("user-agent", UserAgent);
            return client;
        }

        public string DownloadText(string URI)
        {
            WebClient webClient = CreateWebClient();
            string value = null;
            int retry = 0;
            bool success = false;
            do
            {
                if (!LastWasFatalError)
                {
                    try
                    {
                        value = webClient.DownloadString(URI);
                        if (!String.IsNullOrEmpty(value))
                        {
                            success = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        CheckExceptionForFatalError(ex);
                    }
                }
                retry++;
            } while (retry <= RETRY_MAX && !success && !LastWasFatalError);
            return value;
        }

        private XmlDocument DownloadXml(string URI) { return DownloadXml(URI, false); }
        private XmlDocument DownloadXml(string URI, bool allowTable)
        {
            XmlDocument returnDocument = null;
            int retry = 0;
            //Download Text has retry logic in it as well, but that just makes sure it gets a response, this
            //makes sure we get a valid XML response.
            do
            {
                string xml = DownloadText(URI);
                //If it contains "<table", then the armory accidentally returned it as html instead of xml.
                if (!string.IsNullOrEmpty(xml) && (allowTable || !xml.Contains("<table")))
                {
                    try
                    {
                        returnDocument = new XmlDocument();
                        returnDocument.XmlResolver = null;
                        returnDocument.LoadXml(xml.Replace("&", ""));
                        if (returnDocument == null || returnDocument.DocumentElement == null
                                    || !returnDocument.DocumentElement.HasChildNodes)
                        {
                            //document returned no data we care about.
                            returnDocument = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                retry++;
            } while (returnDocument == null && !LastWasFatalError && retry < RETRY_MAX);

            return returnDocument;
        }

        //RAWR DOWNLOAD CODE END

        private static Exception _fatalError = null;
        
        // If the last request received a 407 or no response. Used to prevent a lot of bad calls.
        // It also has the good side effect of not locking someone's account out if they enter the proxy info incorrectly
        // by sending lots of bad authorization attempts.        
        public static bool LastWasFatalError
        {
            get { return _fatalError != null; }
        }

        private void ReadItemCache()
        {
            string xml = System.IO.File.ReadAllText("ItemCache.xml");
            int ilvl, id;
            XmlDocument returnDocument = new XmlDocument();
            returnDocument.XmlResolver = null;
            returnDocument.LoadXml(xml);
            itemCache = new Dictionary<int, item>();
            foreach (XmlNode node in returnDocument.SelectNodes("ArrayOfItem/Item"))
            {
                XmlNode nodeLevel = node.SelectSingleNode("ItemLevel");
                ilvl = int.Parse(nodeLevel.InnerText);
                XmlNode nodeId = node.SelectSingleNode("Id");
                id = int.Parse(nodeId.InnerText);
                
                itemCache.Add(id, new item(id,ilvl));
            }
        }

        //Get stuff of specified player
        private void GetStuff(string name)
        {
            //Get armory infos
            XmlDocument xml = DownloadXml("http://eu.wowarmory.com/character-sheet.xml?r=" + HttpUtility.UrlEncode(realm.Text) + "&n=" + HttpUtility.UrlEncode(name));
            if (xml != null)
            {
                ListViewItem item1;
                int id;
                int slot;
                //for each item found
                foreach (XmlNode node in xml.SelectNodes("page/characterInfo/characterTab/items/item"))
                {
                    id = int.Parse(node.Attributes["id"].Value);
                    slot = int.Parse(node.Attributes["slot"].Value);
                    //-1 = ammo, 3 = shirt, 18 = tabard
                    if (slot != -1 && slot != 3 && slot != 18 && itemCache.ContainsKey(id))
                    {
                        results.Text += "\t\t<Item id=\"" + id + "\" ilvl=\"" + itemCache[id].ilvl + "\" slot=\"" + slot + "\" />\r\n";
                        item1 = new ListViewItem("        <Item id=\"" + id + "\" ilvl=\"" + itemCache[id].ilvl + "\" slot=\"" + slot + "\" />", 0);
                        results.Items.Add(item1);
                    }
                }
            }
        }

        //Get the guild member list and infos
        private void go_Click(object sender, EventArgs e)
        {
            guildMode = true;
            results.Sorting = SortOrder.None;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ListViewItem item1;

            //XmlDocument xml = DownloadXml("http://eu.wowarmory.com/guild-info.xml?r=Eldre%27Thalas&gn=Ancestr%C3%A4l");
            XmlDocument xml = DownloadXml("http://eu.wowarmory.com/guild-info.xml?r=" + HttpUtility.UrlEncode(realm.Text) + "&gn=" + HttpUtility.UrlEncode(guild.Text));
            //results.Text = xml.InnerXml;
            if (xml != null)
            {
                results.Clear();
                results.Columns.Add("Guild", -2);
                item1 = new ListViewItem("<members>", 0);
                results.Items.Add(item1);
                results.Text = "<members>\r\n";
                string name;
                int level;
                int rank;
                foreach (XmlNode node in xml.SelectNodes("page/guildInfo/guild/members/character"))
                {
                    rank = int.Parse(node.Attributes["rank"].Value);
                    level = int.Parse(node.Attributes["level"].Value);
                    if (level == 80 && (rank == 0 || rank == 1 || rank == 3 || rank == 7))
                    {                        
                        name = node.Attributes["name"].Value;

                        item1 = new ListViewItem("    <character name=\"" + name + "\" rank=\"" + rank + "\">", 0);
                        results.Items.Add(item1);

                        results.Text += "\t<character name=\"" + name + "\" rank=\"" + rank + "\">\r\n";
                        GetStuff(name);
                        results.Text += "\t</character>\r\n";
                        item1 = new ListViewItem("    </character>", 0);
                        results.Items.Add(item1);
                        Application.DoEvents();
                    }
                }
                results.Text += "</members>\r\n";
                item1 = new ListViewItem("</members>", 0);
                results.Items.Add(item1);
            }
            else
            {
                results.Text += "Guild info not available\r\n";
                item1 = new ListViewItem("Guild info not available");
                results.Items.Add(item1);
            }

            stopwatch.Stop();

            System.IO.File.WriteAllText("guild.xml", results.Text);
            long elapsedTimeInMilliSeconds = stopwatch.ElapsedMilliseconds;


            results.Text += "Total time: " + elapsedTimeInMilliSeconds / 1000;
            item1 = new ListViewItem("Total time: " + elapsedTimeInMilliSeconds / 1000);
            results.Items.Add(item1);
        }
        
        //Analyze the guild.xml datas
        private void analyze_Click(object sender, EventArgs e)
        {
            guildMode = false;
            results.Sorting = SortOrder.Ascending;

            //get the datas into xml format
            string xml = System.IO.File.ReadAllText("guild.xml");
            XmlDocument returnDocument = new XmlDocument();
            //returnDocument.XmlResolver = null; //usefull ?
            returnDocument.LoadXml(xml);
            itemCache = new Dictionary<int, item>();
            String analyzeStr = "";

            results.Clear();

            results.Columns.Add("Name");
            results.Columns.Add("T9 245");
            results.Columns.Add("T9 232");
            results.Columns.Add("245");
            results.Columns.Add("232");
            results.Columns.Add("226");
            results.Columns.Add("219");
            results.Columns.Add("213");
            results.Columns.Add("200");
            results.Columns.Add("Total");

            String name;
            int id, ilvl, slot;
            //for each member found
            foreach (XmlNode node in returnDocument.SelectNodes("members/character"))
            {
                name = node.Attributes["name"].Value;
                int nbT9245 = 0, nbT9232 = 0, nb245 = 0, nb232 = 0, nb226 = 0, nb219 = 0, nb213 = 0, nb200 = 0;
                int totalValue = 0, value16 = 0, value17 = 0;

                //for each item found
                foreach (XmlNode item in node.SelectNodes("Item"))
                {
                    //get item infos
                    id = int.Parse(item.Attributes["id"].Value);
                    ilvl = int.Parse(item.Attributes["ilvl"].Value);
                    slot = int.Parse(item.Attributes["slot"].Value);

                    //increment countersr by stuff (t9 type, ivl)
                    if (t9_245.Contains(id))
                        nbT9245++;
                    if (t9_232.Contains(id))
                        nbT9232++;
                    if (ilvl == 245)
                        nb245++;
                    if (ilvl == 232)
                        nb232++;
                    if (ilvl == 226)
                        nb226++;
                    if (ilvl == 219)
                        nb219++;
                    if (ilvl == 213)
                        nb213++;
                    if (ilvl == 200)
                        nb200++;

                    totalValue += ilvl;

                    //right hand
                    if (slot == 16)
                        value16 = ilvl;
                    //left hand
                    if (slot == 17)
                        value17 = ilvl;
                }

                //if no left hand, add the weapon a second time as it "sould" be a two handed weapon
                if (value16 == 0)
                    totalValue += value17;

                //print the stuff counters for current member
                if (name.Length > 7)
                    analyzeStr += name + "\t" + nbT9245 + "\t" + nbT9232;
                else
                    analyzeStr += name + "\t\t" + nbT9245 + "\t" + nbT9232;
                analyzeStr += " " + nb245 + " " + nb232 + " " + nb226 + " " + nb219 + " " + nb213 + " " + nb200;
                analyzeStr += " total:" + totalValue + "\r\n";

                ListViewItem item1 = new ListViewItem(name, 0);
                item1.SubItems.Add("" + nbT9245);
                item1.SubItems.Add("" + nbT9232);
                item1.SubItems.Add("" + nb245);
                item1.SubItems.Add("" + nb232);
                item1.SubItems.Add("" + nb226);
                item1.SubItems.Add("" + nb219);
                item1.SubItems.Add("" + nb213);
                item1.SubItems.Add("" + nb200);
                item1.SubItems.Add("" + totalValue);

                results.Items.Add(item1);
            }

            //save the results
            System.IO.File.WriteAllText("analyze.txt", analyzeStr);
        }

        class ListViewItemComparer : IComparer
        {
            private int col;
            private SortOrder order;
            public ListViewItemComparer()
            {
                col = 0;
                order = SortOrder.Ascending;
            }
            public ListViewItemComparer(int column, SortOrder order)
            {
                col = column;
                this.order = order;
            }
            public int Compare(object x, object y) 
            {
                int returnVal= -1;
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                        ((ListViewItem)y).SubItems[col].Text);
                // Determine whether the sort order is descending.
                if (order == SortOrder.Descending)
                    // Invert the value returned by String.Compare.
                    returnVal *= -1;
                return returnVal;
            }
        }

        private void results_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (guildMode)
                return;

            // Determine whether the column is the same as the last column clicked.
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                results.Sorting = SortOrder.Descending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (results.Sorting == SortOrder.Ascending)
                    results.Sorting = SortOrder.Descending;
                else
                    results.Sorting = SortOrder.Ascending;
            }

            // Call the sort method to manually sort.
            results.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.            
            this.results.ListViewItemSorter = new ListViewItemComparer(e.Column, results.Sorting);
        }
    }
}
