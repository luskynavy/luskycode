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

            BinaryReader binReader = new BinaryReader(File.Open(@"..\..\MTF000000000000000000320388.TIF", FileMode.Open));
            //BinaryReader binReader = new BinaryReader(File.Open(@"..\..\MTF000000000000000000320142.TIF", FileMode.Open));
            //BinaryReader binReader = new BinaryReader(File.Open(@"..\..\MTF000000000000000000320037.TIF", FileMode.Open));


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
                    Console.Write(imageNumber + " : IFD " + IFD);

                    //se déplace sur l'IFD
                    binReader.BaseStream.Seek(IFD, SeekOrigin.Begin);

                    //lit le nb de tags
                    short tagNumber = binReader.ReadInt16();

                    //Console.WriteLine(imageNumber + " : IFD " + IFD + " tagNumber " + tagNumber);

                    Console.WriteLine(" tagNumber " + tagNumber);

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
            }
            //lecture en dehors du fichier ?
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine("error in file, last IFD " + IFD + " (fileSize " + fileSize + ")");
            }
        }
    }
}
