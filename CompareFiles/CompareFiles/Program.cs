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
		public string srcPath = "";
		public string dstPath = "";
		public string socket = "";
		public string host = "";
		public bool verbose = false;
	}

	internal class Program
	{
		//InvariantCulture ?  Ordinal ? InvariantCultureIgnore ? OrdinalIgnoreCase ?
		private static readonly StringComparer stringComparer = StringComparer.InvariantCulture;

		private const StringComparison stringComparison = StringComparison.InvariantCulture;

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
				else if (s == "-verbose")
				{
					options.verbose = true;
				}
				else
				{
					//first path is srcPath
					if (string.IsNullOrEmpty(options.srcPath))
					{
						options.srcPath = args[i];
					}
					//second is dstPath
					else if (string.IsNullOrEmpty(options.dstPath))
					{
						options.dstPath = args[i];
					}
				}


				i++;
			}

			return options;
		}

		private static string[] GetFiles(string path)
		{
			string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

			//remove base path
			for (int i = 0; i < files.Length; i++)
			{
				files[i] = files[i].Substring(path.Length + 1);
			}

			Array.Sort(files, stringComparer);

			return files;
		}

		private static List<string> FindDstNotPresentInSrc(string[] filesSrc, string[] filesDst)
		{
			List<string> result = new List<string>();

			int indexSrc = 0, indexDst = 0;

			while (indexSrc < filesSrc.Length && indexDst < filesDst.Length)
			{
				int compareOrdinal = String.Compare(filesDst[indexDst].ToLower(), filesSrc[indexSrc].ToLower(), stringComparison);

				//dstPath == srcPath
				if (compareOrdinal == 0)
				{
					//move two indexes
					indexSrc++;
					indexDst++;
				}
				//dstPath < srcPath
				else if (compareOrdinal < 0)
				{
					result.Add(filesDst[indexDst]);
					indexDst++;
				}
				//dstPath > srcPath
				else
				{
					indexSrc++;
				}
			}

			//remaining in dstPath
			for (; indexDst < filesDst.Length; indexDst++)
			{
				result.Add(filesDst[indexDst]);
			}

			return result;
		}

		private static void SerializeToXML(string[] arrayString)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(string[]));
			TextWriter textWriter = new StreamWriter("arrayString.xml");
			serializer.Serialize(textWriter, arrayString);
			textWriter.Close();
		}

		private static string[] DeserializeFromXML()
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(string[]));

			TextReader textReader = new StreamReader("arrayString.xml");
			string[] arrayString;

			//arrayString = (string[])deserializer.Deserialize();
			arrayString = (string[])deserializer.Deserialize(textReader);

			textReader.Close();

			return arrayString;
		}

		private static IPAddress GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip;
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}

		private static void ServerMode(Int32 port, string path, bool verbose)
		{
			TcpListener server = null;
			try
			{
				// Set the TcpListener on port socket.
				//IPAddress localAddr = IPAddress.Parse("127.0.0.1");
				//var z = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0]; //find the one with 	AddressFamily	InterNetwork and not AddressFamily	InterNetworkV6 ?

				//IPAddress localAddr = Dns.Resolve(Dns.GetHostName()).AddressList[0];
				//IPAddress localAddr = GetLocalIPAddress();
				IPAddress localAddr = IPAddress.Any;

				// TcpListener server = new TcpListener(port);
				server = new TcpListener(localAddr, port);

				// Start listening for client requests.
				server.Start();

				// Buffer for reading data
				Byte[] bytes = new Byte[256];
				//String data = null;

				// Enter the listening loop.
				//no loop, just exit after finding differences
				//while (true)
				{
					Console.Write("Waiting for a connection... ");

					// Perform a blocking call to accept requests.
					// You could also use server.AcceptSocket() here.
					TcpClient client = server.AcceptTcpClient();
					Console.WriteLine("Connected!");

					//data = null;

					// Get a stream object for reading and writing
					NetworkStream stream = client.GetStream();

					int i;

					MemoryStream ms = new MemoryStream();

					// Loop to receive all the data sent by the client.
					while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						// Translate data bytes to a ASCII string.
						//data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
						//Console.WriteLine("Server Received:\n{0}", data);

						if (i != 0)
						{
							ms.Write(bytes, 0, i);
						}
					}

					//Console.WriteLine("Server while finished");

					// Process the data sent by the client.
					/*data = "OK";

					byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

					// Send back a response.
					stream.Write(msg, 0, msg.Length);
					Console.WriteLine("Server Sent: {0}", data);*/

					//deserialize srcPath
					XmlSerializer deserializer = new XmlSerializer(typeof(string[]));
					string[] arrayString;
					//rewind memory stream since Deserialize use current position
					ms.Position = 0;
					arrayString = (string[])deserializer.Deserialize(ms);

					//get local files
					string[] filesDst = GetFiles(path);

					//show results
					ShowResults(arrayString, filesDst, verbose);

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

		private static void ClientMode(string server, Int32 port, string path)
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

		private static void LocalMode(string src, string dst, bool verbose)
		{
			string[] filesSrc = GetFiles(src);
			string[] filesDst = GetFiles(dst);

			//serialize test
			//SerializeToXML(filesSrc);
			//string[] filesSrcSerialized = DeserializeFromXML();

			ShowResults(filesSrc, filesDst, verbose);
		}

		private static void ShowResults(string[] filesSrc, string[] filesDst, bool verbose)
		{
			if (verbose)
			{
				//show srcPath
				Console.WriteLine("src:");
				foreach (var f in filesSrc)
				{
					Console.WriteLine(f);
				}

				Console.WriteLine();

				//show dstPath
				Console.WriteLine("dst:");
				foreach (var f in filesDst)
				{
					Console.WriteLine(f);
				}

				Console.WriteLine();
			}

			List<string> result = FindDstNotPresentInSrc(filesSrc, filesDst);
			if (verbose)
			{
				//show result
				Console.WriteLine("diff:");
			}

			foreach (var f in result)
			{
				Console.WriteLine(f);
			}
		}

		private static void TestDiff(string srcFileName, string dstFileName)
		{
			string[] src = System.IO.File.ReadLines(srcFileName).ToArray();
			string[] dst = System.IO.File.ReadLines(dstFileName).ToArray();

			Array.Sort(src, stringComparer);
			Array.Sort(dst, stringComparer);

			ShowResults(src, dst, false);
		}


		private static void Main(string[] args)
		{
			//used to test FindDstNotPresentInSrc
			//TestDiff(@"..\..\src.txt", @"..\..\dst.txt");
			//return;

			Options options = ManageOptions(args);

			//client mode if -host and path are set
			if (!string.IsNullOrEmpty(options.host) && !string.IsNullOrEmpty(options.srcPath))
			{
				string[] serverPort = options.host.Split(':');
				ClientMode(serverPort[0], Int32.Parse(serverPort[1]), options.srcPath);
			}
			//server mode if -client and path are set
			else
			if (!string.IsNullOrEmpty(options.socket) && !string.IsNullOrEmpty(options.srcPath))
			{
				ServerMode(Int32.Parse(options.socket), options.srcPath, options.verbose);
			}
			//local mode
			else
			{
				LocalMode(options.srcPath, options.dstPath, options.verbose);
			}
		}
	}
}