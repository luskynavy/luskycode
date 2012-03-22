namespace StuffCounter
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.go = new System.Windows.Forms.Button();
            this.realm = new System.Windows.Forms.TextBox();
            this.guild = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ranks = new System.Windows.Forms.TextBox();
            this.analyze = new System.Windows.Forms.Button();
            this.results = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // go
            // 
            this.go.Location = new System.Drawing.Point(386, 0);
            this.go.Name = "go";
            this.go.Size = new System.Drawing.Size(75, 24);
            this.go.TabIndex = 0;
            this.go.Text = "Get guild";
            this.go.UseVisualStyleBackColor = true;
            this.go.Click += new System.EventHandler(this.go_Click);
            // 
            // realm
            // 
            this.realm.Dock = System.Windows.Forms.DockStyle.Left;
            this.realm.Location = new System.Drawing.Point(0, 0);
            this.realm.Name = "realm";
            this.realm.Size = new System.Drawing.Size(100, 20);
            this.realm.TabIndex = 1;
            this.realm.Text = "Eldre\'Thalas";
            // 
            // guild
            // 
            this.guild.Location = new System.Drawing.Point(127, 0);
            this.guild.Name = "guild";
            this.guild.Size = new System.Drawing.Size(100, 20);
            this.guild.TabIndex = 1;
            this.guild.Text = "Rage Against the Murloc";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.ranks);
            this.panel1.Controls.Add(this.analyze);
            this.panel1.Controls.Add(this.go);
            this.panel1.Controls.Add(this.guild);
            this.panel1.Controls.Add(this.realm);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1096, 24);
            this.panel1.TabIndex = 3;
            // 
            // ranks
            // 
            this.ranks.Location = new System.Drawing.Point(254, 0);
            this.ranks.Name = "ranks";
            this.ranks.Size = new System.Drawing.Size(100, 20);
            this.ranks.TabIndex = 5;
            this.ranks.Text = "0;1;2";
            // 
            // analyze
            // 
            this.analyze.Location = new System.Drawing.Point(489, 0);
            this.analyze.Name = "analyze";
            this.analyze.Size = new System.Drawing.Size(75, 23);
            this.analyze.TabIndex = 4;
            this.analyze.Text = "Analyze";
            this.analyze.UseVisualStyleBackColor = true;
            this.analyze.Click += new System.EventHandler(this.analyze_Click);
            // 
            // results
            // 
            this.results.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.results.FullRowSelect = true;
            this.results.GridLines = true;
            this.results.Location = new System.Drawing.Point(0, 26);
            this.results.Name = "results";
            this.results.Size = new System.Drawing.Size(1096, 320);
            this.results.TabIndex = 4;
            this.results.UseCompatibleStateImageBehavior = false;
            this.results.View = System.Windows.Forms.View.Details;
            this.results.DoubleClick += new System.EventHandler(this.results_DoubleClick);
            this.results.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.results_ColumnClick);
            this.results.KeyDown += new System.Windows.Forms.KeyEventHandler(this.results_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 344);
            this.Controls.Add(this.results);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "StuffCounter";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button go;
        private System.Windows.Forms.TextBox realm;
        private System.Windows.Forms.TextBox guild;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button analyze;
        private System.Windows.Forms.ListView results;
        private System.Windows.Forms.TextBox ranks;
    }
}

