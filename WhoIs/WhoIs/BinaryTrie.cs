// Copyright © 2003 by Jeffrey Sax
// All rights reserved.
// http://www.extremeoptimization.com/
// Filename: BinaryTrie.cs
// Description: Classes to represent a binary trie structure
// Last modified: May 28, 2003.

namespace WhoIs
{
	using System;

	/// <summary>
	/// Represents a trie with keys that are binary values of
	/// length up to 32.
	/// </summary>
	public class BinaryTrie
	{
		internal BinaryTrieNode[] _roots;	// Roots of the trie
		private Int32 _indexLength = 0;
		private Int32 _count = 0;	// Number of entries in the trie

		#region Public instance constructors
		/// <summary>
		/// Constructs a <see cref="BinaryTrie"/> with an index length
		/// of 1.
		/// </summary>
		public BinaryTrie()
		{
			_indexLength = 1;
			_roots = new BinaryTrieNode[2];
		}
		/// <summary>
		/// Constructs a <see cref="BinaryTrie"/> with a given index length.
		/// </summary>
		/// <param name="indexLength">The index length.</param>
		public BinaryTrie(Int32 indexLength)
		{
			if ((indexLength < 1) || (indexLength > 18))
				throw new ArgumentOutOfRangeException("indexLength");
			_indexLength = indexLength;
			_roots = new BinaryTrieNode[1 << indexLength];
		}
		#endregion

		#region Protected instance members
		/// <summary>
		/// Gets the collection of root <see cref="BinaryTrieNode"/>
		/// objects in this <see cref="BinaryTrie"/>.
		/// </summary>
		protected BinaryTrieNode[] Roots
		{
			get { return _roots; }
		}
		/// <summary>
		/// Gets or sets the number of keys in the trie.
		/// </summary>
		protected Int32 CountInternal
		{
			get { return _count; }
			set { _count = value; }
		}

		/// <summary>
		/// Adds a key with the given index to the trie.
		/// </summary>
		/// <param name="index">The index of the root <see cref="BinaryTrieNode"/>
		/// for the given key value.</param>
		/// <param name="key">An <see cref="Int32"/> key value.</param>
		/// <param name="keyLength">The length in bits of the significant
		/// portion of the key.</param>
		/// <returns>The <see cref="BinaryTrieNode"/> that was added to the 
		/// trie.</returns></returns>
		protected BinaryTrieNode AddInternal(Int32 index, Int32 key, Int32 keyLength)
		{
			CountInternal++;
			BinaryTrieNode root = Roots[index];
			if (null == root)
				// Create the new root.
				return _roots[index] = new BinaryTrieNode(key, keyLength);
			else
				// Add the record to the trie.
				return root.AddInternal(key, keyLength);
		}

		protected Object FindBestMatchInternal(Int32 index, Int32 key)
		{
			BinaryTrieNode root = _roots[index];
			if (null == root)
				return null;
			return root.FindBestMatch(key).UserData;
		}
		protected Object FindExactMatchInternal(Int32 index, Int32 key)
		{
			BinaryTrieNode root = _roots[index];
			if (null == root)
				return null;
			return root.FindExactMatch(key).UserData;
		}
		#endregion

		#region Public instance properties
		/// <summary>
		/// Gets the index length of this <see cref="BinaryTrie"/>.
		/// </summary>
		/// <remarks>The index length indicates the number of bits
		/// that is to be used to preselect the root nodes.
		/// </remarks>
		public Int32 IndexLength
		{
			get { return _indexLength; }
		}
		/// <summary>
		/// Gets the number of keys in the trie.
		/// </summary>
		public Int32 Count
		{
			get { return _count; }
		}
		#endregion

		#region Public instance methods
		/// <summary>
		/// Adds a node to the trie.
		/// </summary>
		/// <param name="key">An <see cref="Int32"/> key value.</param>
		/// <param name="keyLength">The length in bits of the significant
		/// portion of the key.</param>
		/// <returns>The <see cref="BinaryTrieNode"/> that was added to the 
		/// trie.</returns></returns>
		public BinaryTrieNode Add(Int32 key, Int32 keyLength)
		{
			Int32 index = (Int32)(key >> (32 - _indexLength));
			return AddInternal(index, key, keyLength);
		}

		public Object FindBestMatch(Int32 key)
		{
			Int32 index = (Int32)(key >> (32 - _indexLength));
			return FindBestMatchInternal(index, key);
		}
		#endregion

	}

	/// <summary>
	/// Represents an entry in an <see cref="IPCountryLookup"/> table.
	/// </summary>
	public class BinaryTrieNode
	{
		protected static readonly Object EmptyData = new Object();

		private static Int32[] _bit
			= {0x7FFFFFFF, 0x7FFFFFFF,0x40000000,0x20000000,0x10000000,
				  0x8000000,0x4000000,0x2000000,0x1000000,
				  0x800000,0x400000,0x200000,0x100000,
				  0x80000,0x40000,0x20000,0x10000,
				  0x8000,0x4000,0x2000,0x1000,
				  0x800,0x400,0x200,0x100,
				  0x80,0x40,0x20,0x10,
				  0x8,0x4,0x2,0x1,0};

		private Int32 _key;		// Key value
		private Int32 _keyLength;	// Length of the key
		private BinaryTrieNode _zero = null;	// First child
		private BinaryTrieNode _one = null;	// Second child
		private Object _userData;

		#region Public instance properties
		/// <summary>
		/// Gets or sets the country code for this entry.
		/// </summary>
		public Object UserData
		{
			get
			{
				if (IsKey)
					return _userData; 
				else
					return null;
			}
			set { _userData = value; }
		}
		public Int32 Key
		{
			get { return _key; }
		}
		public Boolean IsKey
		{
			get { return (!Object.ReferenceEquals(_userData, EmptyData)); }
		}
		#endregion

		#region Internal instance members
		/// <summary>
		/// Constructs an <see cref="BinaryTrieNode"/> object.
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="keyLength">Length of the key</param>
		internal BinaryTrieNode(Int32 key, Int32 keyLength)
		{
			_key = key;
			_keyLength = keyLength;
			_userData = EmptyData;
		}

		/// <summary>
		/// Adds a record to the trie using the internal representation
		/// of an IP address.
		/// </summary>
		internal BinaryTrieNode AddInternal(Int32 key, Int32 keyLength)
		{
			// Find the common key keyLength
			Int32 difference = key ^ _key;
			// We are only interested in matches up to the keyLength...
			Int32 commonKeyLength = Math.Min(_keyLength, keyLength);
			// ...so count down from there.
			while (difference >= _bit[commonKeyLength])
				commonKeyLength--;

			// If the new key length is smaller than the common key length, 
			// or equal but smaller than the current key length,
			// the new key should be the parent of the current node.
			if ((keyLength < commonKeyLength)
				|| ((keyLength == commonKeyLength) && (keyLength < _keyLength)))
			{
				// Make a copy that will be the child node.
				BinaryTrieNode copy = (BinaryTrieNode)this.MemberwiseClone(); // new BinaryTrieNode(this);
				// Fill in the child references based on the first
				// bit after the common key.
				if ((_key & _bit[keyLength+1]) != 0)
				{
					_zero = null;
					_one = copy;
				}
				else
				{
					_zero = copy;
					_one = null;
				}
				_key = key;
				_keyLength = keyLength;
				UserData = EmptyData;
				return this;
			}

			// Do we have a complete match?
			if (commonKeyLength == _keyLength)
			{
				if (keyLength == _keyLength)
					return this;

				// Yes. Add the key as a child.
				if ((key & _bit[_keyLength+1]) == 0)
				{
					// The remainder of the key starts with a zero.
					// Do we have a child in this position?
					if (null == _zero)
						// No. Create one.
						return _zero = new BinaryTrieNode(key, keyLength);
					else
						// Yes. Add this key to the child.
						return _zero.AddInternal(key, keyLength);
				}
				else
				{
					// The remainder of the key starts with a one.
					// Do we have a child in this position?
					if (null == _one)
						// No. Create one.
						return _one = new BinaryTrieNode(key, keyLength);
					else
						// Yes. Add this key to the child.
						return _one.AddInternal(key, keyLength);
				}
			}
			else
			{
				// No. The match is only partial, so split this node.
				// Make a copy that will be the first child node.
				BinaryTrieNode copy = (BinaryTrieNode)this.MemberwiseClone(); // new BinaryTrieNode(this);
				// And create the other child node.
				BinaryTrieNode newEntry = new BinaryTrieNode(key, keyLength);
				// Fill in the child references based on the first
				// bit after the common key.
				if ((_key & _bit[commonKeyLength+1]) != 0)
				{
					_zero = newEntry;
					_one = copy;
				}
				else
				{
					_zero = copy;
					_one = newEntry;
				}
				_keyLength = commonKeyLength;
				return newEntry;
			}
		}
		#endregion

		#region Public instance members
		public BinaryTrieNode FindExactMatch(Int32 key)
		{
			if ((key ^ _key) == 0)
				return this;
			
			// Pick the child to investigate.
			if ((key & _bit[_keyLength+1]) == 0)
			{
				// If the key matches the child's key, pass on the request.
				if (null != _zero)
				{
					if ((key ^ _zero._key) < _bit[_zero._keyLength])
						return _zero.FindExactMatch(key);
				}
			}
			else
			{
				// If the key matches the child's key, pass on the request.
				if (null != _one)
				{
					if ((key ^ _one._key) < _bit[_one._keyLength])
						return _one.FindExactMatch(key);
				}
			}
			// If we got here, neither child was a match, so the current
			// node is the best match.
			return null;
		}
		/// <summary>
		/// Looks up a key value in the trie.
		/// </summary>
		/// <param name="key">The key to look up.</param>
		/// <returns>The best matching <see cref="BinaryTrieNode"/>
		/// in the trie.</returns>
		public BinaryTrieNode FindBestMatch(Int32 key)
		{
			// Pick the child to investigate.
			if ((key & _bit[_keyLength+1]) == 0)
			{
				// If the key matches the child's key, pass on the request.
				if (null != _zero)
				{
					if ((key ^ _zero._key) < _bit[_zero._keyLength])
						return _zero.FindBestMatch(key);
				}
			}
			else
			{
				// If the key matches the child's key, pass on the request.
				if (null != _one)
				{
					if ((key ^ _one._key) < _bit[_one._keyLength])
						return _one.FindBestMatch(key);
				}
			}
			// If we got here, neither child was a match, so the current
			// node is the best match.
			return this;
		}
		#endregion
	}
}