using System;
using System.Drawing;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;

/*************************************************************************************
 * 
 *			Class       : Configuration
 *			Date        : 2004
 *			Author      : Bidou
 *			Version		: 1.0.0
 *			Comment		: Aucun
 * 
 *  Permet de serializer certaines classes, contenant les informations relatives
 *  à la sauvegardes des configurations et aux informations concernant l'utilisateur.
*************************************************************************************/

namespace Demineur
{
	///*************************************************************************************
	/// <summary>
	/// Stock les options que l'utilisateur peut sauvegarder.
	/// </summary>
	///*************************************************************************************
	public class Options
	{
		[XmlElement("GameSize")]
		public Size gameSize = new Size(10, 10);
		[XmlElement("Bombs")]
		public int bombs = 12;
		[XmlElement("Level")]
		public Level level = Level.Beginner;
	}
    
	////*************************************************************************************
	/// <summary>
	/// Permet de serializer/deserializer
	/// </summary>
	///*************************************************************************************
	public class Configuration
	{
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Créer le fichier de configuration, on serialize Options.
		/// </summary>
		/// <param name="fileName"> Le fichier dans lequel on écrit. </param>
		/// <param name="o"> Les options à serializer. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public static void Serialize(string fileName, Options o)
		{	
			XmlSerializer serializer = new XmlSerializer(typeof(Options));
			TextWriter writer = new StreamWriter(fileName);
			serializer.Serialize(writer, o);
			writer.Close();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		///  Lit le fichier de configuration, on deserialize Options.
		/// </summary>
		/// <param name="fileName"> Le fichier où on lit. </param>
		/// <returns> Les options de l'utilisateur. </returns>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public static Options Deserialize(string fileName)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Options));
			FileStream fs = new FileStream(fileName, FileMode.Open);
			Options o = (Options)serializer.Deserialize(fs);
			fs.Close();
			return o;
		}
	}

	#region Class User

	////*************************************************************************************
	/// <summary>
	/// A user.
	/// </summary>
	///*************************************************************************************
	[Serializable]
	public class User
	{
		private string _name = string.Empty;
		private int _time = 0;

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set the user name.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set the user time.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public int Time
		{
			get { return this._time; }
			set { this._time = value; }
		}
	}

	#endregion

	#region Class BestScore

	////*************************************************************************************
	/// <summary>
	/// Use to save the best score.
	/// </summary>
	///*************************************************************************************
	public class BestScore
	{
		/// ******************************************************************
		/// <summary>
		/// Deserialize a binary file.
		/// </summary>
		/// <param name="fileName"> The filename to deserialize. </param>
		/// <returns> The high scores founded in the file. </returns>
		/// ******************************************************************
		public static Hashtable Load(string fileName)
		{
			FileStream fs = null;
			Hashtable bestScore = null;
			
			try // Try to deserialize....
			{
				fs = new FileStream(fileName, FileMode.Open);
				BinaryFormatter binF = new BinaryFormatter();
				bestScore = (Hashtable)binF.Deserialize(fs);
			}
			catch(Exception)
			{
				bestScore = new Hashtable(); // An error occured!
			}
			finally
			{
				if(fs != null) fs.Close(); // Close the stream
			}
			return bestScore;
		}
		
		/// ******************************************************************
		/// <summary>
		/// Save the high scores in a file using BinaryFormatter.
		/// </summary>
		/// <param name="fileName"> The location where to save the file. </param>
		/// <param name="h"> The Hashtable to save. </param>
		/// ******************************************************************
		public static void Save(string fileName, Hashtable h)
		{
			FileStream fs = null;
			
			try
			{
				fs = new FileStream(fileName, FileMode.Create);
				BinaryFormatter binF = new BinaryFormatter();
				binF.Serialize(fs, h);
			}
			catch(Exception) 
			{
				throw new Exception("Error while saving...");
			}
			finally
			{
				if(fs != null) fs.Close();
			}
		}
	}

	#endregion
}