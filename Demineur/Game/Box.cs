using System;
using System.Drawing;

/*************************************************************************************
 * 
 *			Class       : Box | UserControl, BoxCollection
 *			Date        : March 2005
 *			Author      : Bidou
 *			Version		: 1.0.0
 *			Comment		: This class extends a Button.
 * 
*************************************************************************************/

namespace Demineur
{
	///*************************************************************************************
	/// <summary>
	/// This is a special button. I use this to simulate a box.
	/// </summary>
	///*************************************************************************************
	public class Box : System.Windows.Forms.Button
	{
		private bool _isBomb = false;
		private bool _canClick = true;
		private bool _isFlaged = false;
		private bool _isChecked = false;
		private Point _position = Point.Empty;
		private BoxCollection _neighbors = new BoxCollection();
		
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Return the number of bombs in the neighborhood.
		/// </summary>
		/// <returns></returns>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public int BombInNeighBors()
		{
			int bomb = 0;
			foreach(Box b in this._neighbors)
			{
				if(b.IsBomb) bomb++;
			}
			return bomb;
		}

		#region Properties

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set the Neighbors of the box.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public BoxCollection Neighbors
		{
			get { return this._neighbors; }
			set { this._neighbors = value; }
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set the position of the box in the array.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public Point Pos
		{
			get { return this._position; }
			set { this._position = value; }
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set if the box has been treated at least once.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool IsChecked
		{
			get { return this._isChecked; }
			set { this._isChecked = value; }
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set if the box is a bomb.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool IsBomb
		{
			get { return this._isBomb; }
			set { this._isBomb = value; }
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set if the box can be clicked.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool CanClick
		{
			get { return this._canClick; }
			set { this._canClick = value; }
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set if the box is flaged.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool IsFlaged
		{
			get { return this._isFlaged; }
			set { this._isFlaged = value; }
		}

		#endregion
	}
}