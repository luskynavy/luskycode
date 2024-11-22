namespace FindBadlySavedWithJpgExt
{
    internal class Program
    {
        //How to make a single file program, not needing .dll or .json:
        //publish the project with
        //- deployement mode : framework dependent
        //- publish options : produce single file

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine("Searching in " + args[0]);
                FindBadImages(args[0]);
            }
        }

        static void FindBadImages(string path)
        {
            long totalSize = 0;

            int pathSize = path.Length;

            //path ends witdh \", sould remove ", os has not doubled ending \ in path starting and ending with "
            if (path[path.Length - 1] == '"')
            {
                path = path.Substring(0, path.Length - 1);
            }

            var dir = new DirectoryInfo(path);
            FileInfo[] files;

            //Find jpg files recursively
            try
            {
                files = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong path : " + path);
                return;
            }

            var buffer = new byte[4];


            foreach (FileInfo file in files)
            {
                try
                {
                    //Open file in read only
                    using (var stream = File.Open(file.FullName, FileMode.Open,FileAccess.Read))
                    using (var reader = new BinaryReader(stream))
                    {
                        int bytesRead = reader.Read(buffer, 0, buffer.Length);

                        //jpg magic
                        if (buffer[0] != 0xff && buffer[1] != 0xd8 && buffer[2] != 0xff && buffer[3] != 0xe0)
                        {
                            //png magic
                            if (buffer[0] == 137 && buffer[1] == 'P' && buffer[2] == 'N' && buffer[3] == 'G')
                            {
                                Console.WriteLine(file.FullName[pathSize..] + " : PNG");
                            }
                            //webp magic
                            else if (buffer[0] == 'R' && buffer[1] == 'I' && buffer[2] == 'F' && buffer[3] == 'F')
                            {
                                Console.WriteLine(file.FullName[pathSize..] + " : WEBP");
                            }
                            //something else
                            else
                            {
                                Console.WriteLine(file.FullName[pathSize..] + "");
                            }

                            totalSize += file.Length;
                        }
                    }
                }
                catch (Exception)
                {

                    Console.WriteLine("Can't read : " + file.FullName[pathSize..]);
                }
            }

            Console.WriteLine("Total size found : " +  totalSize);
        }
    }
}
