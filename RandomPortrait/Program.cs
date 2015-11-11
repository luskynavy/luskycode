using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace RandomPortrait
{
    class Program
    {
        static void Main(string[] args)
        {
            //string[] names = { "Xzar", "Montar" , "Khalid", "Jaheira", "Imoen", "Maghia" };
            StreamReader fileNames = new StreamReader("RandomPortrait.txt");

            //read file of names
            List<String> names = new List<String>();
            string line;
            while ((line = fileNames.ReadLine()) != null)
            {
                if (line != "")
                {
                    Console.WriteLine(line);
                    names.Add(line);
                }
            }

            fileNames.Close();
            
            //current dir for searching *M.bmp
            string bmpDir = @".\";

            string[] files = Directory.GetFiles(bmpDir, "*M.bmp");

            //remove names from files to prevent giving more probability to be choosen
            int found;
            foreach (string n in names)
            {
                found = Array.IndexOf(files, bmpDir + n + "M.bmp");
                if (found != -1)
                {
                    files[found] = "";
                }
            }

            Random rnd = new Random();

            foreach(string n in names)
            {
                //choose a random file
                int choice = rnd.Next(0, files.Length);

                //prevent reuse by skipping empty names
                while (files[choice] == "")
                {
                    choice = rnd.Next(0, files.Length);
                }
                
                Console.WriteLine(files[choice] + " => " + bmpDir + n + "M.bmp");
                //Console.WriteLine(files[choice].Substring(0, files[choice].IndexOf(".bmp")) + "S.bmp" + "\n =>" + bmpDir + n + "S.bmp");
                Console.WriteLine(files[choice].Replace("M.bmp","S.bmp") + " => " + bmpDir + n + "S.bmp");

                File.Copy(files[choice], bmpDir + n + "M.bmp", true);
                File.Copy(files[choice].Replace("M.bmp", "S.bmp"), bmpDir + n + "S.bmp", true);
                
                //prevent reuse by emptying used file
                files[choice] = "";
            }
        }
    }
}
