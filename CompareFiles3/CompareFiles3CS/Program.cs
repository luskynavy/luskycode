using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;


internal class Options
{
    public string srcPath = "";
    public string dstPath = "";
    public string socket = "";
    public string host = "";

    //output file if set
    public string output = "";

    //prefix for each line in output
    public string prefixAdd = "copy";
    public string prefixUpd = "copy /y";
    public string prefixDel = "recycler";

    public bool verbose = false;
}

internal class Results
{
    public List<string> onlyInSource = [];
    public List<string> onlyInDestination = [];
    public List<string> differentSize = [];
}

class FileInfoEntry
{
    public string Path { get; set; }
    public long Size { get; set; }
}

class Program
{
    //InvariantCulture ?  Ordinal ? InvariantCultureIgnore ? OrdinalIgnoreCase ?
    private static readonly StringComparer _stringComparer = StringComparer.InvariantCulture;

    private const StringComparison _stringComparison = StringComparison.InvariantCulture;

    /// <summary>
    /// Get options from command line
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private static Options ManageOptions(string[] args)
    {
        Options options = new Options();
        for (int i = 0; i < args.Length;)
        {
            string s = args[i];

            if (s == "-socket")
            {
                if (i + 1 < args.Length)
                {
                    options.socket = args[i + 1];
                    i++;
                }
            }
            else if (s == "-host")
            {
                if (i + 1 < args.Length)
                {
                    options.host = args[i + 1];
                    i++;
                }
            }
            else if (s == "-output")
            {
                if (i + 1 < args.Length)
                {
                    options.output = args[i + 1];
                    i++;
                }
            }
            else if (s == "-prefixAdd")
            {
                if (i + 1 < args.Length)
                {
                    options.prefixAdd = args[i + 1];
                    i++;
                }
            }
            else if (s == "-prefixUpd")
            {
                if (i + 1 < args.Length)
                {
                    options.prefixUpd = args[i + 1];
                    i++;
                }
            }
            else if (s == "-prefixDel")
            {
                if (i + 1 < args.Length)
                {
                    options.prefixDel = args[i + 1];
                    i++;
                }
            }
            else if (s == "-verbose")
            {
                options.verbose = true;
            }
            else
            {
                //first path is srcPath
                if (string.IsNullOrEmpty(options.srcPath))
                {
                    options.srcPath = args[i];
                }
                //second is dstPath
                else if (string.IsNullOrEmpty(options.dstPath))
                {
                    options.dstPath = args[i];
                }
            }


            i++;
        }

        if (options.srcPath.EndsWith("\\"))
        {
            options.srcPath = options.srcPath.Substring(0, options.srcPath.Length - 1);
        }

        if (options.dstPath.EndsWith("\\"))
        {
            options.dstPath = options.dstPath.Substring(0, options.dstPath.Length - 1);
        }

        return options;
    }

    /// <summary>
    /// Compare directories locally
    /// </summary>
    /// <param name="options">options with srcPath and dstPath</param>
    private static void LocalMode(Options options)
    {
        if (!string.IsNullOrEmpty(options.output))
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " : Start");
        }

        var filesSrc = GetFiles(Path.GetFullPath(options.srcPath));
        // Log only if output file specified
        if (!string.IsNullOrEmpty(options.output))
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " : GetFiles src done");
        }
        var filesDst = GetFiles(Path.GetFullPath(options.dstPath));
        // Log only if output file specified
        if (!string.IsNullOrEmpty(options.output))
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " : GetFiles dst done");
        }

        ShowResults(filesSrc, filesDst, options);

        // Log only if output file specified
        if (!string.IsNullOrEmpty(options.output))
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " : Done");
        }
    }

    /// <summary>
    /// Show the result to console or file if options.output is set
    /// </summary>
    /// <param name="filesSrc">source names</param>
    /// <param name="filesDst">destination names</param>
    /// <param name="options">options</param>
    private static void ShowResults(FileInfoEntry[] filesSrc, FileInfoEntry[] filesDst, Options options)
    {
        TextWriter writer;
        bool writeToFile = !string.IsNullOrEmpty(options.output);

        if (writeToFile)
        {
            writer = new StreamWriter(options.output);
        }
        else
        {
            writer = Console.Out;
        }

        if (options.verbose)
        {
            //show srcPath
            writer.WriteLine("src:");
            foreach (var f in filesSrc)
            {
                writer.WriteLine(f);
            }

            writer.WriteLine();

            //show dstPath
            writer.WriteLine("dst:");
            foreach (var f in filesDst)
            {
                writer.WriteLine(f);
            }

            writer.WriteLine();
        }

        Results results = FindDstNotPresentOrDifferentSize(filesSrc, filesDst, options);
        if (options.verbose)
        {
            //show result
            writer.WriteLine("diff:");
        }

        int lineWritten = 0;

        foreach (var f in results.onlyInSource)
        {
            // Create destination directory it if don't exist
            string fullDstPath = options.dstPath + "\\" + Path.GetDirectoryName(f);
            if (!Path.Exists(fullDstPath))
            {
                // Create drectory only if output file specified
                if (!string.IsNullOrEmpty(options.output))
                {
                    Directory.CreateDirectory(fullDstPath);
                }
                else
                {
                    Console.WriteLine("Created directory: " + fullDstPath);
                }
            }
            writer.WriteLine($"{options.prefixAdd} \"{options.srcPath}\\{f}\" \"{options.dstPath}\\{Path.GetDirectoryName(f)}\"");

            lineWritten = FlushIfNeeded(writer, lineWritten);
        }

        foreach (var f in results.differentSize)
        {
            writer.WriteLine($"{options.prefixUpd} \"{options.srcPath}\\{f}\" \"{options.dstPath}\\{f}\"");

            lineWritten = FlushIfNeeded(writer, lineWritten);
        }

        foreach (var f in results.onlyInDestination)
        {
            writer.WriteLine($"{options.prefixDel} \"{options.dstPath}\\{f}\"");

            lineWritten = FlushIfNeeded(writer, lineWritten);
        }

        if (writeToFile)
        {
            writer.Flush();
            writer.Close();
        }

        static int FlushIfNeeded(TextWriter writer, int lineWritten)
        {
            //flush file every 10 lines
            lineWritten++;
            if (lineWritten % 10 == 0)
            {
                writer.Flush();
            }

            return lineWritten;
        }
    }

    /// <summary>
    /// Get files in path
    /// </summary>
    /// <param name="path"></param>
    /// <returns>array of file names</returns>
    private static FileInfoEntry[] GetFiles(string path)
    {
        var dir = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);

        Console.WriteLine(DateTime.Now.ToLongTimeString() + " : EnumerateFiles done");

        FileInfoEntry[] files = new FileInfoEntry[dir.Count()];       

        int i = 0;
        foreach (string file in dir)
        {
            FileInfo info = new FileInfo(file);

            files[i] = new FileInfoEntry
            {
                Path = info.FullName.Substring(path.Length + 1),
                Size = info.Length
            };
            i++;
        }

        Console.WriteLine(DateTime.Now.ToLongTimeString() + " : Array Fill done");

        Array.Sort(files, (x, y) => _stringComparer.Compare(x.Path, y.Path));

        Console.WriteLine(DateTime.Now.ToLongTimeString() + " : Array Sort done");

        return files;
    }

    /// <summary>
    /// Find names presents in src or dst or with different size
    /// </summary>
    /// <param name="filesSrc">source names</param>
    /// <param name="filesDst">destination names</param>
    /// <returns></returns>
    private static Results FindDstNotPresentOrDifferentSize(FileInfoEntry[] filesSrc, FileInfoEntry[] filesDst, Options options)
    {
        Results results = new();

        int indexSrc = 0, indexDst = 0;

        while (indexSrc < filesSrc.Length && indexDst < filesDst.Length)
        {
            //compare names in lowercase to avoid case change
            int compareOrdinal = String.Compare(filesDst[indexDst].Path.ToLower(), filesSrc[indexSrc].Path.ToLower(), _stringComparison);

            //dstPath == srcPath
            if (compareOrdinal == 0)
            {
                long lengthSrc = filesSrc[indexSrc].Size;
                long lengthDst = filesDst[indexDst].Size;

                if (lengthSrc != lengthDst)
                {
                    results.differentSize.Add(filesSrc[indexSrc].Path);
                }

                //move two indexes
                indexSrc++;
                indexDst++;

            }
            //srcPath > dstPath
            //c.txt > b.txt : destination is not present in source, skip destination
            else if (compareOrdinal < 0)
            {
                results.onlyInDestination.Add(filesDst[indexDst].Path);
                indexDst++;
            }
            //srcPath < dstPath
            //b.txt < c.txt : source is not present in destination, skip source
            else
            {
                results.onlyInSource.Add(filesSrc[indexSrc].Path);
                indexSrc++;
            }
        }

        //remaining in dstPath
        for (; indexDst < filesDst.Length; indexDst++)
        {
            results.onlyInDestination.Add(filesDst[indexDst].Path);
        }

        //remaining in srcPath
        for (; indexSrc < filesSrc.Length; indexSrc++)
        {
            results.onlyInSource.Add(filesSrc[indexSrc].Path);
        }

        return results;
    }

    static void Main(string[] args)
    {
        Stopwatch sw = Stopwatch.StartNew();

        if (args.Length < 2)
        {
            Console.WriteLine("Usage : CompareFiles2 sourcePath destinationPath [-output filename] [-prefixAdd prefixCommand] [-prefixUpd prefixCommand] [-prefixDel prefixCommand]");
            return;
        }

        Options options = ManageOptions(args);

        LocalMode(options);
        
        Console.WriteLine($"Time elapsed: {sw.ElapsedMilliseconds} ms");

        TimeSpan ts = sw.Elapsed;

        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("RunTime " + elapsedTime);
    }
}