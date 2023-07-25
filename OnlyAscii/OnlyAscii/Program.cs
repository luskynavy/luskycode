using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OnlyAscii
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string pwd = Path.GetFileName(Environment.CurrentDirectory);

            DirectoryInfo d = new DirectoryInfo(@"."); //in current folder

            //order descending by name length to avoid name override
            //so shorter name will not override longer name starting with folder
            IOrderedEnumerable<FileInfo> orderedEnumerable = d.GetFiles("*.*").OrderByDescending(f => f.Name.Length);
            //FileInfo[] Files = orderedEnumerable; //Getting files

            foreach (FileInfo file in orderedEnumerable)
            {
                string newName = Regex.Replace(file.Name, @"[^\u0000-\u007F]+", string.Empty);

                //only if newname is different
                if (file.Name != newName)
                {
                    Console.WriteLine(file.Name + " => " + newName);
                    //remove non ascii chars
                    try
                    {
                        System.IO.File.Move(file.Name, newName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        continue;
                    }
                }
            }
        }
    }
}