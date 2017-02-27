using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

/*************************************************************************************
 * 
 *			Class       : frmOptions | Window form
 *			Date        : March 2005
 *			Author      : Bidou
 *			Version		: 1.0.0
 *			Comment		: Display the options form
 * 
*************************************************************************************/

namespace Demineur
{
	///*************************************************************************************
	/// <summary>
	/// Let the user makes some customizations.
	/// </summary>
	///*************************************************************************************
	public class frmOptions : System.Windows.Forms.Form
	{
		#region Graphics Variable

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.NumericUpDown numWidth;
		private System.Windows.Forms.NumericUpDown numHeight;
		private System.Windows.Forms.NumericUpDown numBombs;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.ComponentModel.Container components = null;

		#endregion

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Create a new Option form.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public frmOptions()
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmOptions));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.numHeight = new System.Windows.Forms.NumericUpDown();
			this.numWidth = new System.Windows.Forms.NumericUpDown();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.numBombs = new System.Windows.Forms.NumericUpDown();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numBombs)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.numHeight);
			this.groupBox1.Controls.Add(this.numWidth);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(176, 80);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Grandeur du jeu";
			// 
			// numHeight
			// 
			this.numHeight.Location = new System.Drawing.Point(80, 48);
			this.numHeight.Maximum = new System.Decimal(new int[] {
																	  30,
																	  0,
																	  0,
																	  0});
			this.numHeight.Minimum = new System.Decimal(new int[] {
																	  10,
																	  0,
																	  0,
																	  0});
			this.numHeight.Name = "numHeight";
			this.numHeight.Size = new System.Drawing.Size(56, 20);
			this.numHeight.TabIndex = 1;
			this.numHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numHeight.Value = new System.Decimal(new int[] {
																	10,
																	0,
																	0,
																	0});
			this.numHeight.ValueChanged += new System.EventHandler(this.numHeight_ValueChanged);
			// 
			// numWidth
			// 
			this.numWidth.Location = new System.Drawing.Point(80, 24);
			this.numWidth.Maximum = new System.Decimal(new int[] {
																	 30,
																	 0,
																	 0,
																	 0});
			this.numWidth.Minimum = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.numWidth.Name = "numWidth";
			this.numWidth.Size = new System.Drawing.Size(56, 20);
			this.numWidth.TabIndex = 0;
			this.numWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numWidth.Value = new System.Decimal(new int[] {
																   10,
																   0,
																   0,
																   0});
			this.numWidth.ValueChanged += new System.EventHandler(this.numWidth_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.numBombs);
			this.groupBox2.Location = new System.Drawing.Point(16, 96);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(176, 56);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Nombre de bombe";
			// 
			// numBombs
			// 
			this.numBombs.Location = new System.Drawing.Point(80, 24);
			this.numBombs.Minimum = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.numBombs.Name = "numBombs";
			this.numBombs.Size = new System.Drawing.Size(56, 20);
			this.numBombs.TabIndex = 0;
			this.numBombs.Value = new System.Decimal(new int[] {
																   10,
																   0,
																   0,
																   0});
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(240)), ((System.Byte)(240)), ((System.Byte)(240)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnOk);
			this.panel1.Location = new System.Drawing.Point(-8, 200);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(216, 48);
			this.panel1.TabIndex = 2;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(96, 8);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(48, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(152, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(48, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Largeur";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Hauteur";
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(192, 32);
			this.label3.TabIndex = 3;
			this.label3.Text = "Options valable uniquement lors du choix de Niveau \'Custom\'";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(202, 240);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOptions";
			this.Text = "Options";
			this.Load += new System.EventHandler(this.frmOptions_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numBombs)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// When the form load, fill the controls.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void frmOptions_Load(object sender, System.EventArgs e)
		{
			this.numWidth.Value = frmMain.options.gameSize.Width;
			this.numHeight.Value = frmMain.options.gameSize.Height;
			this.numBombs.Value = frmMain.options.bombs;
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Assign the new values to the 'options' static fields.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			int max = (int)((this.numHeight.Value * this.numWidth.Value) / 2);
			if(this.numBombs.Value > max) this.numBombs.Value = max;
			frmMain.options.bombs = Convert.ToInt32(this.numBombs.Value);
			frmMain.options.gameSize = new Size(Convert.ToInt32(this.numWidth.Value), Convert.ToInt32(this.numHeight.Value));
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// No comment.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void numWidth_ValueChanged(object sender, System.EventArgs e)
		{
			int nbCells = (int)(this.numHeight.Value * this.numWidth.Value);
			this.numBombs.Maximum = (int)nbCells/2;
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// No comment.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void numHeight_ValueChanged(object sender, System.EventArgs e)
		{
			int nbCells = (int)(this.numHeight.Value * this.numWidth.Value);
			this.numBombs.Maximum = (int)nbCells/2;
		}
	}
}
