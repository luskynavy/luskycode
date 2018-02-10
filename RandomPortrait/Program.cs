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
            const string EXTENSION_BIG = "M.bmp";
            const string EXTENSION_SMALL = "S.bmp";

            StreamReader fileNames = null;
            try
            {
                fileNames = new StreamReader("RandomPortrait.txt");
            }
            catch (FileNotFoundException)
            {
            }

            if (fileNames != null)
            {
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

                //current dir for searching *EXTENSION_BIG
                string bmpDir = @".\";

                string[] files = Directory.GetFiles(bmpDir, "*" + EXTENSION_BIG);

                //if found some files
                if (files.Length > 0)
                {
                    //if more names than files not possible to not reuse allready choosen names
                    bool moreNamesThanFiles = names.Count > files.Length;

                    //only if possible (no more names than files)
                    if (moreNamesThanFiles)
                    {
                        //remove names from files to prevent giving more probability to be choosen
                        int found;
                        foreach (string n in names)
                        {
                            found = Array.IndexOf(files, bmpDir + n + EXTENSION_BIG);
                            if (found != -1)
                            {
                                files[found] = "";
                            }
                        }
                    }

                    Random rnd = new Random();

                    foreach (string n in names)
                    {
                        //choose a random file
                        int choice = rnd.Next(0, files.Length);

                        //prevent reuse by skipping empty names
                        while (files[choice] == "")
                        {
                            choice = rnd.Next(0, files.Length);
                        }

                        Console.WriteLine(files[choice] + " => " + bmpDir + n + EXTENSION_BIG);
                        //Console.WriteLine(files[choice].Substring(0, files[choice].IndexOf(".bmp")) + EXTENSION_SMALL + "\n =>" + bmpDir + n + EXTENSION_SMALL);
                        Console.WriteLine(files[choice].Replace(EXTENSION_BIG, EXTENSION_SMALL) + " => " + bmpDir + n + EXTENSION_SMALL);

                        try
                        {
                            File.Copy(files[choice], bmpDir + n + EXTENSION_BIG, true);
                            File.Copy(files[choice].Replace(EXTENSION_BIG, EXTENSION_SMALL), bmpDir + n + EXTENSION_SMALL, true);
                        }
                        catch (FileNotFoundException)
                        {
                        }

                        //prevent reuse by emptying used file only if possible
                        if (!moreNamesThanFiles)
                        {
                            files[choice] = "";
                        }
                    }
                }
            }
        }
    }
}