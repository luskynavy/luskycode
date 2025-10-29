using System.Collections;
using System.Resources;
using System.Resources.Extensions;

namespace ResourceReaderTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> resourcesFiles = [
                @".\ResourceReaderTest.dll",
                @".\en\ResourceReaderTest.resources.dll",
                @".\fr\ResourceReaderTest.resources.dll",
                @".\de\ResourceReaderTest.resources.dll",
                @".\ru\ResourceReaderTest.resources.dll"
            ];

            foreach (string file in resourcesFiles)
            {
                Console.WriteLine(file + ":");
                var assem = System.Reflection.Assembly.LoadFrom(file);
                foreach (var resourceName in assem.GetManifestResourceNames())
                {
                    Console.WriteLine(resourceName + ":");
                    var stream = assem.GetManifestResourceStream(resourceName);

                    //ok
                    //var reader = new System.Resources.Extensions.DeserializingResourceReader(stream);

                    //ok too and don't need System.Resources.Extensions.dll
                    var reader = new ResourceReader(stream);
                    IDictionaryEnumerator dict = reader.GetEnumerator();
                    int ctr = 0;

                    while (dict.MoveNext())
                    {
                        ctr++;
                        Console.WriteLine("{0:00}: {1} = {2}", ctr, dict.Key, dict.Value);
                    }
                }

                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
