#include <iostream>
#include <filesystem>
#include <vector>
#include <string>
#include <fstream>
#include <ostream>

namespace fs = std::filesystem;

/// <summary>
/// Get files in path
/// </summary>
/// <param name="path">path for search</param>
/// <param name="files">vector of file names for return</param>
static void getFiles(fs::path path, std::vector<std::string>& files)
{
    // Recursively iterate through the directory and collect file paths and sizes
    for (const auto& entry :
        fs::recursive_directory_iterator(path, fs::directory_options::skip_permission_denied))
    {
        try
        {
            if (entry.is_regular_file())
            {
                files.push_back(
                    entry.path().string().substr(path.string().size() + 1)
                    );
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

int main(int argc, char* argv[])
{
    fs::path src = (argc > 1) ? argv[1] : ".";

    std::vector<std::string> filesSrc;

    try
    {
        getFiles(src, filesSrc);
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
            fs.open(f, std::ios::in | std::ios::binary);

            // Read the first 8 bytes to check for magic numbers
            unsigned char buffer[8];
            fs.read(reinterpret_cast<char*>(buffer), 8);

            //jpg magic
            if (buffer[0] != 0xff && buffer[1] != 0xd8 && buffer[2] != 0xff && buffer[3] != 0xe0)            
            {
                //png magic
                if (buffer[0] == 137 && buffer[1] == 'P' && buffer[2] == 'N' && buffer[3] == 'G')
                {
                    std::cout << f << " : PNG\n";
                }
                //webp magic
                else if (buffer[0] == 'R' && buffer[1] == 'I' && buffer[2] == 'F' && buffer[3] == 'F')
                {
                    std::cout << f << " : WEBP\n";
                }
                //avif magic
                else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0 && buffer[3] == 24
                    && buffer[4] == 'f' && buffer[5] == 't' && buffer[6] == 'y' && buffer[7] == 'p')
                {
                    std::cout << f << " : AVIF\n";
                }
                //something else
                else
                {
                    std::cout << f << "\n";
                }                
            }

            fs.close();
		}
        catch (const std::exception& e)
        {
            std::cerr << "Error processing file " << f << ": " << e.what() << std::endl;
        }
	}
}

