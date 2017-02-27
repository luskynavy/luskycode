using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

/*************************************************************************************
 * 
 *			Class       : frmBestScore | Window form
 *			Date        : March 2005
 *			Author      : Bidou
 *			Version		: 1.0.0
 *			Comment		: None
 * 
*************************************************************************************/

namespace Demineur
{
	public class frmBestScore : System.Windows.Forms.Form
	{
		#region Graphics Variables

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelUnderline;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblBeginName;
		private System.Windows.Forms.Label lblBeginTime;
		private System.Windows.Forms.Label lblInterName;
		private System.Windows.Forms.Label lblExpertName;
		private System.Windows.Forms.Label lblInterTime;
		private System.Windows.Forms.Label lblExpertTime;
		private System.ComponentModel.Container components = null;

		#endregion

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Create a new best score form.
		/// </summary>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public frmBestScore()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmBestScore));
			this.btnOk = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.panelUnderline = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblBeginName = new System.Windows.Forms.Label();
			this.lblBeginTime = new System.Windows.Forms.Label();
			this.lblInterName = new System.Windows.Forms.Label();
			this.lblExpertName = new System.Windows.Forms.Label();
			this.lblInterTime = new System.Windows.Forms.Label();
			this.lblExpertTime = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOk.Location = new System.Drawing.Point(296, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(48, 24);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(240)), ((System.Byte)(240)), ((System.Byte)(240)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.btnOk);
			this.panel1.Location = new System.Drawing.Point(-8, 112);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(360, 48);
			this.panel1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(104, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Nom du record";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(240, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Temps (sec)";
			// 
			// panelUnderline
			// 
			this.panelUnderline.BackColor = System.Drawing.Color.Black;
			this.panelUnderline.Location = new System.Drawing.Point(8, 32);
			this.panelUnderline.Name = "panelUnderline";
			this.panelUnderline.Size = new System.Drawing.Size(328, 2);
			this.panelUnderline.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Débutant :";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "Intermédiaire :";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 16);
			this.label5.TabIndex = 7;
			this.label5.Text = "Expert :";
			// 
			// lblBeginName
			// 
			this.lblBeginName.Location = new System.Drawing.Point(104, 40);
			this.lblBeginName.Name = "lblBeginName";
			this.lblBeginName.Size = new System.Drawing.Size(128, 16);
			this.lblBeginName.TabIndex = 8;
			this.lblBeginName.Text = "Unknown";
			// 
			// lblBeginTime
			// 
			this.lblBeginTime.Location = new System.Drawing.Point(240, 40);
			this.lblBeginTime.Name = "lblBeginTime";
			this.lblBeginTime.Size = new System.Drawing.Size(96, 16);
			this.lblBeginTime.TabIndex = 9;
			this.lblBeginTime.Text = "Unknown";
			// 
			// lblInterName
			// 
			this.lblInterName.Location = new System.Drawing.Point(104, 64);
			this.lblInterName.Name = "lblInterName";
			this.lblInterName.Size = new System.Drawing.Size(128, 16);
			this.lblInterName.TabIndex = 10;
			this.lblInterName.Text = "Unknown";
			// 
			// lblExpertName
			// 
			this.lblExpertName.Location = new System.Drawing.Point(104, 88);
			this.lblExpertName.Name = "lblExpertName";
			this.lblExpertName.Size = new System.Drawing.Size(128, 16);
			this.lblExpertName.TabIndex = 11;
			this.lblExpertName.Text = "Unknown";
			// 
			// lblInterTime
			// 
			this.lblInterTime.Location = new System.Drawing.Point(240, 64);
			this.lblInterTime.Name = "lblInterTime";
			this.lblInterTime.Size = new System.Drawing.Size(96, 16);
			this.lblInterTime.TabIndex = 12;
			this.lblInterTime.Text = "Unknown";
			// 
			// lblExpertTime
			// 
			this.lblExpertTime.Location = new System.Drawing.Point(240, 88);
			this.lblExpertTime.Name = "lblExpertTime";
			this.lblExpertTime.Size = new System.Drawing.Size(96, 16);
			this.lblExpertTime.TabIndex = 13;
			this.lblExpertTime.Text = "Unknown";
			// 
			// frmBestScore
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnOk;
			this.ClientSize = new System.Drawing.Size(346, 152);
			this.Controls.Add(this.lblExpertTime);
			this.Controls.Add(this.lblInterTime);
			this.Controls.Add(this.lblExpertName);
			this.Controls.Add(this.lblInterName);
			this.Controls.Add(this.lblBeginTime);
			this.Controls.Add(this.lblBeginName);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.panelUnderline);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmBestScore";
			this.Text = "Meilleurs Scores";
			this.Load += new System.EventHandler(this.frmBestScore_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~	
		/// <summary>
		/// Load the best scores.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void frmBestScore_Load(object sender, System.EventArgs e)
		{
			Hashtable h = frmMain.highScore;
			if(h == null) return; // No file or error

			IDictionaryEnumerator id = h.GetEnumerator();
			while(id.MoveNext())
			{
				User user = id.Value as User;
				string name = (user == null ? "Unknow" : user.Name);
				string time = (user == null ? "Unknow" : user.Time.ToString());

				switch(id.Key.ToString())
				{
					case "Beginner" : 
						this.lblBeginName.Text = name;
						this.lblBeginTime.Text = time;
						break;
					case "Intermediate" :
						this.lblInterName.Text = name;
						this.lblInterTime.Text = time;
						break;
					case "Expert" :
						this.lblExpertName.Text = name;
						this.lblExpertTime.Text = time;
						break;
				}
			}
		}

		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Close the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			this.Close();	
		}
	}
}
