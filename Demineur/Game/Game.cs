using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using System.IO;

/*************************************************************************************
 * 
 *			Class       : Game | UserControl
 *			Date        : March 2005
 *			Author      : Bidou
 *			Version		: 1.0.1
 *			Comment		: This is the game control.
 * 
*************************************************************************************/

namespace Demineur
{
	///*************************************************************************************
	/// <summary>
	/// This is the game. It contains the boxes <see cref="Box"/>.
	/// </summary>
	///*************************************************************************************
	public class Game : System.Windows.Forms.UserControl
	{
		#region Graphics Variable

		private System.ComponentModel.Container components = null;

		#endregion

		#region Events&Delegates

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// A flag has been added.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public event FlagAddedEventHandler FlagAdded;
		public delegate void FlagAddedEventHandler(Box c);

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// The game is over.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public event GameOverEventHandler GameOver;
		public delegate void GameOverEventHandler(Box c, int remainedBombs);

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// The game is finished, winner.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public event WinnerEventHandler Winner;
		public delegate void WinnerEventHandler();

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// The first valid click
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public event FirstClickEventHandler FirstClick;
		public delegate void FirstClickEventHandler();

		#endregion

		private BoxCollection _bombCollection = new BoxCollection();
		private Size _gameDim = new Size(10, 10);
		private Random _random = new Random();
		private Array _game = null;
		private int _remainedBombs = -1;	// The number of bombs that remains
		private int _boxRemaining = -1;		// The number of boxes that remains
		private int _totalBombsStart = 12;	// The number of bombs in the game
		private int _nbLeftClick = 0;		// The number of left (valid) click
		private bool _gameEnded = false;

        private Bitmap back = new Bitmap(1, 1);

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Create a new game.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public Game()
		{
			InitializeComponent();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null) components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent()
		{
			// 
			// Game
			// 
			this.Name = "Game";
		}

		#endregion

		#region Properties
		
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set the game dimensions.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public Size GameDim
		{
			get { return this._gameDim; }
			set { this._gameDim = value; }
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get the number of bombs that remains.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public int RemainedBombs
		{
			get { return this._remainedBombs; }
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Get or set the number of bombs when the game start.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public int BombsWhenStart
		{
			get { return this._totalBombsStart; }
			set { this._totalBombsStart = value; }
		}

		#endregion
	
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Create the game (init the two-dimensional array and display the boxes).
		/// </summary>
		/// <param name="size"> A cell's size. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void CreateGame(int size)
		{
			// The game is in fact a two-dimensional array
			this._game = Array.CreateInstance(typeof(Box), _gameDim.Width, _gameDim.Height);
			this._gameEnded = false;		// Game is not over
			this._nbLeftClick = 0;			// Init the left click counter
			this._bombCollection.Clear();	// The bomb

			// Clear all previous controls and set the new size
			this.Controls.Clear();
			this.Size = new Size(_gameDim.Width * size, _gameDim.Height * size);

			// Create the boxes
			for(int i=0; i<_gameDim.Width; i++)
			{
				for(int j=0; j<_gameDim.Height; j++)
				{
					Box b = new Box();
					b.MouseUp += new MouseEventHandler(c_MouseUp);
					b.Pos = new Point(i, j);
					b.Size = new Size(size, size);
					b.Location = new Point(i * size, j * size);
					this.Controls.Add(b);
					this._game.SetValue(b, i, j);
				}
			}
			this.CreateNeighborhood(); // Link the cells
			this._boxRemaining = (_gameDim.Width * _gameDim.Height) - this._totalBombsStart;
			this._remainedBombs = this._totalBombsStart;


            try
            {
                //choose a random image from args, no / at the end of path
                DirectoryInfo dir = new DirectoryInfo(Environment.GetCommandLineArgs()[1]);
                FileInfo[] files = dir.GetFiles("*.jpg");
                Random rand = new Random();
                int choosenImage = rand.Next(files.Length);
                back = new Bitmap(files[choosenImage].FullName);

                //resize image if bigger than board
                if (back.Height > _gameDim.Height * size)
                {
                    back = new Bitmap(back, new Size(back.Width * _gameDim.Height * size / back.Height, _gameDim.Height * size));
                }

                if (back.Width > _gameDim.Width * size)
                {
                    back = new Bitmap(back, new Size(_gameDim.Width * size, back.Height * _gameDim.Width * size / back.Width));
                }
            }
            catch
            {
            }
		}

		#region Link

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Link the cells.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void CreateNeighborhood()
		{
			for(int i=0; i<_gameDim.Width; i++)
			{
				for(int j=0; j<_gameDim.Height; j++)
				{
					Box b = this._game.GetValue(i, j) as Box;
					this.Link(b, i-1, j-1);	// TopLeft
					this.Link(b, i, j-1);	// TopMiddle
					this.Link(b, i+1, j-1);	// TopRight			X  X  X
					this.Link(b, i-1, j);	// MiddleLeft		X  o  X
					this.Link(b, i+1, j);	// MiddleRight		X  X  X
					this.Link(b, i-1, j+1);	// BottomLeft
					this.Link(b, i, j+1);	// BottomMiddle
					this.Link(b, i+1, j+1);	// BottomRight
				}
			}
		}
	
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Link a box to another (its neighbor).
		/// </summary>
		/// <param name="b"> The box the link. </param>
		/// <param name="i"> The x position in the array. </param>
		/// <param name="j"> The y position in the array. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void Link(Box b, int i, int j)
		{
			try
			{
				Box array = this._game.GetValue(i, j) as Box;
				b.Neighbors.Add(array);
			}
			catch(Exception) {} // ArrayOutOfBoundsException here
		}

		#endregion

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Add some bombs randomly to the game.
		/// </summary>
		/// <param name="n"> The number of bombs to add. </param>
		/// <param name="b"> Boxes that cannot contain a bomb. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void AddBombs(int n, Box[] b)
		{
			// If the box contains already a bomb, try another one
			for(int bomb=0; bomb<n; bomb++) while(!AddBomb(b)); 
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Add a bomb randomly. If the choosen box is already a bomb, return false,
		/// otherwise, put the bomb and return true.
		/// </summary>
		/// <param name="box"> A set of box that cannot contain a bomb. </param>
		/// <returns> True if the bomb has been placed, otherwise false. </returns>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private bool AddBomb(Box[] box)
		{
			int x = _random.Next(this._gameDim.Width);
			int y = _random.Next(this._gameDim.Height);
			Box b = _game.GetValue(x, y) as Box;

			if(b.IsBomb) return false;	// already a bomb
			if(box != null)				// Check if authorized
			{	// Found in the banned list !
				foreach(Box test in box) if(b.Equals(test)) return false;
			}	
			// The box is now a bomb
			b.IsBomb = true;
			this._bombCollection.Add(b);
			return true;
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// The user click on a box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void c_MouseUp(object sender, MouseEventArgs e)
		{
			Box b = sender as Box;
			// Cannot be clicked or game over, do nothing
			if(!b.CanClick || _gameEnded) return;

			if(e.Button == MouseButtons.Right) // Right button pressed
			{
				if(b.IsFlaged) // The clicked box is already flaged
				{
					b.IsFlaged = false;				// "Deflaged"
					b.Image = null;					// Erase the flag image
					this._remainedBombs++;			// Increment
					this._bombCollection.Remove(b);	// Remove the bomb
				}
				else // The clicked box is not flaged
				{
					b.IsFlaged = true;		// Flag
					b.Image = Tools.GetAssemblyImage("Images", "flag.bmp"); // Draw the flag
					this._remainedBombs--;			// Decrement
					this._bombCollection.Add(b);	// Add the bomb
				}
				if(FlagAdded != null) FlagAdded(b); // Flag has been added or removed !
			}
			else if(e.Button == MouseButtons.Left) // Left button pressed
			{
				if(b.IsFlaged) return;	// A flag cannot be left clicked
				this._nbLeftClick++;
				if(this._nbLeftClick == 1)
				{
					this.AddBombs(this._totalBombsStart, new Box[]{ b }); // First valid click
					if(FirstClick != null) FirstClick();
				}
				if(b.IsBomb) // It's a bomb, the game is now over
				{
					b.FlatStyle = FlatStyle.Flat;	// Flat style for clicked box
					this.ShowBombs(b, false);		// Show all bombs
					this._gameEnded = true;			// End the game
					if(GameOver != null) GameOver(b, this._remainedBombs); // Game over
				}
				else if(!b.IsBomb) this.DisplayNumber(b); // Ok, it's not a bomb
			}

			if(this._boxRemaining <= 0) // Winner (should never be < 0)
			{
				this._gameEnded = true;	// End the game
				this.ShowBombs(null, true);
				if(Winner != null) Winner();
			}
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Show all bombs.
		/// </summary>
		/// <param name="looser"> The bombs that the user clicked. </param>
		/// <param name="winner"> True if the user win, false if the user loose. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void ShowBombs(Box looser, bool winner)
		{
			foreach(Box b in this._bombCollection)
			{
				if(b == looser) this.PutBomb(b, "foundedBomb.bmp", Color.Red);
				else if(!b.IsFlaged)
				{
					if(winner) this.PutBomb(b, "flag.bmp", Color.Silver);
					else this.PutBomb(b, "bomb.bmp", Color.Silver);
				}
				else if(!b.IsBomb && b.IsFlaged) this.PutBomb(b, "notBomb.bmp", Color.Silver);	// Normal
			}
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Draw a bomb on a button.
		/// </summary>
		/// <param name="b"> The button. </param>
		/// <param name="imageName"> The image to display. </param>
		/// <param name="c"> The button's backcolor. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void PutBomb(Box b, string imageName, Color c)
		{            
			b.FlatStyle = FlatStyle.Flat;
			b.BackColor = c;
			b.Image = Tools.GetAssemblyImage("Images", imageName);
		}

        //return a cropped image from position and size
        public static Bitmap CropImage(Image source, int x, int y, int width, int height)
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Display the number of neighbors near a box and "expand" the game.
		/// </summary>
		/// <param name="b"> The box to check. </param>
		/// <remarks> Note the recurency. </remarks>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void DisplayNumber(Box b)
		{
            b.Image = CropImage(back, (b.Pos.X * b.Size.Width) % back.Width, (b.Pos.Y * b.Size.Height) % back.Height, b.Size.Width, b.Size.Height);

			b.FlatStyle = FlatStyle.Flat;			// Flat style for clicked boxes
			b.BackColor = Color.Silver;				// Change the background color
			b.IsChecked = true;
			b.CanClick = false;

			this._boxRemaining--;					// Decrement the number of bombs that remains
			int bombsNear = b.BombInNeighBors();	// Get the number of bombs near a box

			if(bombsNear > 0) // There is some bombs, display the number of bombs
			{
				b.Text = bombsNear.ToString();
				b.Font = new Font("Arial", 9, FontStyle.Bold);
				b.ForeColor = Tools.GetColor(bombsNear);
			}
			else // There is no bomb near this box, go to the next one
			{
				foreach(Box box in b.Neighbors)
				{	// Recurency !
					if(!box.IsChecked && !box.IsFlaged) this.DisplayNumber(box); 
				}
			}
		}
	}

	#region Level

	///*************************************************************************************
	/// <summary>
	/// The different levels.
	/// </summary>
	///*************************************************************************************
	public enum Level
	{
		Beginner,
		Intermediate,
		Expert,
		Custom
	}

	#endregion
}