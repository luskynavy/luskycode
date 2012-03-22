using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Net.Sockets;

using System.Net;

// Who is de Romain
// http://www.netreport.fr/whois/redirect.asp?ip=82.67.16.100<asp>

// Mapping IP addresses to country codes
// http://www.codeproject.com/KB/cs/iplookupoptimise.aspx?msg=502031


namespace WhoIs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Init IPCountryTable
            table = new IPCountryTable(1/*indexLength*/);
//             table.LoadStatisticsFile(@"..\..\resources\ripencc.latest", true);
//             table.LoadStatisticsFile(@"..\..\resources\arin.latest", true);
//             table.LoadStatisticsFile(@"..\..\resources\apnic.latest", false);
//             table.LoadStatisticsFile(@"..\..\resources\lacnic.latest", true);

            //Source
//         ftp://ftp.afrinic.net/pub/stats/afrinic/delegated-afrinic-latest
//         ftp://ftp.apnic.net/pub/stats/apnic/delegated-apnic-latest
//         ftp://ftp.arin.net/pub/stats/arin/delegated-arin-latest
//         ftp://ftp.lacnic.net/pub/stats/lacnic/delegated-lacnic-latest
// 
//         ftp://ftp.ripe.net/ripe/stats/delegated-ripencc-latest
//         ftp://ftp.apnic.net/pub/stats/iana/delegated-iana-latest

            table.LoadStatisticsFile(@"..\..\resources\delegated-iana-latest", true);
            table.LoadStatisticsFile(@"..\..\resources\delegated-apnic-latest", true);
            table.LoadStatisticsFile(@"..\..\resources\delegated-ripencc-latest.txt", true);
            table.LoadStatisticsFile(@"..\..\resources\delegated-lacnic-latest", true);
            table.LoadStatisticsFile(@"..\..\resources\delegated-arin-latest", true);
            table.LoadStatisticsFile(@"..\..\resources\delegated-afrinic-latest", true);
        }

        // Source : http://www.asp-php.net/ressources/bouts_de_code.aspx?id=961

        /// <summary>
        /// Interroge un serveur Whois pour le nom de domaine spécifié.
        /// </summary>
        /// <param name="domain">Nom de domaine à vérifier.</param>
        /// <param name="server">Adresse du serveur Whois à interroger.</param>
        /// <param name="port">Port du serveur Whois à interroger.</param>
        /// <returns>Résultat de l'interrogation du serveur Whois, ou une référence null en cas d'erreur.</returns>
        /// <exception cref="ArgumentException"><paramref name="domain" /> ou <paramref name="server" /> est vide.</exception>
        public static string QueryWhoisServer(string domain, string server, int port)
        {
            // Validation des arguments
            //if (string.IsNullOrEmpty(domain)) throw new ArgumentException("Domain name can't be empty", "domain");
            //if (string.IsNullOrEmpty(server)) throw new ArgumentException("Whois server can't be empty", "server");

            try
            {
                // Ouverture de la connexion au serveur : les services Whois utilisent en général le port 43
                using (var client = new TcpClient(server, port))
                {
                    // Ouverture du flux de données
                    using (var stream = client.GetStream())
                    {
                        // Envoi de la requête Whois
                        var query = Encoding.ASCII.GetBytes(domain + "\r\n");
                        stream.Write(query, 0, query.Length);

                        // Lecture de la réponse
                        using (var reader = new StreamReader(stream, Encoding.ASCII)) return reader.ReadToEnd().Trim();
                    }
                }
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static IPCountryTable table;

        private void Go()
        {
            // IpLookupCountry
            IPAddress ip;

            if (textBoxIp.Text[0] > '9')
                ip = System.Net.Dns.GetHostEntry(textBoxIp.Text).AddressList[0];
            else
                ip = IPAddress.Parse(textBoxIp.Text);
            String resultIp = table.GetCountry(ip.ToString());

            textBoxCountry.Text = resultIp;


            // Refresh app to show info during next phase
            Application.DoEvents();


            // NsLookup
            IPHostEntry ipEntry;

            try
            {
                ipEntry = Dns.GetHostEntry(textBoxIp.Text);
                textBoxNslookup.Text = ipEntry.HostName + ": ";
                for (int i = 0; i < ipEntry.AddressList.Length; i++)
                {
                    if (i > 0)
                    {
                        textBoxNslookup.Text += ", ";
                    }
                    textBoxNslookup.Text += ipEntry.AddressList[i];
                }
            }
            catch (Exception ex)
            {
                textBoxNslookup.Text = ex.Message;
            }


            // Refresh app to show info during next phase
            Application.DoEvents();


            return;

            // Do the who is
            string result = QueryWhoisServer(textBoxIp.Text, textBoxServerIp.Text, 43/*0 + textBoxServerPort.Text*/);
            if (result != null)
            {
                string resultNewLine = result.Replace("\n", "\r\n");
                textBoxResult.Text = resultNewLine;
            }
            else
            {
                textBoxResult.Text = "null";
            }            
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            Go();
        }       
    }
}


