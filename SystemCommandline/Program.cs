using System;
using System.CommandLine;
using System.IO;
using System.Linq;

namespace SystemCommandline
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Required option, file to be read
            var fileOption = new Option<FileInfo>(
                name: "--file",
                description: "The file to read and display on the console.")
            {
                IsRequired = true
            };
            fileOption.AddAlias("-f");

            //Optional option for detailed info
            var verboseOption = new Option<bool>(
                name: "--verbose",
                description: "Show detailed information.");
            verboseOption.AddAlias("-V");

            var rootCommand = new RootCommand("Sample app for System.CommandLine");
            rootCommand.AddOption(fileOption);
            rootCommand.AddOption(verboseOption);

            rootCommand.SetHandler((file, verbose) =>
                {
                    ReadFile(file, verbose);
                },
                fileOption, verboseOption);

            rootCommand.InvokeAsync(args);
        }

        /// <summary>
        /// Manage option and show file
        /// </summary>
        /// <param name="file">file to read</param>
        /// <param name="verbose">show detailed info</param>
        static void ReadFile(FileInfo file, bool verbose)
        {
            if (file != null)
            {
                if (verbose)
                {
                    Console.WriteLine("======> Start of file");
                }

                try
                {
                    File.ReadLines(file.FullName).ToList()
                        .ForEach(line => Console.WriteLine(line));
                }
                catch (Exception)
                {
                    Console.WriteLine("Error reading file :" + file.FullName);
                }

                if (verbose)
                {
                    Console.WriteLine("======> End of file");
                }
            }

        }
    }
}
