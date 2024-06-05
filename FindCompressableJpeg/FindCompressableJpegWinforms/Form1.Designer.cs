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
            label1 = new Label();
            imagesPath = new TextBox();
            SelectPathButton = new Button();
            GetRatiosButton = new Button();
            label2 = new Label();
            treshold = new NumericUpDown();
            NameColumn = new DataGridViewTextBoxColumn();
            FileSize = new DataGridViewTextBoxColumn();
            Ratio = new DataGridViewTextBoxColumn();
            Dimensions = new DataGridViewTextBoxColumn();
            nbPixels = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)treshold).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { NameColumn, FileSize, Ratio, Dimensions, nbPixels });
            dataGridView1.Location = new Point(12, 74);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(776, 364);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
            dataGridView1.SortCompare += dataGridView1_SortCompare;
            dataGridView1.KeyDown += dataGridView1_KeyDown;
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
            imagesPath.Location = new Point(76, 12);
            imagesPath.Name = "imagesPath";
            imagesPath.Size = new Size(667, 23);
            imagesPath.TabIndex = 2;
            // 
            // SelectPathButton
            // 
            SelectPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SelectPathButton.Location = new Point(749, 12);
            SelectPathButton.Name = "SelectPathButton";
            SelectPathButton.Size = new Size(39, 23);
            SelectPathButton.TabIndex = 3;
            SelectPathButton.Text = "...";
            SelectPathButton.UseVisualStyleBackColor = true;
            SelectPathButton.Click += SelectPathButton_Click;
            // 
            // GetRatiosButton
            // 
            GetRatiosButton.Location = new Point(205, 41);
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
            label2.Size = new Size(51, 15);
            label2.TabIndex = 5;
            label2.Text = "Treshold";
            // 
            // treshold
            // 
            treshold.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            treshold.Location = new Point(76, 42);
            treshold.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            treshold.Name = "treshold";
            treshold.Size = new Size(120, 23);
            treshold.TabIndex = 7;
            treshold.Value = new decimal(new int[] { 300, 0, 0, 0 });
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(treshold);
            Controls.Add(label2);
            Controls.Add(GetRatiosButton);
            Controls.Add(SelectPathButton);
            Controls.Add(imagesPath);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "FindCompressableJpeg";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)treshold).EndInit();
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
        private NumericUpDown treshold;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn FileSize;
        private DataGridViewTextBoxColumn Ratio;
        private DataGridViewTextBoxColumn Dimensions;
        private DataGridViewTextBoxColumn nbPixels;
    }
}
