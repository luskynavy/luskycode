#include <iostream>
#include <filesystem>
#include <vector>
#include <string>
#include <fstream>
#include <ostream>
#include <chrono>

namespace fs = std::filesystem;

/// <summary>
/// Get files in path
/// </summary>
/// <param name="path">path for search</param>
/// <param name="files">vector of file names for return</param>
/// <param name="weeksToKeep">number of weeks to keep files</param>
static void getFiles(fs::path path, std::vector<std::string>& files, int weeksToKeep = 0)
{
    auto now = fs::file_time_type::clock::now();

    // Recursively iterate through the directory and collect file paths
    for (const auto& entry :
        fs::recursive_directory_iterator(path, fs::directory_options::skip_permission_denied))
    {
        try
        {
            if (entry.is_regular_file())
            {
				// Check if the file's last write time is within the specified number of weeks
                if (weeksToKeep == 0 || entry.last_write_time() >= now - std::chrono::weeks(weeksToKeep))
                {
                    files.push_back(
                        entry.path().string().substr(path.string().size() + 1)
                    );
                }
            }
        }
        catch (const fs::filesystem_error& e)
        {
            std::cerr << "Filesystem error: " << e.what() << std::endl;
        }
    }

    // Sort files by path in case-insensitive way
    std::sort(files.begin(), files.end(),
        [](const std::string& a, const std::string& b) {
            std::string aLower = a;
            std::string bLower = b;

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
            std::cout << f << "\n";
        }
    }
}

/// <summary>
/// Display file format based on magic numbers
/// </summary>
/// <param name="buffer">Buffer containing the first 8 bytes of the file</param>
/// <param name="filename">Name of the file</param>
static void DisplayFileFormat(unsigned char  buffer[8], const std::string& filename)
{
    //jpg magic
    if (buffer[0] != 0xff && buffer[1] != 0xd8 && buffer[2] != 0xff && buffer[3] != 0xe0)
    {
        //png magic
        if (buffer[0] == 137 && buffer[1] == 'P' && buffer[2] == 'N' && buffer[3] == 'G')
        {
            std::cout << filename << " : PNG\n";
        }
        //webp magic
        else if (buffer[0] == 'R' && buffer[1] == 'I' && buffer[2] == 'F' && buffer[3] == 'F')
        {
            std::cout << filename << " : WEBP\n";
        }
        //avif magic
        else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0 && buffer[3] == 24
            && buffer[4] == 'f' && buffer[5] == 't' && buffer[6] == 'y' && buffer[7] == 'p')
        {
            std::cout << filename << " : AVIF\n";
        }
        //something else
        else
        {
            std::cout << filename << "\n";
        }
    }
}

int main(int argc, char* argv[])
{
    std::string firstArg = (argc > 1) ? argv[1] : ".";
	if (!fs::exists(firstArg) || !fs::is_directory(firstArg))
	{
		std::cerr << "Invalid directory: " << firstArg << std::endl;
		return 1;
	}
    if (firstArg.ends_with(fs::path::preferred_separator))
    {
        firstArg.pop_back();
    }
    fs::path src = firstArg;

    int weeksToKeep = (argc > 2) ? std::stoi(argv[2]) : 0; 

    std::vector<std::string> filesSrc;

    // Recording the timestamp at the start of the code
    auto beg = std::chrono::high_resolution_clock::now();

    try
    {
        getFiles(src, filesSrc, weeksToKeep);
    }
    catch (const std::exception& e)
    {
        std::cerr << "Error getting files: " << e.what() << std::endl;
    }

	for (const auto& f : filesSrc)
	{
        try
        {
            // Open the file read only in binary
            std::fstream fs;
			std::string fullPath = (src / f).string();
            fs.open(fullPath, std::ios::in | std::ios::binary);

            // Read the first 8 bytes to check for magic numbers
            unsigned char buffer[8];
            fs.read(reinterpret_cast<char*>(buffer), 8);

            DisplayFileFormat(buffer, f);

            fs.close();
		}
        catch (const std::exception& e)
        {
            std::cerr << "Error processing file " << f << ": " << e.what() << std::endl;
        }
	}

    // Taking a timestamp after the code is ran
    auto end = std::chrono::high_resolution_clock::now();

    auto duration = duration_cast<std::chrono::milliseconds>(end - beg);

    std::cout << "Done in: " << (double)duration.count() / 1000 << " s" << std::endl;

}

