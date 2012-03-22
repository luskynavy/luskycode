using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;


namespace RandWallpaper
{
    public static class Win32 
    {
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SystemParametersInfo (int uAction , int uParam , string lpvParam , int fuWinIni) ;
    };

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 1)
            {
                DirectoryInfo dir = new DirectoryInfo(args[0]);
                FileInfo[] files = null;
                try
                {
                    files = dir.GetFiles();
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Invalid directory: " + args[0]);
                }

                //if directory exists and there are files
                if (files != null && files.Count() > 0)
                {
                    Random rand = new Random();
                    int i = rand.Next(files.Count());

                    string wallpaper = args[0] + @"\" + files[i].Name;

                    int nResult = Win32.SystemParametersInfo(20/*SPI_SETDESKWALLPAPER*/, 0, wallpaper, 0x1 | 0x2);
                    if (nResult != 1)
                    {
                        Console.WriteLine("SystemParametersInfo SPI_SETDESKWALLPAPER failed for " + wallpaper);
                    }
                    else
                    {
                        Console.WriteLine(wallpaper + " was choosen.");
                    }
                }
            }
            else
            {
                Console.WriteLine("You must specify a directory as argument.");
            }
        }
    }
}
