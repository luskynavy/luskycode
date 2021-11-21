using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CompareFiles
{
	internal class Options
	{
		public string src = "";
		public string dst = "";
		public string socket = "";
		public string host = "";
	}

	internal class Program
	{
		private static Options ManageOptions(string[] args)
		{
			Options options = new Options();
			for (int i = 0; i < args.Length;)
			{
				string s = args[i];

				if (s == "-socket")
				{
					if (i + 1 < args.Length)
					{
						options.socket = args[i + 1];
						i++;
					}
				}
				else if (s == "-host")
				{
					if (i + 1 < args.Length)
					{
						options.host = args[i + 1];
						i++;
					}
				}
				else
				{
					//first path is src
					if (string.IsNullOrEmpty(options.src))
					{
						options.src = args[i];
					}
					//second is dst
					else if (string.IsNullOrEmpty(options.dst))
					{
						options.dst = args[i];
					}
				}


				i++;
			}

			return options;
		}

		private static string[] GetFiles(string path)
		{
			string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

			for (int i = 0; i < files.Length; i++)
			{
				files[i] = files[i].Substring(path.Length);
			}

			Array.Sort(files, StringComparer.InvariantCulture);

			return files;
		}

		private static List<string> FindDstNotPresentInSrc(string[] filesSrc, string[] filesDst)
		{
			List<string> result = new List<string>();

			int indexSrc = 0, indexDst = 0;

			while (indexSrc < filesSrc.Length && indexDst < filesDst.Length)
			{
				int compareOrdinal = String.Compare(filesDst[indexDst], filesSrc[indexSrc], StringComparison.Ordinal);

				//dst == src
				if (compareOrdinal == 0)
				{
					//move two indexes
					indexSrc++;
					indexDst++;
				}
				//dst < src
				else if (compareOrdinal < 0)
				{
					result.Add(filesDst[indexDst]);
					indexDst++;
				}
				//dst > src
				else
				{
					indexSrc++;
				}
			}

			//remaining in dst
			for (; indexDst < filesDst.Length; indexDst++)
			{
				result.Add(filesDst[indexDst]);
			}

			return result;
		}

		public static void SerializeToXML(string[] arrayString)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(string[]));
			TextWriter textWriter = new StreamWriter("arrayString.xml");
			serializer.Serialize(textWriter, arrayString);
			textWriter.Close();
		}

		public static string[] DeserializeFromXML(/*Stream stream*/)
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(string[]));
			TextReader textReader = new StreamReader("arrayString.xml");
			string[] arrayString;
			//arrayString = (string[])deserializer.Deserialize(stream);
			arrayString = (string[])deserializer.Deserialize(textReader);
			textReader.Close();

			return arrayString;
		}

		public static void ServerMode(Int32 port, string path)
		{
			TcpListener server = null;
			try
			{
				// Set the TcpListener on port socket.
				IPAddress localAddr = IPAddress.Parse("127.0.0.1");

				// TcpListener server = new TcpListener(port);
				server = new TcpListener(localAddr, port);

				// Start listening for client requests.
				server.Start();

				// Buffer for reading data
				Byte[] bytes = new Byte[256];
				String data = null;

				// Enter the listening loop.
				while (true)
				{
					Console.Write("Waiting for a connection... ");

					// Perform a blocking call to accept requests.
					// You could also use server.AcceptSocket() here.
					TcpClient client = server.AcceptTcpClient();
					Console.WriteLine("Connected!");

					data = null;

					// Get a stream object for reading and writing
					NetworkStream stream = client.GetStream();

					int i;

					MemoryStream ms = new MemoryStream();

					// Loop to receive all the data sent by the client.
					while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						// Translate data bytes to a ASCII string.
						data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
						Console.WriteLine("Server Received:\n{0}", data);

						if (i != 0)
						{
							ms.Write(bytes, 0, i);
						}
					}

					Console.WriteLine("Server while finished");

					// Process the data sent by the client.
					//data = data.ToUpper();
					/*data = "OK";

					byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

					// Send back a response.
					stream.Write(msg, 0, msg.Length);
					Console.WriteLine("Server Sent: {0}", data);*/

					XmlSerializer deserializer = new XmlSerializer(typeof(string[]));
					string[] arrayString;
					ms.Position = 0;
					arrayString = (string[])deserializer.Deserialize(ms);

					//show src
					foreach (var f in arrayString)
					{
						Console.WriteLine(f);
					}

					ms.Close();

					// Shutdown and end connection
					client.Close();
				}
			}
			catch (SocketException e)
			{
				Console.WriteLine("SocketException: {0}", e);
			}
			finally
			{
				// Stop listening for new clients.
				server.Stop();
			}
		}

		public static void ClientMode(string server, Int32 port, string path)
		{
			try
			{
				// Create a TcpClient.
				// Note, for this client to work you need to have a TcpServer
				// connected to the same address as specified by the server, port
				// combination.
				TcpClient client = new TcpClient(server, port);

				// Translate the passed message into ASCII and store it as a Byte array.
				//string message = "test";
				//Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

				string[] filesSrc = GetFiles(path);
				XmlSerializer serializer = new XmlSerializer(typeof(string[]));
				MemoryStream ms = new MemoryStream();
				serializer.Serialize(ms, filesSrc);
				Byte[] data = ms.ToArray();
				ms.Close();

				// Get a client stream for reading and writing.
				//  Stream stream = client.GetStream();

				NetworkStream stream = client.GetStream();

				// Send the message to the connected TcpServer.
				stream.Write(data, 0, data.Length);

				Console.WriteLine("Client Sent size: {0}", data.Length);

				// Receive the TcpServer.response.

				// Buffer to store the response bytes.
				/*data = new Byte[256];

				// String to store the response ASCII representation.
				String responseData = String.Empty;

				// Read the first batch of the TcpServer response bytes.
				Int32 bytes = stream.Read(data, 0, data.Length);
				responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
				Console.WriteLine("Client Received: {0}", responseData);*/

				// Close everything.
				stream.Close();
				client.Close();
			}
			catch (ArgumentNullException e)
			{
				Console.WriteLine("ArgumentNullException: {0}", e);
			}
			catch (SocketException e)
			{
				Console.WriteLine("SocketException: {0}", e);
			}
		}


		private static void Main(string[] args)
		{
			Options options = ManageOptions(args);

			//client mode
			if (!string.IsNullOrEmpty(options.host) && !string.IsNullOrEmpty(options.src))
			{
				string[] serverPort = options.host.Split(':');
				ClientMode(serverPort[0], Int32.Parse(serverPort[1]), options.src);
			}
			//server mode
			else
			if (!string.IsNullOrEmpty(options.socket) && !string.IsNullOrEmpty(options.src))
			{
				ServerMode(Int32.Parse(options.socket), options.src);
			}
			//local mode
			else
			{
				//string path = @"D:\d\d";
				//string path = @"D:\d\rk";

				string[] filesSrc = GetFiles(options.src);
				string[] filesDst = GetFiles(options.dst);

				//sereialize test
				SerializeToXML(filesSrc);
				string[] filesSrcSerialized = DeserializeFromXML();

				//show src
				Console.WriteLine("src:");
				foreach (var f in filesSrc)
				{
					Console.WriteLine(f);
				}

				Console.WriteLine();

				//show dst
				Console.WriteLine("dst:");
				foreach (var f in filesDst)
				{
					Console.WriteLine(f);
				}

				Console.WriteLine();

				List<string> result = FindDstNotPresentInSrc(filesSrc, filesDst);

				//shor result
				Console.WriteLine("diff:");
				foreach (var f in result)
				{
					Console.WriteLine(f);
				}
			}
		}
	}
}