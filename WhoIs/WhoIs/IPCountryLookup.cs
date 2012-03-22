// Copyright © 2003 by Jeffrey Sax
// All rights reserved.
// http://www.extremeoptimization.com/
// Filename: IPCountryLookup.cs
// First version: April 18, 2003.
// Changes:
//   May 16, 2003. Replaced GetKeyLength
//   May 24, 2003. Extracted binary trie code. (see BinaryTrie.cs)
//   May 24, 2003. Expanded root of trie to 256 nodes.
//	 May 26, 2003. Inlined 'Match' function.
//	 May 26, 2003. Cleaned up interface.
//	 May 27, 2003. Expanded root of trie to user-supplied number of nodes.

using System;
using System.Collections;
using System.IO;
using System.Net;
//using Extreme;

namespace WhoIs
{
	/// <summary>
	/// Represents a trie that can be used to look up the country
	/// corresponding to an IP address.
	/// </summary>
	public class IPCountryTable : BinaryTrie
	{
		private Int32 _extraNodes = 0;

		static protected Int32 GetKeyLength(Int32 length)
		{
			if (length < 0)
				return 1;
			Int32 keyLength = 33;
			while (length != 0)
			{
				length >>= 1;
				keyLength--;
			}
			return keyLength;
		}

		private Int32 _indexOffset; // Number of bits after index part.
		/// <summary>
		/// Constructs an <see cref="IPCountryTable3"/> object.
		/// </summary>
		public IPCountryTable(Int32 indexLength) : base(indexLength)
		{
			_indexOffset = 32 - indexLength;
		}

		/// <summary>
		/// Loads an IP-country database file into the trie.
		/// </summary>
		/// <param name="filename">The path and filename of the file
		/// that holds the database.</param>
		/// <param name="calculateKeyLength">A boolean value that
		/// indicates whether the <em>size</em> field in the database
		/// contains the total length (<strong>true</strong>) or the 
		/// exponent of the length (<strong>false</strong> of the
		/// allocated segment.</param>
		public void LoadStatisticsFile(String filename, Boolean calculateKeyLength)
		{
			StreamReader reader = new StreamReader(filename);
			try 
			{
				String record;
				while (null != (record = reader.ReadLine()))
				{
					String[] fields = record.Split('|');

					// Skip if not the right number of fields
					if (fields.Length != 7)
						continue;
					// Skip if not an IPv4 record
					if (fields[2] != "ipv4")
						continue;
					// Skip if header or info line
					if (fields[1] == "*")
						continue;

					String ip = fields[3];

					Int32 length = Int32.Parse(fields[4]);
					Int32 keyLength;

					// Convert number of available IP's to key length
					if (calculateKeyLength)	
						keyLength = GetKeyLength(length);
					else
						keyLength = (Int32)length;

					// Interning the country strings saves us a little bit of memory.
					String countryCode = String.Intern(fields[1]);

					String [] parts = ip.Split('.');

					// The first IndexLength bits of the IP address get
					// to be the index into our table of roots.
					Int32 indexBase = ((Int32.Parse(parts[0]) << 8)
						+ Int32.Parse(parts[1]));
					Int32 keyBase = (indexBase << 16)
						+ (Int32.Parse(parts[2]) << 8)
						+ Int32.Parse(parts[3]);
					indexBase >>= (_indexOffset - 16);

					// If the keyLength is less than our IndexLength,
					// the current record spans multiple root nodes.
					Int32 count = (1 << (IndexLength - Math.Min(keyLength, IndexLength)));

					// The key length should be at least the IndexLength.
					keyLength = Math.Max(keyLength, IndexLength);

					for(Int32 index = 0; index < count; index++)
					{
						// keyBase already contains the indexBase part,
						// so just add the shifted index.
						Int32 key = (index << _indexOffset) + keyBase;
						base.AddInternal(indexBase + index, key, keyLength).UserData = countryCode;
					}
					// We want the count to reflect the actual number of 
					// networks, so remove the duplicates from the count.
					_extraNodes += count - 1;
				}
			} 
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Gets the total number of entries in the trie.
		/// </summary>
		public Int32 NetworkCodeCount
		{
			get { return base.Count - _extraNodes; }
		}
		/// <summary>
		/// Attempts to find the country code corresponding to
		/// a given IP address.
		/// </summary>
		/// <param name="address">A <see cref="String"/> value
		/// representing the </param>
		/// <returns>The two letter country code corresponding to
		/// the IP address, or <strong>"??"</strong> if it was not 
		/// found.</returns>
		public String GetCountry(String address)
		{
			String [] parts = address.Split('.');

			// The first IndexLength bits form the key into the
			// array of root nodes.
			Int32 indexBase = ((Int32.Parse(parts[0]) << 8)
				+ Int32.Parse(parts[1]));
			Int32 index = indexBase >> (_indexOffset - 16);

			BinaryTrieNode root = base.Roots[index];
			// If we don't have a root, we don't have a value.
			if (null == root)
				return null;

			// Calculate the full key...
			Int32 key = (indexBase << 16)
				+ (Int32.Parse(parts[2]) << 8)
				+ Int32.Parse(parts[3]);
			// ...and look it up.
			return (String)root.FindBestMatch(key).UserData;
		}
	}
}