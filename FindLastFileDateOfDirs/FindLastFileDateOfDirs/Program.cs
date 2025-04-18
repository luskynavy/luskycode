namespace FindLastFileDateOfDirs
{
    internal class Program
    {
        //How to make a single file program, not needing .dll or .json:
        //publish the project as folder with
        //- deployement mode : framework dependent
        //- runtime: win-x64
        //- publish options : produce single file

        /// <summary>
        /// Find directories olders than their most recent file
        /// </summary>
        /// <param name="dir">search in this directory</param>
        static void FindLastFileDateOfDirs(string dir)
        {
            //Find all directories
            string[] dirs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);

            //Search in all directories
            foreach (string dirName in dirs)
            {
                try
                {
                    //Get modified time of current directory
                    DateTime dtDir = File.GetLastWriteTime(dirName);

                    //Get file with last modified date
                    var directory = new DirectoryInfo(dirName);
                    var lastFile = directory.GetFiles()
                                 .OrderByDescending(f => f.LastWriteTime)
                                 .FirstOrDefault();

                    //if there is at least a file
                    if (lastFile != null)
                    {
                        //if file is more recent with at least 1 hour
                        if ((lastFile.LastWriteTime - dtDir).TotalHours > 1)
                        {
                            Console.WriteLine(Path.GetFileName(dirName));
                        }
                    }
                }
                catch (Exception)
                {
                    //Do nothing, just don't stop
                }
            }
        }
        static void Main(string[] args)
        {
            //Use directory passed in argument
            if (args.Length >= 1)
            {
                FindLastFileDateOfDirs(args[0]);
            }
            //or current directory
            else
            {
                FindLastFileDateOfDirs(Directory.GetCurrentDirectory());
            }
            Console.WriteLine("Press a key to quit");
            Console.ReadKey();
        }
    }
}
