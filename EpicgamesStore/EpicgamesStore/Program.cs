using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EpicgamesStore
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			//same as grep -oP 'data-tip="([^"]*)"' "EpicGames Purchase History.html"  | sort

			string text = File.ReadAllText("EpicGames Purchase History.html");
			Regex rx = new Regex("data-tip=\"([^\"]*)\"");

			// Find matches.
			MatchCollection matches = rx.Matches(text);

			// Report the number of matches found.
			/*Console.WriteLine("{0} matches found in:\n   {1}",
							  matches.Count,
							  text);*/

			List<string> sorted = new List<string>();

			// Report on each match.
			foreach (Match match in matches)
			{
				GroupCollection groups = match.Groups;
				//Console.WriteLine(match.Groups[1]);
				sorted.Add(match.Groups[1].Value);
			}

			sorted.Sort();

			foreach (string s in sorted)
			{
				Console.WriteLine(s);
			}
		}
	}
}