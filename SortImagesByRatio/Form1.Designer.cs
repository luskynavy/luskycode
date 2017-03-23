namespace SortImagesByRatio
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
            this.label1 = new System.Windows.Forms.Label();
            this.desiredWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.desiredHeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.imagesPath = new System.Windows.Forms.TextBox();
            this.buttonSelectPath = new System.Windows.Forms.Button();
            this.richTextBoxResultsH = new System.Windows.Forms.RichTextBox();
            this.richTextBoxResultsW = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 91);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Sort jpeg";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "desired width";
            // 
            // desiredWidth
            // 
            this.desiredWidth.Location = new System.Drawing.Point(101, 16);
            this.desiredWidth.Name = "desiredWidth";
            this.desiredWidth.Size = new System.Drawing.Size(100, 20);
            this.desiredWidth.TabIndex = 2;
            this.desiredWidth.Text = "176";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "desired height";
            // 
            // desiredHeight
            // 
            this.desiredHeight.Location = new System.Drawing.Point(101, 42);
            this.desiredHeight.Name = "desiredHeight";
            this.desiredHeight.Size = new System.Drawing.Size(100, 20);
            this.desiredHeight.TabIndex = 2;
            this.desiredHeight.Text = "220";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "images path";
            // 
            // imagesPath
            // 
            this.imagesPath.Location = new System.Drawing.Point(101, 68);
            this.imagesPath.Name = "imagesPath";
            this.imagesPath.Size = new System.Drawing.Size(462, 20);
            this.imagesPath.TabIndex = 2;
            // 
            // buttonSelectPath
            // 
            this.buttonSelectPath.Location = new System.Drawing.Point(569, 65);
            this.buttonSelectPath.Name = "buttonSelectPath";
            this.buttonSelectPath.Size = new System.Drawing.Size(24, 23);
            this.buttonSelectPath.TabIndex = 4;
            this.buttonSelectPath.Text = "...";
            this.buttonSelectPath.UseVisualStyleBackColor = true;
            this.buttonSelectPath.Click += new System.EventHandler(this.buttonSelectPath_Click);
            // 
            // richTextBoxResultsH
            // 
            this.richTextBoxResultsH.Location = new System.Drawing.Point(36, 276);
            this.richTextBoxResultsH.Name = "richTextBoxResultsH";
            this.richTextBoxResultsH.ReadOnly = true;
            this.richTextBoxResultsH.Size = new System.Drawing.Size(571, 150);
            this.richTextBoxResultsH.TabIndex = 6;
            this.richTextBoxResultsH.Text = "";
            // 
            // richTextBoxResultsW
            // 
            this.richTextBoxResultsW.Location = new System.Drawing.Point(36, 120);
            this.richTextBoxResultsW.Name = "richTextBoxResultsW";
            this.richTextBoxResultsW.ReadOnly = true;
            this.richTextBoxResultsW.Size = new System.Drawing.Size(571, 150);
            this.richTextBoxResultsW.TabIndex = 6;
            this.richTextBoxResultsW.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "W";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 340);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "H";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 433);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.richTextBoxResultsW);
            this.Controls.Add(this.richTextBoxResultsH);
            this.Controls.Add(this.buttonSelectPath);
            this.Controls.Add(this.imagesPath);
            this.Controls.Add(this.desiredHeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.desiredWidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox desiredWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox desiredHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox imagesPath;
        private System.Windows.Forms.Button buttonSelectPath;
        private System.Windows.Forms.RichTextBox richTextBoxResultsH;
        private System.Windows.Forms.RichTextBox richTextBoxResultsW;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

