namespace ToastCSharp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ButtonSendToast = new Button();
            SuspendLayout();
            // 
            // ButtonSendToast
            // 
            ButtonSendToast.Location = new Point(277, 121);
            ButtonSendToast.Name = "ButtonSendToast";
            ButtonSendToast.Size = new Size(94, 29);
            ButtonSendToast.TabIndex = 0;
            ButtonSendToast.Text = "Send Toast";
            ButtonSendToast.UseVisualStyleBackColor = true;
            ButtonSendToast.Click += ButtonSendToast_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ButtonSendToast);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button ButtonSendToast;
    }
}
