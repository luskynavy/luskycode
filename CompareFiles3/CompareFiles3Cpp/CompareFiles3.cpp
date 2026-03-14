#include <iostream>
#include <filesystem>
#include <vector>
#include <string>
#include <fstream>
#include <ostream>

namespace fs = std::filesystem;

struct Options
{
    std::string srcPath = "";
    std::string dstPath = "";
    std::string socket = "";
    std::string host = "";

    //output file if set
    std::string output = "";

    //prefix for each line in output
    std::string prefixAdd = "copy";
    std::string prefixUpd = "copy /y";
    std::string prefixDel = "recycler";

    bool verbose = false;
};

struct Results
{
    std::vector<std::string> onlyInSource;
    std::vector<std::string> onlyInDestination;
    std::vector<std::string> differentSize;
};

struct FileInfo
{
    std::string path;
    uintmax_t size;
};

/// <summary>
/// Get options from command line
/// </summary>
/// <param name="args"></param>
/// <returns>option found</returns>
static Options manageOptions(int argc, char* argv[])
{
    Options options;
	// Start from 1 to skip program name
    for (int i = 1; i < argc;)
    {
        std::string s = argv[i];

        if (s == "-socket")
        {
            if (i + 1 < argc)
            {
                options.socket = argv[i + 1];
                i++;
            }
        }
        else if (s == "-host")
        {
            if (i + 1 < argc)
            {
                options.host = argv[i + 1];
                i++;
            }
        }
        else if (s == "-output")
        {
            if (i + 1 < argc)
            {
                options.output = argv[i + 1];
                i++;
            }
        }
        else if (s == "-prefixAdd")
        {
            if (i + 1 < argc)
            {
                options.prefixAdd = argv[i + 1];
                i++;
            }
        }
        else if (s == "-prefixUpd")
        {
            if (i + 1 < argc)
            {
                options.prefixUpd = argv[i + 1];
                i++;
            }
        }
        else if (s == "-prefixDel")
        {
            if (i + 1 < argc)
            {
                options.prefixDel = argv[i + 1];
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
            if (options.srcPath.empty())
            {
                options.srcPath = argv[i];
            }
            //second is dstPath
            else if (options.dstPath.empty())
            {
                options.dstPath = argv[i];
            }
        }


        i++;
    }

    if (options.srcPath.ends_with("\\"))
    {
        options.srcPath = options.srcPath.substr(0, options.srcPath.size() - 1);
    }

    if (options.dstPath.ends_with("\\"))
    {
        options.dstPath = options.dstPath.substr(0, options.dstPath.size() - 1);
    }

    return options;
}

/// <summary>
/// Get files in path
/// </summary>
/// <param name="path">path for search</param>
/// <param name="files">vector of file names and size for return</param>
static void getFiles(fs::path path, std::vector<FileInfo>& files)
{
	// Recursively iterate through the directory and collect file paths and sizes
    for (const auto& entry :
            fs::recursive_directory_iterator(path, fs::directory_options::skip_permission_denied))
    {
        try
        {
            if (entry.is_regular_file())
            {
                files.push_back({
                    entry.path().string().substr(path.string().size() + 1),
                    entry.file_size()
                    });
            }
        }
        catch (const fs::filesystem_error& e)
        {
            std::cerr << "Filesystem error: " << e.what() << std::endl;
        }
    }

	// Sort files by path in case-insensitive way
    std::sort(files.begin(), files.end(),
        [](const FileInfo& a, const FileInfo& b) {
            std::string aLower = a.path;
            std::string bLower = b.path;

            std::transform(aLower.begin(), aLower.end(), aLower.begin(),
                [](unsigned char c) { return std::tolower(c); });

            std::transform(bLower.begin(), bLower.end(), bLower.begin(),
                [](unsigned char c) { return std::tolower(c); });
            return aLower < bLower;
        });

    bool displayFiles = false; // Set to false to skip printing file details

    if (displayFiles)
    {
        // Example usage: print collected data
        for (const auto& f : files)
        {
            std::cout << f.path << " - " << f.size << " bytes\n";
        }
    }
}

/// <summary>
/// Find names presents in src or dst or with different size
/// </summary>
/// <param name="filesSrc">source names</param>
/// <param name="filesDst">destination names</param>
/// <param name="results">results</param>
/// <returns></returns>
static void findDstNotPresentOrDifferentSize(std::vector<FileInfo>& filesSrc, std::vector<FileInfo>& filesDst, Results& results)
{
    int indexSrc = 0, indexDst = 0;
    while (indexSrc < filesSrc.size() && indexDst < filesDst.size())
    {
        // Compare names in lowercase to avoid case change
	    std::string pathSrc = filesSrc[indexSrc].path;
		std::string pathDst = filesDst[indexDst].path;

        std::transform(pathSrc.begin(), pathSrc.end(), pathSrc.begin(),
            [](unsigned char c) { return std::tolower(c); });

        std::transform(pathDst.begin(), pathDst.end(), pathDst.begin(),
            [](unsigned char c) { return std::tolower(c); });

        // dstPath == srcPath
        if (pathSrc == pathDst)
        {
			// Compare sizes
            if (filesSrc[indexSrc].size != filesDst[indexDst].size)
            {
                //std::cout << "Different size: " << filesSrc[indexSrc].path << " (src: " << filesSrc[indexSrc].size
                //    << " bytes, dst: " << filesDst[indexDst].size << " bytes)\n";
				results.differentSize.push_back(filesSrc[indexSrc].path);
            }

            // Move two indexes
            ++indexSrc;
            ++indexDst;
        }
        //srcPath > dstPath
        //c.txt > b.txt : destination is not present in source, skip destination
        else if (pathSrc > pathDst)
        {
            //std::cout << "Not present in src: " << filesDst[indexDst].path << "\n";
			results.onlyInDestination.push_back(filesDst[indexDst].path);
            ++indexDst;
        }
        //srcPath < dstPath
        //b.txt < c.txt : source is not present in destination, skip source
        else
        {
            //std::cout << "Not present in dst: " << filesSrc[indexSrc].path << "\n";
			results.onlyInSource.push_back(filesSrc[indexSrc].path);
            ++indexSrc;
        }
    }

    // Remaining in dstPath
    while (indexDst < filesDst.size())
    {
        //std::cout << "Not present in src: " << filesDst[indexDst].path << "\n";
		results.onlyInDestination.push_back(filesDst[indexDst].path);
        ++indexDst;
    }

    // Remaining in srcPath
    while (indexSrc < filesSrc.size())
    {
        //std::cout << "Not present in dst: " << filesSrc[indexSrc].path << "\n";
		results.onlyInSource.push_back(filesSrc[indexSrc].path);
        ++indexSrc;
    }
}

/// <summary>
/// Show the result to console or file if options.output is set
/// </summary>
/// <param name="filesSrc">source names</param>
/// <param name="filesDst">destination names</param>
/// <param name="options">options</param>
static void showResults(std::vector<FileInfo>& filesSrc, std::vector<FileInfo>& filesDst, Options& options)
{
	Results results;
    
	findDstNotPresentOrDifferentSize(filesSrc, filesDst, results);    

    std::ofstream ofs;
    
    if (!options.output.empty())
    {
       ofs = std::ofstream(options.output);       
    }

	// Copy files only in source to destination, create directory if not exist
    for(auto& f : results.onlyInSource)
    {
        std::filesystem::path dstPath(f);
        // Create destination directory it if don't exist
        std::string fullDstPath = options.dstPath + "\\" + dstPath.parent_path().filename().string();
        std::filesystem::path filepath = std::string(fullDstPath);
        if (!std::filesystem::is_directory(filepath))
        {
            // Create drectory only if output file specified
            if (!options.output.empty())
            {
                std::filesystem::create_directories(fullDstPath);
            }
            else
            {
                std::cout << "Created directory: " << fullDstPath << "\n";
            }
        }

		std::string command = options.prefixAdd + " \"" + options.srcPath + "\\" + f + "\" \"" + options.dstPath + "\\" + dstPath.parent_path().filename().string() + "\"" + "\n";
        if (!options.output.empty())
        {
            ofs << command;
        }
        else
        {
            std::cout << command;
        }
    }

	// Update files with different size
    for (auto& f : results.differentSize)
    {
		std::string command = options.prefixUpd + " \"" + options.srcPath + "\\" + f + "\" \"" + options.dstPath + "\\" + f + "\"" + "\n";
        if (!options.output.empty())
        {
            ofs << command;
        }
        else
        {
            std::cout << command;
        }
    }

	/// Delete files only in destination
    for (auto& f : results.onlyInDestination)
    {
		std::string command = options.prefixDel + " \"" + options.dstPath + "\\" + f + "\"" + "\n";
        if (!options.output.empty())
        {
            ofs << command;
        }
        else
        {
            std::cout << command;
        }
    }

    if (!options.output.empty())
    {
        ofs.close();
    }
}

int main(int argc, char* argv[])
{
    if (argc < 2)
    {
        std::cout << "Usage: " << argv[0] << " <source_path> <destination_path>\n";
        std::cout << "No directory specified, using current directory.\n";
	}

    auto options = manageOptions(argc, argv);
    
    fs::path src = (argc > 1) ? argv[1] : ".";
    fs::path dst = (argc > 1) ? argv[2] : ".";

    std::vector<FileInfo> filesSrc;
    std::vector<FileInfo> filesDst;

    std::chrono::steady_clock::time_point begin = std::chrono::steady_clock::now();
	getFiles(src, filesSrc);
    std::chrono::steady_clock::time_point end = std::chrono::steady_clock::now();
    std::cout << "getFiles src " << std::chrono::duration_cast<std::chrono::seconds> (end - begin).count() << "s" << std::endl;

    begin = std::chrono::steady_clock::now();
	getFiles(dst, filesDst);
    end = std::chrono::steady_clock::now();
    std::cout << "getFiles dst " << std::chrono::duration_cast<std::chrono::seconds> (end - begin).count() << "s" << std::endl;

    begin = std::chrono::steady_clock::now();
    showResults(filesSrc, filesDst, options);
    end = std::chrono::steady_clock::now();
    std::cout << "compare " << std::chrono::duration_cast<std::chrono::seconds> (end - begin).count() << "s" << std::endl;

    return 0;
}