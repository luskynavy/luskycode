namespace JangoPlayer2
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            buttonPlay = new Button();
            buttonNext = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(10, 6);
            webView21.Margin = new Padding(3, 2, 3, 2);
            webView21.Name = "webView21";
            webView21.Size = new Size(291, 595);
            webView21.Source = new Uri("https://www.jango.com", UriKind.Absolute);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // buttonPlay
            // 
            buttonPlay.Location = new Point(1, 4);
            buttonPlay.Margin = new Padding(3, 2, 3, 2);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(52, 22);
            buttonPlay.TabIndex = 1;
            buttonPlay.Text = "Play";
            buttonPlay.UseVisualStyleBackColor = true;
            buttonPlay.Click += ButtonPlay_Click;
            // 
            // buttonNext
            // 
            buttonNext.Location = new Point(59, 4);
            buttonNext.Margin = new Padding(3, 2, 3, 2);
            buttonNext.Name = "buttonNext";
            buttonNext.Size = new Size(50, 22);
            buttonNext.TabIndex = 2;
            buttonNext.Text = "Next";
            buttonNext.UseVisualStyleBackColor = true;
            buttonNext.Click += ButtonNext_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 5000;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(304, 611);
            Controls.Add(buttonNext);
            Controls.Add(buttonPlay);
            Controls.Add(webView21);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "JangoPlayer2";
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Button buttonPlay;
        private Button buttonNext;
        private System.Windows.Forms.Timer timer1;
    }
}
