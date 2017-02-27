using System;
using System.Drawing;
using System.Reflection;

/*************************************************************************************
 * 
 *			Class       : Tools
 *			Date        : March 2005
 *			Author      : Bidou
 *			Version		: 1.0.0
 *			Comment		: Only static properties
 * 
*************************************************************************************/

namespace Demineur
{
	///*************************************************************************************
	/// <summary>
	/// Utilities class.
	/// </summary>
	///*************************************************************************************
	public class Tools
	{
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Retrieves a specified image from the current assembly.
		/// </summary>
		/// <param name="iconName"> The image to load. </param>
		/// <param name="dir"> The directory where the image must be loaded. </param>
		/// <returns> The loaded image. </returns>
		/// <remarks> To get an image that is embedded, use the following syntax :
		/// Namespace.directoryName.ImageName
		/// </remarks>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public static Image GetAssemblyImage(string dir, string imageName)
		{
			Assembly a = Assembly.GetExecutingAssembly();
			return Image.FromStream((a.GetManifestResourceStream("Demineur." + dir + "." + imageName)));
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Return the right color depending of the number of bombs.
		/// </summary>
		/// <param name="bombs"> The number of bombs. </param>
		/// <returns> The right color. </returns>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public static Color GetColor(int bombs)
		{
			switch(bombs)
			{
				case 1: return Color.Blue;
				case 2: return Color.Green;
				case 3: return Color.Red;
				case 4: return Color.DarkBlue;
				case 5: return Color.DarkRed;
				case 6: return Color.DarkGreen;
				case 7: return Color.Magenta;
				case 8: return Color.Gray;
				default : return Color.Empty;
			}
		}
	}
}
