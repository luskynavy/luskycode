namespace FindCompressableJpegWinforms
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
			//Save values to ini file
			var myIni = new IniFile();
			myIni.Write("Path", imagesPath.Text);
			myIni.Write("Recursive", recursive.Checked.ToString());
			myIni.Write("SizeTreshold", sizeTreshold.Value.ToString());
			myIni.Write("RatioTreshold", ratioTreshold.Value.ToString());

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
			dataGridView1 = new DataGridView();
			NameColumn = new DataGridViewTextBoxColumn();
			FileSize = new DataGridViewTextBoxColumn();
			Ratio = new DataGridViewTextBoxColumn();
			Dimensions = new DataGridViewTextBoxColumn();
			nbPixels = new DataGridViewTextBoxColumn();
			label1 = new Label();
			imagesPath = new TextBox();
			SelectPathButton = new Button();
			GetRatiosButton = new Button();
			label2 = new Label();
			ratioTreshold = new NumericUpDown();
			sizeTreshold = new NumericUpDown();
			label3 = new Label();
			progressBar1 = new ProgressBar();
			recursive = new CheckBox();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)ratioTreshold).BeginInit();
			((System.ComponentModel.ISupportInitialize)sizeTreshold).BeginInit();
			SuspendLayout();
			// 
			// dataGridView1
			// 
			dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Columns.AddRange(new DataGridViewColumn[] { NameColumn, FileSize, Ratio, Dimensions, nbPixels });
			dataGridView1.Location = new Point(12, 74);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.Size = new Size(878, 364);
			dataGridView1.TabIndex = 0;
			dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
			dataGridView1.SortCompare += dataGridView1_SortCompare;
			dataGridView1.KeyDown += dataGridView1_KeyDown;
			// 
			// NameColumn
			// 
			NameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			NameColumn.HeaderText = "Name";
			NameColumn.Name = "NameColumn";
			NameColumn.ReadOnly = true;
			// 
			// FileSize
			// 
			FileSize.HeaderText = "File size";
			FileSize.Name = "FileSize";
			FileSize.ReadOnly = true;
			// 
			// Ratio
			// 
			Ratio.HeaderText = "Ratio";
			Ratio.Name = "Ratio";
			Ratio.ReadOnly = true;
			// 
			// Dimensions
			// 
			Dimensions.HeaderText = "Dimensions";
			Dimensions.Name = "Dimensions";
			Dimensions.ReadOnly = true;
			// 
			// nbPixels
			// 
			nbPixels.HeaderText = "Nb pixels";
			nbPixels.Name = "nbPixels";
			nbPixels.ReadOnly = true;
			nbPixels.Visible = false;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(12, 15);
			label1.Name = "label1";
			label1.Size = new Size(58, 15);
			label1.TabIndex = 1;
			label1.Text = "Jpeg path";
			// 
			// imagesPath
			// 
			imagesPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			imagesPath.Location = new Point(98, 12);
			imagesPath.Name = "imagesPath";
			imagesPath.Size = new Size(658, 23);
			imagesPath.TabIndex = 2;
			// 
			// SelectPathButton
			// 
			SelectPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			SelectPathButton.Location = new Point(762, 12);
			SelectPathButton.Name = "SelectPathButton";
			SelectPathButton.Size = new Size(39, 23);
			SelectPathButton.TabIndex = 3;
			SelectPathButton.Text = "...";
			SelectPathButton.UseVisualStyleBackColor = true;
			SelectPathButton.Click += SelectPathButton_Click;
			// 
			// GetRatiosButton
			// 
			GetRatiosButton.Location = new Point(503, 42);
			GetRatiosButton.Name = "GetRatiosButton";
			GetRatiosButton.Size = new Size(75, 23);
			GetRatiosButton.TabIndex = 4;
			GetRatiosButton.Text = "Get ratios";
			GetRatiosButton.UseVisualStyleBackColor = true;
			GetRatiosButton.Click += GetRatiosButton2_Click;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(12, 44);
			label2.Name = "label2";
			label2.Size = new Size(80, 15);
			label2.TabIndex = 5;
			label2.Text = "Ratio treshold";
			// 
			// ratioTreshold
			// 
			ratioTreshold.Increment = new decimal(new int[] { 100, 0, 0, 0 });
			ratioTreshold.Location = new Point(98, 42);
			ratioTreshold.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			ratioTreshold.Name = "ratioTreshold";
			ratioTreshold.Size = new Size(120, 23);
			ratioTreshold.TabIndex = 7;
			ratioTreshold.Value = new decimal(new int[] { 300, 0, 0, 0 });
			// 
			// sizeTreshold
			// 
			sizeTreshold.Increment = new decimal(new int[] { 100, 0, 0, 0 });
			sizeTreshold.Location = new Point(339, 42);
			sizeTreshold.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			sizeTreshold.Name = "sizeTreshold";
			sizeTreshold.Size = new Size(120, 23);
			sizeTreshold.TabIndex = 8;
			sizeTreshold.Value = new decimal(new int[] { 300, 0, 0, 0 });
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(260, 44);
			label3.Name = "label3";
			label3.Size = new Size(73, 15);
			label3.TabIndex = 9;
			label3.Text = "Size treshold";
			// 
			// progressBar1
			// 
			progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			progressBar1.Location = new Point(612, 42);
			progressBar1.Name = "progressBar1";
			progressBar1.Size = new Size(278, 23);
			progressBar1.TabIndex = 10;
			// 
			// recursive
			// 
			recursive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			recursive.Location = new Point(807, 14);
			recursive.Name = "recursive";
			recursive.Size = new Size(83, 19);
			recursive.TabIndex = 11;
			recursive.Text = "Recursive";
			recursive.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(902, 450);
			Controls.Add(recursive);
			Controls.Add(progressBar1);
			Controls.Add(label3);
			Controls.Add(sizeTreshold);
			Controls.Add(ratioTreshold);
			Controls.Add(label2);
			Controls.Add(GetRatiosButton);
			Controls.Add(SelectPathButton);
			Controls.Add(imagesPath);
			Controls.Add(label1);
			Controls.Add(dataGridView1);
			Name = "Form1";
			Text = "FindCompressableJpeg";
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)ratioTreshold).EndInit();
			((System.ComponentModel.ISupportInitialize)sizeTreshold).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private DataGridView dataGridView1;
        private Label label1;
        private TextBox imagesPath;
        private Button SelectPathButton;
        private Button GetRatiosButton;
        private Label label2;
        private NumericUpDown ratioTreshold;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn FileSize;
        private DataGridViewTextBoxColumn Ratio;
        private DataGridViewTextBoxColumn Dimensions;
        private DataGridViewTextBoxColumn nbPixels;
        private NumericUpDown sizeTreshold;
        private Label label3;
        private ProgressBar progressBar1;
		private CheckBox recursive;
	}
}
