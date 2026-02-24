using System.Net;
using System.Net.Sockets;

// Manage parameters and make sure a port and message are specified.
if (args.Count() <= 1)
{
    Console.WriteLine("Usage: ConsoleMessageSender <port> <message>");
    return;
}

//string server = "localhost";
string server = "127.0.0.1";
Int32 port = Int32.Parse(args[0]);

var data = System.Text.Encoding.UTF8.GetBytes(args[1]);


// Tcp Client and Socket examples are provided. The TcpClient class is easier to use,
// but the Socket class provides more control. Uncomment the section you want to use.
#if true
// Create a TcpClient.
TcpClient client = new TcpClient(server, port);

// Translate the passed message into ASCII and store it as a Byte array.
NetworkStream stream = client.GetStream();

// Send the message to the connected TcpServer.
stream.Write(data);

Console.WriteLine("Client Sent size: {0}", data.Length);

// Close everything.
stream.Close();
client.Close();

#else

// Create a TCP/IP  socket.
IPEndPoint ipEndPoint = new(IPAddress.Parse(server), port);
using Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

// Connect the socket to the remote endpoint
client.Connect(ipEndPoint);

// Blocks until send returns.
int byteCount = client.Send(data, 0, data.Length, SocketFlags.None);
Console.WriteLine("Sent {0} bytes.", byteCount);
client.Shutdown(SocketShutdown.Both);

#endif

/*
// Receive the response from the remote device.
byte[] buffer = new byte[1024];
int iRx = socket.Receive(buffer);
char[] chars = new char[iRx];

System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
System.String recv = new System.String(chars);
Console.WriteLine("Received: {0}", recv);
*/


