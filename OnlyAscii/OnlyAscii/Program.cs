using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlyAscii
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			string pwd = Path.GetFileName(Environment.CurrentDirectory);

			DirectoryInfo d = new DirectoryInfo(@"."); //in current folder

			//order descending by name length to avoid name override
			//so shorter name will not override longer name starting with folder
			IOrderedEnumerable<FileInfo> orderedEnumerable = d.GetFiles("*.*").OrderByDescending(f => f.Name.Length);
			//FileInfo[] Files = orderedEnumerable; //Getting files

			foreach (FileInfo file in orderedEnumerable)
			{
				string newName = Regex.Replace(file.Name, @"[^\u0000-\u007F]+", string.Empty);

				//only if newname is different
				if (file.Name != newName)
				{
					Console.WriteLine(file.Name + " => " + file.Name);
					//remove non ascii chars
					System.IO.File.Move(file.Name, newName);
				}
			}
		}
	}
}