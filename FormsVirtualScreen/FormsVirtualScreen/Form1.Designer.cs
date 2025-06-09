namespace FormsVirtualScreen
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxRealReactangle = new System.Windows.Forms.CheckBox();
            this.infos = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(412, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Get Virtual Screen Infos";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBoxRealReactangle
            // 
            this.checkBoxRealReactangle.AutoSize = true;
            this.checkBoxRealReactangle.Location = new System.Drawing.Point(12, 41);
            this.checkBoxRealReactangle.Name = "checkBoxRealReactangle";
            this.checkBoxRealReactangle.Size = new System.Drawing.Size(106, 14);
            this.checkBoxRealReactangle.TabIndex = 1;
            this.checkBoxRealReactangle.Text = "Real VIsual Rectangle";
            this.checkBoxRealReactangle.UseVisualStyleBackColor = true;
            // 
            // infos
            // 
            this.infos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infos.Location = new System.Drawing.Point(12, 64);
            this.infos.Multiline = true;
            this.infos.Name = "infos";
            this.infos.Size = new System.Drawing.Size(412, 243);
            this.infos.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 319);
            this.Controls.Add(this.infos);
            this.Controls.Add(this.checkBoxRealReactangle);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxRealReactangle;
        private System.Windows.Forms.TextBox infos;
    }
}

