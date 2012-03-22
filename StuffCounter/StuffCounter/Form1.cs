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

namespace StuffCounter
{
    public partial class Form1 : Form
    {
        //5 seems to be the magic number when the armory is acting up.
        private const int RETRY_MAX = 5;
        string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";

        public Form1()
        {
            InitializeComponent();
        }

        private static Exception _fatalError = null;

        /// <summary>
        /// If the last request received a 407 or no response. Used to prevent a lot of bad calls.
        /// It also has the good side effect of not locking someone's account out if they enter the proxy info incorrectly
        /// by sending lots of bad authorization attempts.
        /// </summary>
        public static bool LastWasFatalError
        {
            get { return _fatalError != null; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xml = DownloadXml("http://eu.wowarmory.com/guild-info.xml?r=Eldre%27Thalas&gn=Ancestr%C3%A4l");
            //results.Text = xml.InnerXml;
            results.Text = "";
            string name;
            int level;
            int rank;
            foreach (XmlNode node in xml.SelectNodes("page/guildInfo/guild/members/character"))
            {
                rank = int.Parse(node.Attributes["rank"].Value);
                if (rank >= 0 && rank <= 3 || rank == 7)
                {
                    level = int.Parse(node.Attributes["level"].Value);
                    name = node.Attributes["name"].Value;
                    results.Text += name + " " + level + " " + rank;
                    results.Text += "\r\n";
                }
            };
        }

        /// <summary>
		/// This is used to prevent multiple attempts at network traffic when its not working and 
		/// continuing to issue requests could cause serious problems for the user.
		/// </summary>
		/// <param name="ex"></param>
		private void CheckExceptionForFatalError(Exception ex)
		{
			//Log.Write("Exception trying to download: "+ ex);
            //Log.Write(ex.StackTrace);
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

        /// <summary>
        /// Used to create a web client with all of the appropriote proxy/useragent/etc settings
        /// </summary>
        private WebClient CreateWebClient()
        {
            WebClient client = new WebClient() { Encoding = Encoding.UTF8 };
            client.Headers.Add("user-agent", UserAgent);
           /* if (NetworkSettingsProvider.ProxyType == "Http")
            {
                //if (_useDefaultProxy)
                {
                    client.Proxy = HttpWebRequest.DefaultWebProxy;
                }
                else if (!String.IsNullOrEmpty(_proxyServer))
                {
                    client.Proxy = new WebProxy(_proxyServer, _proxyPort);
                }
                if (client.Proxy != null && NetworkSettingsProvider.ProxyRequiresAuthentication)
                {
                    if (NetworkSettingsProvider.UseDefaultAuthenticationForProxy)
                    {
                        client.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
                    }
                    else
                    {
                        client.Proxy.Credentials = new NetworkCredential(_proxyUserName, _proxyPassword, _proxyDomain);
                    }
                }
            }*/
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
