using System;
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
            //string s = System.Web.HttpUtility.UrlEncode("Eldre'Thalas Ancesträl");            
            ReadItemCache();
            GetStuff("Lusky");
        }

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
            string xml = System.IO.File.ReadAllText("../../ItemCache.xml");
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
                //results.Text += ilvl + " " + id;
                //results.Text += "\r\n";
            }
        }

        private void GetStuff(string name)
        {
            XmlDocument xml = DownloadXml("http://eu.wowarmory.com/character-sheet.xml?r=" + HttpUtility.UrlEncode(realm.Text) + "&n=" + HttpUtility.UrlEncode(name));
            if (xml != null)
            {
                int nb245 = 0;
                int nb232 = 0;
                int nb226 = 0;
                int nb219 = 0;
                int nb213 = 0;
                int nb200 = 0;
                int id;
                int slot;
                foreach (XmlNode node in xml.SelectNodes("page/characterInfo/characterTab/items/item"))
                {
                    id = int.Parse(node.Attributes["id"].Value);
                    slot = int.Parse(node.Attributes["slot"].Value);
                    //-1 = ammo, 3 = shirt, 18 = tabard
                    if (slot != -1 && slot != 3 && slot != 18 && itemCache.ContainsKey(id))
                    {
                        if (itemCache[id].ilvl == 245)
                            nb245++;
                        if (itemCache[id].ilvl == 232)
                            nb232++;
                        if (itemCache[id].ilvl == 226)
                            nb226++;
                        if (itemCache[id].ilvl == 219)
                            nb219++;
                        if (itemCache[id].ilvl == 213)
                            nb213++;
                        if (itemCache[id].ilvl == 200)
                            nb200++;
                        results.Text += " " + id + ":" + itemCache[id].ilvl;
                    }
                    else
                    {
                        //results.Text += " " + id + ":" + 0;
                    }
                }
                results.Text += " 245:" + nb245 + " 232:" + nb232 + " 226:" + nb226 + " 219:" + nb219 + " 213:" + nb213 + " 200:" + nb200;
            }
        }

        private void go_Click(object sender, EventArgs e)
        {            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //XmlDocument xml = DownloadXml("http://eu.wowarmory.com/guild-info.xml?r=Eldre%27Thalas&gn=Ancestr%C3%A4l");
            XmlDocument xml = DownloadXml("http://eu.wowarmory.com/guild-info.xml?r=" + HttpUtility.UrlEncode(realm.Text) + "&gn=" + HttpUtility.UrlEncode(guild.Text));
            //results.Text = xml.InnerXml;
            if (xml != null)
            {
                results.Text = "";
                string name;
                int level;
                int rank;
                foreach (XmlNode node in xml.SelectNodes("page/guildInfo/guild/members/character"))
                {
                    rank = int.Parse(node.Attributes["rank"].Value);
                    if (rank == 0 || rank == 1 || rank == 3 || rank == 7)
                    {
                        level = int.Parse(node.Attributes["level"].Value);
                        name = node.Attributes["name"].Value;
                        results.Text += name + " " + level + " " + rank;
                        GetStuff(name);
                        results.Text += "\r\n";
                        Application.DoEvents();
                    }
                }
            }
            else
            {
                results.Text += "Guild info not available\r\n";
            }

            stopwatch.Stop();
            long elapsedTimeInMilliSeconds = stopwatch.ElapsedMilliseconds;
            results.Text += "Total time: " + elapsedTimeInMilliSeconds / 1000;
        }
        
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
                                    || !returnDocument.DocumentElement.HasChildNodes
                            /*|| !returnDocument.DocumentElement.ChildNodes[0].HasChildNodes*/) // this check is no longer valid
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
    }
}
