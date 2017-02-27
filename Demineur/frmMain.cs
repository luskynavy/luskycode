using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

/*************************************************************************************
 * 
 *			Class       : frmMain | Window form
 *			Date        : March 2005
 *			Author      : Bidou
 *			Version		: 1.0.0
 *			Comment		: Display the main form
 * 
*************************************************************************************/
// from http://codes-sources.commentcamarche.net/source/29917-demineur-minesweeper


namespace Demineur
{
	///*************************************************************************************
	/// <summary>
	/// This is the main form.
	/// </summary>
	///*************************************************************************************
	public class frmMain : System.Windows.Forms.Form
	{
		#region Graphics Variables

		private Demineur.Game game;
		private TimeCounter.Counter counterBomb;
		private TimeCounter.Counter counterTime;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.Timer timer;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem mnuGame;
		private System.Windows.Forms.MenuItem mnuGameNew;
		private System.Windows.Forms.MenuItem mnuGameExit;
		private System.Windows.Forms.MenuItem mnuGameSep1;
		private System.Windows.Forms.MenuItem mnuGameOptions;
		private System.Windows.Forms.MenuItem mnuGameLevel;
		private System.Windows.Forms.MenuItem mnuGameLevelBegin;
		private System.Windows.Forms.MenuItem mnuGameLevelInter;
		private System.Windows.Forms.MenuItem mnuGameBest;
		private System.Windows.Forms.MenuItem mnuGameLevelExpert;
		private System.Windows.Forms.MenuItem mnuGameLevelCustom;
		private System.Windows.Forms.MenuItem mnuGameSep2;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuHelpAbout;
		private System.Windows.Forms.Button btnRestart;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Panel panelMain;
		private System.Windows.Forms.Panel panelLeft;
		private System.Windows.Forms.Panel panelRight;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel statusBarPanel;

		#endregion

		private int _counter = 0;
		public static readonly string OPTIONSFILEPATH = Application.StartupPath + "\\" + "options.xml";
		public static readonly string SCOREFILEPATH = Application.StartupPath + "\\" + "score.bin";
		public static Hashtable highScore = null;
		public static Options options = null;
	
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Create the main form.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public frmMain()
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

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			this.game = new Demineur.Game();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.mnuGame = new System.Windows.Forms.MenuItem();
			this.mnuGameNew = new System.Windows.Forms.MenuItem();
			this.mnuGameSep1 = new System.Windows.Forms.MenuItem();
			this.mnuGameLevel = new System.Windows.Forms.MenuItem();
			this.mnuGameLevelBegin = new System.Windows.Forms.MenuItem();
			this.mnuGameLevelInter = new System.Windows.Forms.MenuItem();
			this.mnuGameLevelExpert = new System.Windows.Forms.MenuItem();
			this.mnuGameLevelCustom = new System.Windows.Forms.MenuItem();
			this.mnuGameOptions = new System.Windows.Forms.MenuItem();
			this.mnuGameBest = new System.Windows.Forms.MenuItem();
			this.mnuGameSep2 = new System.Windows.Forms.MenuItem();
			this.mnuGameExit = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuHelpAbout = new System.Windows.Forms.MenuItem();
			this.panelMain = new System.Windows.Forms.Panel();
			this.panelTop = new System.Windows.Forms.Panel();
			this.panelRight = new System.Windows.Forms.Panel();
			this.counterTime = new TimeCounter.Counter();
			this.panelLeft = new System.Windows.Forms.Panel();
			this.counterBomb = new TimeCounter.Counter();
			this.btnRestart = new System.Windows.Forms.Button();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.panelMain.SuspendLayout();
			this.panelTop.SuspendLayout();
			this.panelRight.SuspendLayout();
			this.panelLeft.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
			this.SuspendLayout();
			// 
			// game
			// 
			this.game.BackColor = System.Drawing.SystemColors.Control;
			this.game.BombsWhenStart = 12;
			this.game.Enabled = false;
			this.game.GameDim = new System.Drawing.Size(10, 10);
			this.game.Location = new System.Drawing.Point(10, 64);
			this.game.Name = "game";
			this.game.TabIndex = 0;
			this.game.FirstClick += new Demineur.Game.FirstClickEventHandler(this.game_FirstClick);
			this.game.Resize += new System.EventHandler(this.game_Resize);
			this.game.Winner += new Demineur.Game.WinnerEventHandler(this.game_Winner);
			this.game.GameOver += new Demineur.Game.GameOverEventHandler(this.game_GameOver);
			this.game.FlagAdded += new Demineur.Game.FlagAddedEventHandler(this.game_FlagAdded);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuGame,
																					 this.mnuHelp});
			// 
			// mnuGame
			// 
			this.mnuGame.Index = 0;
			this.mnuGame.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuGameNew,
																					this.mnuGameSep1,
																					this.mnuGameLevel,
																					this.mnuGameOptions,
																					this.mnuGameBest,
																					this.mnuGameSep2,
																					this.mnuGameExit});
			this.mnuGame.Text = "Jeu";
			// 
			// mnuGameNew
			// 
			this.mnuGameNew.Index = 0;
			this.mnuGameNew.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.mnuGameNew.Text = "Nouveau";
			this.mnuGameNew.Click += new System.EventHandler(this.mnuGameNew_Click);
			// 
			// mnuGameSep1
			// 
			this.mnuGameSep1.Index = 1;
			this.mnuGameSep1.Text = "-";
			// 
			// mnuGameLevel
			// 
			this.mnuGameLevel.Index = 2;
			this.mnuGameLevel.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.mnuGameLevelBegin,
																						 this.mnuGameLevelInter,
																						 this.mnuGameLevelExpert,
																						 this.mnuGameLevelCustom});
			this.mnuGameLevel.Text = "Niveau";
			// 
			// mnuGameLevelBegin
			// 
			this.mnuGameLevelBegin.Checked = true;
			this.mnuGameLevelBegin.DefaultItem = true;
			this.mnuGameLevelBegin.Index = 0;
			this.mnuGameLevelBegin.RadioCheck = true;
			this.mnuGameLevelBegin.Text = "Débutant";
			this.mnuGameLevelBegin.Click += new System.EventHandler(this.mnuGameLevelBegin_Click);
			// 
			// mnuGameLevelInter
			// 
			this.mnuGameLevelInter.Index = 1;
			this.mnuGameLevelInter.RadioCheck = true;
			this.mnuGameLevelInter.Text = "Intermédiaire";
			this.mnuGameLevelInter.Click += new System.EventHandler(this.mnuGameLevelInter_Click);
			// 
			// mnuGameLevelExpert
			// 
			this.mnuGameLevelExpert.Index = 2;
			this.mnuGameLevelExpert.RadioCheck = true;
			this.mnuGameLevelExpert.Text = "Expert";
			this.mnuGameLevelExpert.Click += new System.EventHandler(this.mnuGameLevelExpert_Click);
			// 
			// mnuGameLevelCustom
			// 
			this.mnuGameLevelCustom.Index = 3;
			this.mnuGameLevelCustom.RadioCheck = true;
			this.mnuGameLevelCustom.Text = "Custom";
			this.mnuGameLevelCustom.Click += new System.EventHandler(this.mnuGameLevelCustom_Click);
			// 
			// mnuGameOptions
			// 
			this.mnuGameOptions.Index = 3;
			this.mnuGameOptions.Text = "Options";
			this.mnuGameOptions.Click += new System.EventHandler(this.mnuGameOptions_Click);
			// 
			// mnuGameBest
			// 
			this.mnuGameBest.Index = 4;
			this.mnuGameBest.Text = "Meilleurs Scores";
			this.mnuGameBest.Click += new System.EventHandler(this.mnuGameBest_Click);
			// 
			// mnuGameSep2
			// 
			this.mnuGameSep2.Index = 5;
			this.mnuGameSep2.Text = "-";
			// 
			// mnuGameExit
			// 
			this.mnuGameExit.Index = 6;
			this.mnuGameExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.mnuGameExit.Text = "Quitter";
			this.mnuGameExit.Click += new System.EventHandler(this.mnuGameExit_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 1;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuHelpAbout});
			this.mnuHelp.Text = "Aide";
			// 
			// mnuHelpAbout
			// 
			this.mnuHelpAbout.Index = 0;
			this.mnuHelpAbout.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.mnuHelpAbout.Text = "A propos de";
			this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
			// 
			// panelMain
			// 
			this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panelMain.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panelMain.Controls.Add(this.panelTop);
			this.panelMain.Controls.Add(this.game);
			this.panelMain.Location = new System.Drawing.Point(8, 8);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(170, 224);
			this.panelMain.TabIndex = 0;
			// 
			// panelTop
			// 
			this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelTop.Controls.Add(this.panelRight);
			this.panelTop.Controls.Add(this.panelLeft);
			this.panelTop.Controls.Add(this.btnRestart);
			this.panelTop.Location = new System.Drawing.Point(8, 8);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(152, 40);
			this.panelTop.TabIndex = 4;
			// 
			// panelRight
			// 
			this.panelRight.BackColor = System.Drawing.Color.Black;
			this.panelRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelRight.Controls.Add(this.counterTime);
			this.panelRight.Location = new System.Drawing.Point(92, 5);
			this.panelRight.Name = "panelRight";
			this.panelRight.Size = new System.Drawing.Size(49, 26);
			this.panelRight.TabIndex = 7;
			// 
			// counterTime
			// 
			this.counterTime.AddZero = true;
			this.counterTime.BackColor = System.Drawing.Color.Black;
			this.counterTime.DigitActiveColor = System.Drawing.Color.Red;
			this.counterTime.DigitPassiveColor = System.Drawing.Color.DarkRed;
			this.counterTime.Location = new System.Drawing.Point(1, 1);
			this.counterTime.Name = "counterTime";
			this.counterTime.NumberOfDigit = 3;
			this.counterTime.Size = new System.Drawing.Size(45, 20);
			this.counterTime.TabIndex = 5;
			this.counterTime.TimeOverflow += new TimeCounter.Counter.TimeOverflowEventHandler(this.counterTime_TimeOverflow);
			// 
			// panelLeft
			// 
			this.panelLeft.BackColor = System.Drawing.Color.Black;
			this.panelLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelLeft.Controls.Add(this.counterBomb);
			this.panelLeft.Location = new System.Drawing.Point(5, 5);
			this.panelLeft.Name = "panelLeft";
			this.panelLeft.Size = new System.Drawing.Size(49, 26);
			this.panelLeft.TabIndex = 6;
			// 
			// counterBomb
			// 
			this.counterBomb.AddZero = true;
			this.counterBomb.BackColor = System.Drawing.Color.Black;
			this.counterBomb.DigitActiveColor = System.Drawing.Color.Red;
			this.counterBomb.DigitPassiveColor = System.Drawing.Color.DarkRed;
			this.counterBomb.Location = new System.Drawing.Point(1, 1);
			this.counterBomb.Name = "counterBomb";
			this.counterBomb.NumberOfDigit = 3;
			this.counterBomb.Size = new System.Drawing.Size(45, 20);
			this.counterBomb.TabIndex = 4;
			// 
			// btnRestart
			// 
			this.btnRestart.BackColor = System.Drawing.Color.Silver;
			this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRestart.Image = ((System.Drawing.Image)(resources.GetObject("btnRestart.Image")));
			this.btnRestart.Location = new System.Drawing.Point(60, 6);
			this.btnRestart.Name = "btnRestart";
			this.btnRestart.Size = new System.Drawing.Size(28, 26);
			this.btnRestart.TabIndex = 1;
			this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 239);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusBarPanel});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(186, 20);
			this.statusBar.SizingGrip = false;
			this.statusBar.TabIndex = 1;
			// 
			// statusBarPanel
			// 
			this.statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel.Text = "Jeu/Nouveau pour commencer";
			this.statusBarPanel.ToolTipText = "En attente...";
			this.statusBarPanel.Width = 186;
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(186, 259);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.panelMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMenu;
			this.Name = "frmMain";
			this.Text = "Démineur";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.panelMain.ResumeLayout(false);
			this.panelTop.ResumeLayout(false);
			this.panelRight.ResumeLayout(false);
			this.panelLeft.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Try to load the options (if an error occurs, default values are used).
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		[STAThread]
		static void Main() 
		{
			try
			{
				options = Configuration.Deserialize(OPTIONSFILEPATH);
				highScore = BestScore.Load(SCOREFILEPATH);
			}
			catch(Exception)
			{
				options = new Options(); // Error, load defaut values
				highScore = new Hashtable();
			}
			Application.Run(new frmMain()); // Load the application
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Increment the timer.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void timer_Tick(object sender, System.EventArgs e)
		{
			this._counter++;
			this.counterTime.SetValue(this._counter);
		}

		#region Menu

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Exit the application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuGameExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Start a new game.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuGameNew_Click(object sender, System.EventArgs e)
		{
			StartGame();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Display the about form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuHelpAbout_Click(object sender, System.EventArgs e)
		{
			frmAbout frmAbout = new frmAbout();
			frmAbout.ShowDialog();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Display the options form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuGameOptions_Click(object sender, System.EventArgs e)
		{
			frmOptions frmOpt = new frmOptions();
			if(frmOpt.ShowDialog() == DialogResult.OK) this.StartGame();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Set the level (beginner).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuGameLevelBegin_Click(object sender, System.EventArgs e)
		{
			options.level = Level.Beginner;
			this.SelectGoodMenu(Level.Beginner);
			this.StartGame();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Set the level (intermediate).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuGameLevelInter_Click(object sender, System.EventArgs e)
		{
			options.level = Level.Intermediate;
			this.SelectGoodMenu(Level.Intermediate);
			this.StartGame();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Set the level (expert).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuGameLevelExpert_Click(object sender, System.EventArgs e)
		{
			options.level = Level.Expert;
			this.SelectGoodMenu(Level.Expert);
			this.StartGame();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Set the level (custom).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuGameLevelCustom_Click(object sender, System.EventArgs e)
		{
			options.level = Level.Custom;
			this.SelectGoodMenu(Level.Custom);
			this.StartGame();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Show the best score.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void mnuGameBest_Click(object sender, System.EventArgs e)
		{
			new frmBestScore().ShowDialog();
		}

		#endregion

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Select the good level.
		/// </summary>
		/// <param name="level"> The level to select. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void SelectGoodMenu(Level level)
		{
			foreach(MenuItem mi in this.mnuGameLevel.MenuItems) mi.Checked = false;
			switch(level)
			{
				case Level.Beginner :
					this.mnuGameLevelBegin.Checked = true;
					break;
				case Level.Intermediate : 
					this.mnuGameLevelInter.Checked = true;
					break;
				case Level.Expert : 
					this.mnuGameLevelExpert.Checked = true;
					break;
				case Level.Custom : 
					this.mnuGameLevelCustom.Checked = true;
					break;
			}
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Start a new game.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void btnRestart_Click(object sender, System.EventArgs e)
		{
			this.StartGame();
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Create a new game. One specifies the game's size, the cells' size, 
		/// and the number of bombs and the start the game (timer is enabled)
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void StartGame()
		{
			this.btnRestart.Enabled = false;
			this.DisplayGame();			// Display the game
			this._counter = 0;			// (Re)init the counter
			this.counterBomb.SetValue(this.game.RemainedBombs);
			this.statusBarPanel.Text = "Le jeu est prêt";
			// Game and restart button enabled
			this.game.Enabled = true;
			this.btnRestart.Enabled = true;
			this.counterTime.SetValue(0);
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Create a new game. One specifies the game's size, the cells' size, 
		/// and the number of bombs.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void DisplayGame()
		{
			const int CELLSIZE = 20;
			this.statusBarPanel.Text = "Création du jeu...";
			// Create the game
			switch(options.level)
			{
				case Level.Beginner :
					this.game.GameDim = new Size(10, 10);
					this.game.BombsWhenStart = 13;
					break;
				case Level.Intermediate :
					this.game.GameDim = new Size(20, 20);
					this.game.BombsWhenStart = 60;
					break;
				case Level.Expert : 
					this.game.GameDim = new Size(30, 30);
					this.game.BombsWhenStart = 140;
					break;
				case Level.Custom :
					this.game.GameDim = options.gameSize;
					this.game.BombsWhenStart = options.bombs;
					break;
			}

			this.game.CreateGame(CELLSIZE);
			// Display the smiley and update the status bar
			this.btnRestart.Image = Tools.GetAssemblyImage("Images", "happySmiley.bmp");
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// When the user close the application, save the options.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				Configuration.Serialize(OPTIONSFILEPATH, options); // Try to save
				BestScore.Save(SCOREFILEPATH, highScore);
			}
			catch(Exception ex) // Error occured
			{
				MessageBox.Show("Vos options n'ont pas pu être sauvegardée pour la raison suivante :\n" +
					ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Display the game on load.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void frmMain_Load(object sender, System.EventArgs e)
		{
			this.SelectGoodMenu(options.level);
			this.DisplayGame();
			this.statusBarPanel.Text ="F2 ou menu pour nouvelle partie";
		}

		#region Game Events

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Winner. High score ?
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void game_Winner()
		{
			this.timer.Enabled = false;
			this.btnRestart.Image = Tools.GetAssemblyImage("Images", "winnerSmiley.bmp");
			this.statusBarPanel.Text = "Fin de la partie - vous avez gagné";

			if(options.level == Level.Custom) return;

			User u = null;
			if(highScore.ContainsKey(options.level)) u = highScore[options.level] as User;
			else u = new User();

			// No best score or new best score
			if(this._counter < u.Time || u.Time == 0)
			{
				highScore.Remove(options.level);
				frmName frmName = new frmName();
				frmName.Time = this._counter;
				// Display the form
				if(frmName.ShowDialog() == DialogResult.OK) u.Name = frmName.UserName;
				else u.Name = "Unknown";
				// Set the time and add to the hashtable
				u.Time = this._counter;
				highScore.Add(options.level, u);
			}
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// The game is over.
		/// </summary>
		/// <param name="c"> The bomb location. </param>
		/// <param name="remainedBombs"> The number of bombs that remains. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void game_GameOver(Demineur.Box b, int remainedBombs)
		{
			this.timer.Enabled = false;
			this.btnRestart.Image = Tools.GetAssemblyImage("Images", "sadSmiley.bmp");
			this.btnRestart.Focus();
			this.statusBarPanel.Text = "Fin de la partie - vous avez perdu";
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// The user add a flag. Display the number of bombs that remains.
		/// </summary>
		/// <param name="b"> The flaged cell. </param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void game_FlagAdded(Demineur.Box b)
		{
			this.counterBomb.SetValue(this.game.RemainedBombs);
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// When the game is resized, the main form should also be resized.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void game_Resize(object sender, System.EventArgs e)
		{
			this.Size = new Size(this.game.Width + 40, this.game.Height + 160);
			this.panelTop.Location = new Point((this.panelMain.Width - this.panelTop.Width) / 2, this.panelTop.Top);
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Occurs after the first valid click on the game.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void game_FirstClick()
		{
			this.timer.Enabled = true;	// Start the counter	
		}

		#endregion

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Time up !
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void counterTime_TimeOverflow()
		{
			this.timer.Enabled = false;
			this.game.Enabled = false;
			this.statusBarPanel.Text = "Fin de la partie - vous avez perdu (temps)";
			MessageBox.Show("Vous avez perdu (plus de temps!)", "Perdu", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
