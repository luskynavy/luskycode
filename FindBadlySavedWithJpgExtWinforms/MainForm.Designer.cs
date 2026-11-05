namespace ImageMismatchFinder
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Button btnBrowse;

        private System.Windows.Forms.CheckBox chkSubdirectories;
        private System.Windows.Forms.Label lblWeeks;
        private System.Windows.Forms.NumericUpDown numWeeks;
        private System.Windows.Forms.Label lblWeeksHint;

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.CheckBox chkCopyFullPath;

        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDimensions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFolder;

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel lblTimer;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private System.Windows.Forms.ToolStripStatusLabel lblSelected;

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem menuOpenFolder;
        private System.Windows.Forms.ToolStripSeparator menuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuCopyNames;
        private System.Windows.Forms.ToolStripMenuItem menuCopyFullPaths;
        private System.Windows.Forms.ToolStripMenuItem menuSelectAll;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dgvCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();

            pnlTop = new System.Windows.Forms.Panel();
            lblDirectory = new System.Windows.Forms.Label();
            txtDirectory = new System.Windows.Forms.TextBox();
            btnBrowse = new System.Windows.Forms.Button();

            chkSubdirectories = new System.Windows.Forms.CheckBox();
            lblWeeks = new System.Windows.Forms.Label();
            numWeeks = new System.Windows.Forms.NumericUpDown();
            lblWeeksHint = new System.Windows.Forms.Label();

            btnSearch = new System.Windows.Forms.Button();
            btnStop = new System.Windows.Forms.Button();
            btnCopy = new System.Windows.Forms.Button();
            chkCopyFullPath = new System.Windows.Forms.CheckBox();

            dgvResults = new System.Windows.Forms.DataGridView();
            colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colRealType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDimensions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();

            statusStrip = new System.Windows.Forms.StatusStrip();
            lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            progressBar = new System.Windows.Forms.ToolStripProgressBar();
            lblTimer = new System.Windows.Forms.ToolStripStatusLabel();
            lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            lblSelected = new System.Windows.Forms.ToolStripStatusLabel();

            contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            menuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            menuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            menuCopyNames = new System.Windows.Forms.ToolStripMenuItem();
            menuCopyFullPaths = new System.Windows.Forms.ToolStripMenuItem();
            menuSelectAll = new System.Windows.Forms.ToolStripMenuItem();

            ((System.ComponentModel.ISupportInitialize)numWeeks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            pnlTop.SuspendLayout();
            statusStrip.SuspendLayout();
            contextMenu.SuspendLayout();
            SuspendLayout();

            // 
            // pnlTop
            // 
            pnlTop.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlTop.Controls.Add(lblDirectory);
            pnlTop.Controls.Add(txtDirectory);
            pnlTop.Controls.Add(btnBrowse);
            pnlTop.Controls.Add(chkSubdirectories);
            pnlTop.Controls.Add(lblWeeks);
            pnlTop.Controls.Add(numWeeks);
            pnlTop.Controls.Add(lblWeeksHint);
            pnlTop.Controls.Add(btnSearch);
            pnlTop.Controls.Add(btnStop);
            pnlTop.Controls.Add(btnCopy);
            pnlTop.Controls.Add(chkCopyFullPath);
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Location = new System.Drawing.Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new System.Drawing.Size(984, 115);
            pnlTop.TabIndex = 0;

            // 
            // lblDirectory
            // 
            lblDirectory.AutoSize = true;
            lblDirectory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDirectory.Location = new System.Drawing.Point(12, 14);
            lblDirectory.Name = "lblDirectory";
            lblDirectory.Size = new System.Drawing.Size(61, 15);
            lblDirectory.TabIndex = 0;
            lblDirectory.Text = "Directory:";

            // 
            // txtDirectory
            // 
            txtDirectory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtDirectory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            txtDirectory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            txtDirectory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtDirectory.Location = new System.Drawing.Point(79, 10);
            txtDirectory.Name = "txtDirectory";
            txtDirectory.Size = new System.Drawing.Size(785, 24);
            txtDirectory.TabIndex = 1;

            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnBrowse.Location = new System.Drawing.Point(870, 9);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(100, 26);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;

            // 
            // chkSubdirectories
            // 
            chkSubdirectories.AutoSize = true;
            chkSubdirectories.Checked = true;
            chkSubdirectories.CheckState = System.Windows.Forms.CheckState.Checked;
            chkSubdirectories.Font = new System.Drawing.Font("Segoe UI", 9F);
            chkSubdirectories.Location = new System.Drawing.Point(79, 44);
            chkSubdirectories.Name = "chkSubdirectories";
            chkSubdirectories.Size = new System.Drawing.Size(155, 19);
            chkSubdirectories.TabIndex = 3;
            chkSubdirectories.Text = "Search in subdirectories";
            chkSubdirectories.UseVisualStyleBackColor = true;

            // 
            // lblWeeks
            // 
            lblWeeks.AutoSize = true;
            lblWeeks.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblWeeks.Location = new System.Drawing.Point(260, 45);
            lblWeeks.Name = "lblWeeks";
            lblWeeks.Size = new System.Drawing.Size(126, 15);
            lblWeeks.TabIndex = 4;
            lblWeeks.Text = "Limit search by weeks:";

            // 
            // numWeeks
            // 
            numWeeks.Font = new System.Drawing.Font("Segoe UI", 9F);
            numWeeks.Location = new System.Drawing.Point(392, 42);
            numWeeks.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numWeeks.Name = "numWeeks";
            numWeeks.Size = new System.Drawing.Size(75, 23);
            numWeeks.TabIndex = 5;

            // 
            // lblWeeksHint
            // 
            lblWeeksHint.AutoSize = true;
            lblWeeksHint.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            lblWeeksHint.ForeColor = System.Drawing.Color.DimGray;
            lblWeeksHint.Location = new System.Drawing.Point(475, 46);
            lblWeeksHint.Name = "lblWeeksHint";
            lblWeeksHint.Size = new System.Drawing.Size(270, 13);
            lblWeeksHint.TabIndex = 6;
            lblWeeksHint.Text = "(0 = no limit, 1 = max 1 week old, 2 = max 2 weeks old...)";

            // 
            // btnSearch
            // 
            btnSearch.BackColor = System.Drawing.Color.FromArgb(13, 110, 253);
            btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnSearch.ForeColor = System.Drawing.Color.White;
            btnSearch.Location = new System.Drawing.Point(79, 74);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(110, 30);
            btnSearch.TabIndex = 7;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;

            // 
            // btnStop
            // 
            btnStop.Enabled = false;
            btnStop.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnStop.Location = new System.Drawing.Point(198, 74);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(90, 30);
            btnStop.TabIndex = 8;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;

            // 
            // btnCopy
            // 
            btnCopy.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnCopy.Location = new System.Drawing.Point(310, 74);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(160, 30);
            btnCopy.TabIndex = 9;
            btnCopy.Text = "Copy Selected Names";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;

            // 
            // chkCopyFullPath
            // 
            chkCopyFullPath.AutoSize = true;
            chkCopyFullPath.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            chkCopyFullPath.Location = new System.Drawing.Point(478, 81);
            chkCopyFullPath.Name = "chkCopyFullPath";
            chkCopyFullPath.Size = new System.Drawing.Size(118, 19);
            chkCopyFullPath.TabIndex = 10;
            chkCopyFullPath.Text = "Include full paths";
            chkCopyFullPath.UseVisualStyleBackColor = true;

            // 
            // dgvResults
            // 
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.AllowUserToOrderColumns = true;
            dgvResults.AllowUserToResizeRows = false;
            dgvCellStyle1.BackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            dgvResults.AlternatingRowsDefaultCellStyle = dgvCellStyle1;
            dgvResults.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dgvCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvCellStyle2.BackColor = System.Drawing.Color.FromArgb(238, 242, 246);
            dgvCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dgvCellStyle2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            dgvCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgvCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dgvCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvResults.ColumnHeadersDefaultCellStyle = dgvCellStyle2;
            dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                colName,
                colRealType,
                colSize,
                colDimensions,
                colDate,
                colFolder
            });
            dgvResults.ContextMenuStrip = contextMenu;
            dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvResults.EnableHeadersVisualStyles = false;
            dgvResults.GridColor = System.Drawing.Color.FromArgb(230, 233, 238);
            dgvResults.Location = new System.Drawing.Point(0, 115);
            dgvResults.MultiSelect = true;
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersVisible = false;
            dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvResults.Size = new System.Drawing.Size(984, 421);
            dgvResults.TabIndex = 1;
            dgvResults.CellDoubleClick += dgvResults_CellDoubleClick;
            dgvResults.SelectionChanged += dgvResults_SelectionChanged;
            dgvResults.KeyDown += dgvResults_KeyDown;

            // 
            // colName
            // 
            colName.DataPropertyName = "FileName";
            colName.HeaderText = "Name";
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.Width = 220;

            // 
            // colRealType
            // 
            colRealType.DataPropertyName = "RealType";
            colRealType.HeaderText = "Real File Type";
            colRealType.Name = "colRealType";
            colRealType.ReadOnly = true;
            colRealType.Width = 110;

            // 
            // colSize
            // 
            colSize.DataPropertyName = "FileSizeFormatted";
            colSize.HeaderText = "File Size";
            colSize.Name = "colSize";
            colSize.ReadOnly = true;
            colSize.Width = 110;

            // 
            // colDimensions
            // 
            colDimensions.DataPropertyName = "Dimensions";
            colDimensions.HeaderText = "Dimensions";
            colDimensions.Name = "colDimensions";
            colDimensions.ReadOnly = true;
            colDimensions.Width = 120;

            // 
            // colDate
            // 
            colDate.DataPropertyName = "DateFormatted";
            colDate.HeaderText = "Date";
            colDate.Name = "colDate";
            colDate.ReadOnly = true;
            colDate.Width = 150;

            // 
            // colFolder
            // 
            colFolder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colFolder.DataPropertyName = "Folder";
            colFolder.HeaderText = "Folder";
            colFolder.Name = "colFolder";
            colFolder.ReadOnly = true;

            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                lblStatus,
                progressBar,
                lblTimer,
                lblCount,
                lblSelected
            });
            statusStrip.Location = new System.Drawing.Point(0, 536);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(984, 25);
            statusStrip.TabIndex = 2;

            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(659, 20);
            lblStatus.Spring = true;
            lblStatus.Text = "Ready";
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // progressBar
            // 
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(120, 19);
            progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar.Visible = false;

            // 
            // lblTimer
            // 
            lblTimer.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblTimer.ForeColor = System.Drawing.Color.FromArgb(40, 40, 40);
            lblTimer.Name = "lblTimer";
            lblTimer.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblTimer.Size = new System.Drawing.Size(66, 20);
            lblTimer.Text = "00:00.0";

            // 
            // lblCount
            // 
            lblCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblCount.Name = "lblCount";
            lblCount.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            lblCount.Size = new System.Drawing.Size(107, 20);
            lblCount.Text = "0 mismatches";

            // 
            // lblSelected
            // 
            lblSelected.ForeColor = System.Drawing.Color.DimGray;
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new System.Drawing.Size(63, 20);
            lblSelected.Text = "0 selected";

            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                menuOpen,
                menuOpenFolder,
                menuSeparator1,
                menuCopyNames,
                menuCopyFullPaths,
                menuSelectAll
            });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new System.Drawing.Size(209, 120);

            // 
            // menuOpen
            // 
            menuOpen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            menuOpen.Name = "menuOpen";
            menuOpen.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Return;
            menuOpen.Size = new System.Drawing.Size(208, 22);
            menuOpen.Text = "Open Image";
            menuOpen.Click += menuOpen_Click;

            // 
            // menuOpenFolder
            // 
            menuOpenFolder.Name = "menuOpenFolder";
            menuOpenFolder.Size = new System.Drawing.Size(208, 22);
            menuOpenFolder.Text = "Open Containing Folder";
            menuOpenFolder.Click += menuOpenFolder_Click;

            // 
            // menuSeparator1
            // 
            menuSeparator1.Name = "menuSeparator1";
            menuSeparator1.Size = new System.Drawing.Size(205, 6);

            // 
            // menuCopyNames
            // 
            menuCopyNames.Name = "menuCopyNames";
            menuCopyNames.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            menuCopyNames.Size = new System.Drawing.Size(208, 22);
            menuCopyNames.Text = "Copy Name(s)";
            menuCopyNames.Click += menuCopyNames_Click;

            // 
            // menuCopyFullPaths
            // 
            menuCopyFullPaths.Name = "menuCopyFullPaths";
            menuCopyFullPaths.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.C;
            menuCopyFullPaths.Size = new System.Drawing.Size(208, 22);
            menuCopyFullPaths.Text = "Copy Full Path(s)";
            menuCopyFullPaths.Click += menuCopyFullPaths_Click;

            // 
            // menuSelectAll
            // 
            menuSelectAll.Name = "menuSelectAll";
            menuSelectAll.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;
            menuSelectAll.Size = new System.Drawing.Size(208, 22);
            menuSelectAll.Text = "Select All";
            menuSelectAll.Click += menuSelectAll_Click;

            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(984, 561);
            Controls.Add(dgvResults);
            Controls.Add(statusStrip);
            Controls.Add(pnlTop);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            MinimumSize = new System.Drawing.Size(750, 400);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Image Mismatch Finder";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)numWeeks).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
