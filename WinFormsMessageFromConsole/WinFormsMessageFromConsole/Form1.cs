using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace WinFormsMessageFromConsole
{
    public partial class Form1 : Form
    {
        Socket mListenSocket;
        public Form1()
        {
            InitializeComponent();

            //IPAddress localAddr = IPAddress.Any;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 33000;

            // Create the socket
            mListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //mListenSocket.Blocking = false;

            // Establish the local endpoint for the socket
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            // Bind the listening socket to the port
            mListenSocket.Bind(localEndPoint);

            // Start listening
            mListenSocket.Listen(10);
        }

        // This is called on the UI thread, so we can update the label directly.
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = "Winforms Timer " + DateTime.Now.ToString("T");

            byte[] data = new byte[8192];

            // Check if there is a pending connection request. If there is, accept it and read the data.
            if (mListenSocket.Poll(0, SelectMode.SelectRead))
            {
                // Accept the connection and read the data.
                Socket handler = mListenSocket.Accept();
                handler.Blocking = false;
                handler.Receive(data);
                textBox1.Text = DateTime.Now.ToString("T") + " Received: " + System.Text.Encoding.UTF8.GetString(data);
                handler.Close();
                Array.Clear(data, 0, data.Length);
                /*if (serverSocket.Available > 0)
                {
                    serverSocket.Receive(data);
                }*/
            }
        }
    }
}
