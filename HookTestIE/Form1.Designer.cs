namespace HookTest
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
            this.ReHook = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ReHook
            // 
            this.ReHook.Location = new System.Drawing.Point(0, 0);
            this.ReHook.Name = "ReHook";
            this.ReHook.Size = new System.Drawing.Size(23, 23);
            this.ReHook.TabIndex = 0;
            this.ReHook.Text = "R";
            this.ReHook.UseVisualStyleBackColor = true;
            this.ReHook.Click += new System.EventHandler(this.ReHook_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(159, 29);
            this.Controls.Add(this.ReHook);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "JangoPlayer";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ReHook;

    }
}

