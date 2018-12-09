using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace RemoveNonAlpha
{
    class Program
    {
        static void Main(string[] args)
        {
            //current dir for searching
            string bmpDir = ".";

            string[] files = Directory.GetFiles(bmpDir, "*");

            //if found some files
            if (files.Length > 0)
            {
                string fCleaned;
                foreach (string f in files)
                {
                    Regex rgx = new Regex("[^a-zA-Z0-9 -.]");
                    fCleaned = rgx.Replace(f, "");
                    fCleaned = fCleaned.Replace("--", "-");
                    fCleaned = fCleaned.Replace("--", "-");
                    fCleaned = fCleaned.Replace("--", "-");
                    Console.WriteLine("copy /b " + f.Substring(2) + " " + fCleaned.Substring(1));
                }
            }        
        }
    }
}
