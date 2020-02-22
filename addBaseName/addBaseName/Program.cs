using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace addBaseName
{
    class Program
    {
        static void Main(string[] args)
        {
            string pwd = Path.GetFileName(Environment.CurrentDirectory);

            DirectoryInfo d = new DirectoryInfo(@"."); //in current folder

            //order descending by name length to avoid name override
            //so shorter name will not override longer name starting with folder
            IOrderedEnumerable<FileInfo> orderedEnumerable = d.GetFiles("*.*").OrderByDescending(f => f.Name.Length );
            //FileInfo[] Files = orderedEnumerable; //Getting files

            foreach (FileInfo file in orderedEnumerable)
            {
                Console.WriteLine(file.Name + " => " + pwd + file.Name);
                //System.IO.File.Copy(file.Name, pwd + file.Name);
                System.IO.File.Move(file.Name, pwd + " " + file.Name);
            }
        }
    }
}
