namespace CalculeDistanceEcran
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
            this.size = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resolutionX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.distance = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButton1610 = new System.Windows.Forms.RadioButton();
            this.radioButton169 = new System.Windows.Forms.RadioButton();
            this.radioButton43 = new System.Windows.Forms.RadioButton();
            this.sizeCm = new System.Windows.Forms.Label();
            this.widthCm = new System.Windows.Forms.Label();
            this.heightCm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // size
            // 
            this.size.Location = new System.Drawing.Point(107, 32);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(69, 20);
            this.size.TabIndex = 0;
            this.size.Text = "10,1";
            this.size.TextChanged += new System.EventHandler(this.size_TextChanged);
            this.size.Leave += new System.EventHandler(this.Form1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "diagonale (\")";
            // 
            // resolutionX
            // 
            this.resolutionX.Location = new System.Drawing.Point(107, 58);
            this.resolutionX.Name = "resolutionX";
            this.resolutionX.Size = new System.Drawing.Size(69, 20);
            this.resolutionX.TabIndex = 0;
            this.resolutionX.Text = "1280";
            this.resolutionX.Leave += new System.EventHandler(this.Form1_Load);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "résolution x";
            // 
            // distance
            // 
            this.distance.Location = new System.Drawing.Point(107, 166);
            this.distance.Name = "distance";
            this.distance.Size = new System.Drawing.Size(69, 20);
            this.distance.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "distance (cm)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "ratio";
            // 
            // radioButton1610
            // 
            this.radioButton1610.AutoSize = true;
            this.radioButton1610.Checked = true;
            this.radioButton1610.Location = new System.Drawing.Point(108, 89);
            this.radioButton1610.Name = "radioButton1610";
            this.radioButton1610.Size = new System.Drawing.Size(54, 17);
            this.radioButton1610.TabIndex = 2;
            this.radioButton1610.TabStop = true;
            this.radioButton1610.Text = "16/10";
            this.radioButton1610.UseVisualStyleBackColor = true;
            this.radioButton1610.CheckedChanged += new System.EventHandler(this.Form1_Load);
            // 
            // radioButton169
            // 
            this.radioButton169.AutoSize = true;
            this.radioButton169.Location = new System.Drawing.Point(107, 112);
            this.radioButton169.Name = "radioButton169";
            this.radioButton169.Size = new System.Drawing.Size(48, 17);
            this.radioButton169.TabIndex = 2;
            this.radioButton169.Text = "16/9";
            this.radioButton169.UseVisualStyleBackColor = true;
            this.radioButton169.CheckedChanged += new System.EventHandler(this.Form1_Load);
            // 
            // radioButton43
            // 
            this.radioButton43.AutoSize = true;
            this.radioButton43.Location = new System.Drawing.Point(107, 135);
            this.radioButton43.Name = "radioButton43";
            this.radioButton43.Size = new System.Drawing.Size(42, 17);
            this.radioButton43.TabIndex = 2;
            this.radioButton43.Text = "4/3";
            this.radioButton43.UseVisualStyleBackColor = true;
            this.radioButton43.CheckedChanged += new System.EventHandler(this.Form1_Load);
            // 
            // sizeCm
            // 
            this.sizeCm.AutoSize = true;
            this.sizeCm.Location = new System.Drawing.Point(182, 32);
            this.sizeCm.Name = "sizeCm";
            this.sizeCm.Size = new System.Drawing.Size(21, 13);
            this.sizeCm.TabIndex = 3;
            this.sizeCm.Text = "cm";
            // 
            // widthCm
            // 
            this.widthCm.AutoSize = true;
            this.widthCm.Location = new System.Drawing.Point(182, 58);
            this.widthCm.Name = "widthCm";
            this.widthCm.Size = new System.Drawing.Size(21, 13);
            this.widthCm.TabIndex = 3;
            this.widthCm.Text = "cm";
            this.widthCm.Click += new System.EventHandler(this.widthCm_Click);
            // 
            // heightCm
            // 
            this.heightCm.AutoSize = true;
            this.heightCm.Location = new System.Drawing.Point(175, 71);
            this.heightCm.Name = "heightCm";
            this.heightCm.Size = new System.Drawing.Size(28, 13);
            this.heightCm.TabIndex = 3;
            this.heightCm.Text = "* cm";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 225);
            this.Controls.Add(this.heightCm);
            this.Controls.Add(this.widthCm);
            this.Controls.Add(this.sizeCm);
            this.Controls.Add(this.radioButton43);
            this.Controls.Add(this.radioButton169);
            this.Controls.Add(this.radioButton1610);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.distance);
            this.Controls.Add(this.resolutionX);
            this.Controls.Add(this.size);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "CalculeDistanceEcran";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox size;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox resolutionX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox distance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton1610;
        private System.Windows.Forms.RadioButton radioButton169;
        private System.Windows.Forms.RadioButton radioButton43;
        private System.Windows.Forms.Label sizeCm;
        private System.Windows.Forms.Label widthCm;
        private System.Windows.Forms.Label heightCm;
    }
}

