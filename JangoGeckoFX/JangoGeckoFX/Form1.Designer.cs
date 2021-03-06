﻿namespace JangoGeckoFX
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ReHook = new System.Windows.Forms.Button();
            this.toolTipReHook = new System.Windows.Forms.ToolTip(this.components);
            this.Home = new System.Windows.Forms.Button();
            this.update = new System.Windows.Forms.Timer(this.components);
            this.Dump = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ReHook
            // 
            this.ReHook.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ReHook.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ReHook.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ReHook.Location = new System.Drawing.Point(0, 0);
            this.ReHook.Name = "ReHook";
            this.ReHook.Size = new System.Drawing.Size(23, 23);
            this.ReHook.TabIndex = 0;
            this.ReHook.Text = "R";
            this.toolTipReHook.SetToolTip(this.ReHook, "Reinstall hook keys");
            this.ReHook.UseVisualStyleBackColor = false;
            this.ReHook.Click += new System.EventHandler(this.ReHook_Click);
            // 
            // Home
            // 
            this.Home.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Home.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Home.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Home.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Home.Location = new System.Drawing.Point(23, 0);
            this.Home.Name = "Home";
            this.Home.Size = new System.Drawing.Size(23, 23);
            this.Home.TabIndex = 0;
            this.Home.Text = "H";
            this.toolTipReHook.SetToolTip(this.Home, "Home");
            this.Home.UseVisualStyleBackColor = false;
            this.Home.Click += new System.EventHandler(this.Home_Click);
            // 
            // update
            // 
            this.update.Enabled = true;
            this.update.Interval = 1000;
            this.update.Tick += new System.EventHandler(this.update_Tick);
            // 
            // Dump
            // 
            this.Dump.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Dump.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Dump.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Dump.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Dump.Location = new System.Drawing.Point(46, 0);
            this.Dump.Name = "Dump";
            this.Dump.Size = new System.Drawing.Size(23, 23);
            this.Dump.TabIndex = 1;
            this.Dump.Text = "D";
            this.toolTipReHook.SetToolTip(this.Dump, "Dump");
            this.Dump.UseVisualStyleBackColor = false;
            this.Dump.Click += new System.EventHandler(this.Dump_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 321);
            this.Controls.Add(this.Dump);
            this.Controls.Add(this.Home);
            this.Controls.Add(this.ReHook);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ReHook;
        private System.Windows.Forms.ToolTip toolTipReHook;
        private System.Windows.Forms.Timer update;
        private System.Windows.Forms.Button Home;
        private System.Windows.Forms.Button Dump;
    }
}

