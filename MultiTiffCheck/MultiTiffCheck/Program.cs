using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

//format here http://www.awaresystems.be/imaging/tiff/faq.html

namespace MultiTiffCheck
{
	class Program
	{
		static void Main(string[] args)
		{
			//BinaryReader binReader = new BinaryReader(File.Open(@"..\..\MTF000000000000000000320388.TIF", FileMode.Open));
			//BinaryReader binReader = new BinaryReader(File.Open(@"..\..\MTF000000000000000000320389.TIF", FileMode.Open));
			//BinaryReader binReader = new BinaryReader(File.Open(@"..\..\MTF000000000000000000320142.TIF", FileMode.Open));
			//BinaryReader binReader = new BinaryReader(File.Open(@"..\..\MTF000000000000000000320037.TIF", FileMode.Open));
			if (args.Length > 0)
			{
				//string[] files = System.IO.Directory.GetFiles(@"..\..\", "*.tif");
				string[] files = System.IO.Directory.GetFiles(args[0], "*.tif");
				foreach (string f in files)
				{
					//ouverture du fichier
					BinaryReader binReader = null;
					try
					{
						//binReader = new BinaryReader(File.Open(args[0], FileMode.Open));
						binReader = new BinaryReader(File.Open(f, FileMode.Open));
					}
					catch (Exception)
					{
					}

					//si le fichier a pu être ouvert
					if (binReader != null)
					{
						//cherche la taille du fichier
						long fileSize = binReader.BaseStream.Seek(0, SeekOrigin.End);

						//retour au début
						binReader.BaseStream.Seek(0, SeekOrigin.Begin);

						int imageNumber = 0;

						uint IFD = uint.MaxValue;

						short byteOrder = binReader.ReadInt16(); //'II' pour intel byte order, 'MM' sinon
						short version = binReader.ReadInt16(); //toujours 42
						IFD = binReader.ReadUInt32(); //lecture du 1er offset (Image File Directory)

						try
						{
							//si l'offset est à 0, on a atteint la dernière image
							while (IFD != 0)
							{
								//Console.Write(imageNumber + " : IFD " + IFD + "(0x" + String.Format("{0:X}", IFD) + ")");

								//se déplace sur l'IFD
								binReader.BaseStream.Seek(IFD, SeekOrigin.Begin);

								//lit le nb de tags
								short tagNumber = binReader.ReadInt16();

								//Console.WriteLine(imageNumber + " : IFD " + IFD + " tagNumber " + tagNumber);

								//Console.WriteLine(" tagNumber " + tagNumber);

								//lit les données de chaque tag
								for (int i = 0; i < tagNumber; i++)
								{
									short tagIdentifyingCode = binReader.ReadInt16();
									short datatypeOftagData = binReader.ReadInt16();
									uint numberOfValues = binReader.ReadUInt32();

									//pas de valeurs, on skippe
									if (numberOfValues > 0)
									{
										uint tagData = binReader.ReadUInt32();
									}
								}

								imageNumber++;

								//lecture de l'offset suivant
								IFD = binReader.ReadUInt32();
							}

							binReader.Close();
						}
						//lecture en dehors du fichier ?
						catch (Exception)
						{
							Console.WriteLine();
							Console.WriteLine("error in file " + f + ", last IFD " + IFD + " (fileSize " + fileSize + ")");
						}
					}
					else
					{
						//fichier inexistant ?
						//if (args.Length > 0)
						{
							//Console.WriteLine("Can not open file " + args[0]);
							Console.WriteLine("Can not open file " + f);
						}
						//sinon paramètre manquant
						/*else
						{
							Console.WriteLine("No file specified");
						}*/
					}
				}
			}
			else
			{
				Console.WriteLine("No directory specified");
			}
		}
	}
}
