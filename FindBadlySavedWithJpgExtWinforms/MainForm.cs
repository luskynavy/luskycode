using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageMismatchFinder
{
    public partial class MainForm : Form
    {
        private static readonly HashSet<string> TargetExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".jfif"
        };

        private readonly string _iniPath;
        private readonly SortableBindingList<ImageFileItem> _resultsList = new();
        private CancellationTokenSource? _cts;
        private bool _isSearching;

        public MainForm()
        {
            InitializeComponent();

            _iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");

            // Setup DataGridView
            dgvResults.AutoGenerateColumns = false;
            dgvResults.DataSource = _resultsList;

            // Enable double buffering for smooth DataGridView scrolling
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null,
                dgvResults,
                new object[] { true }
            );
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            LoadSettings();
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isSearching)
            {
                _cts?.Cancel();
            }
        }

        #region INI Settings

        private void LoadSettings()
        {
            try
            {
                var ini = new IniFile(_iniPath);

                string defaultDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                string savedPath = ini.GetValue("Settings", "SearchPath", defaultDir);
                txtDirectory.Text = string.IsNullOrWhiteSpace(savedPath) ? defaultDir : savedPath;

                chkSubdirectories.Checked = ini.GetBool("Settings", "SearchSubdirectories", true);

                int weeks = ini.GetInt("Settings", "WeeksFilter", 0);
                numWeeks.Value = Math.Clamp(weeks, (int)numWeeks.Minimum, (int)numWeeks.Maximum);

                chkCopyFullPath.Checked = ini.GetBool("Settings", "CopyFullPath", false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading settings: {ex.Message}");
            }
        }

        private void SaveSettings()
        {
            try
            {
                var ini = new IniFile(_iniPath);
                ini.SetValue("Settings", "SearchPath", txtDirectory.Text.Trim());
                ini.SetBool("Settings", "SearchSubdirectories", chkSubdirectories.Checked);
                ini.SetInt("Settings", "WeeksFilter", (int)numWeeks.Value);
                ini.SetBool("Settings", "CopyFullPath", chkCopyFullPath.Checked);
                ini.Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        #endregion

        #region Button & Control Events

        private void btnBrowse_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Select folder to search for mismatched image files";
            dialog.UseDescriptionForTitle = true;

            if (!string.IsNullOrWhiteSpace(txtDirectory.Text) && Directory.Exists(txtDirectory.Text))
            {
                dialog.InitialDirectory = txtDirectory.Text;
            }

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                txtDirectory.Text = dialog.SelectedPath;
            }
        }

        private async void btnSearch_Click(object? sender, EventArgs e)
        {
            if (_isSearching)
                return;

            string searchPath = txtDirectory.Text.Trim();
            if (string.IsNullOrEmpty(searchPath) || !Directory.Exists(searchPath))
            {
                MessageBox.Show(this, "The specified directory does not exist. Please select a valid folder.",
                    "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDirectory.Focus();
                return;
            }

            // Save settings whenever search is launched
            SaveSettings();

            _isSearching = true;
            btnSearch.Enabled = false;
            btnStop.Enabled = true;
            btnBrowse.Enabled = false;
            txtDirectory.ReadOnly = true;
            chkSubdirectories.Enabled = false;
            numWeeks.Enabled = false;
            progressBar.Visible = true;

            _resultsList.Clear();
            lblStatus.Text = "Starting search...";
            lblCount.Text = "0 mismatches";

            bool recursive = chkSubdirectories.Checked;
            int weeksLimit = (int)numWeeks.Value;

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            var progress = new Progress<SearchProgressUpdate>(update =>
            {
                if (update.NewItem != null)
                {
                    _resultsList.Add(update.NewItem);
                }

                lblCount.Text = $"{_resultsList.Count} mismatch{(_resultsList.Count == 1 ? "" : "es")}";
                lblStatus.Text = $"Scanned: {update.ScannedCount:N0} files | In folder: {update.CurrentDirectory}";
            });

            try
            {
                await Task.Run(() => PerformSearch(searchPath, recursive, weeksLimit, progress, token), token);

                if (!token.IsCancellationRequested)
                {
                    lblStatus.Text = $"Search completed. Found {_resultsList.Count} mismatched image{(_resultsList.Count == 1 ? "" : "s")}.";
                }
            }
            catch (OperationCanceledException)
            {
                lblStatus.Text = $"Search stopped. Displaying {_resultsList.Count} result{(_resultsList.Count == 1 ? "" : "s")} found so far.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error during search: {ex.Message}";
                MessageBox.Show(this, $"An error occurred during search:\n{ex.Message}", "Search Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isSearching = false;
                btnSearch.Enabled = true;
                btnStop.Enabled = false;
                btnBrowse.Enabled = true;
                txtDirectory.ReadOnly = false;
                chkSubdirectories.Enabled = true;
                numWeeks.Enabled = true;
                progressBar.Visible = false;

                _cts?.Dispose();
                _cts = null;
            }
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            if (_isSearching && _cts != null && !_cts.IsCancellationRequested)
            {
                lblStatus.Text = "Stopping search...";
                _cts.Cancel();
            }
        }

        private void btnCopy_Click(object? sender, EventArgs e)
        {
            CopySelectedItems(chkCopyFullPath.Checked);
        }

        private void dgvResults_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvResults.Rows.Count)
            {
                if (dgvResults.Rows[e.RowIndex].DataBoundItem is ImageFileItem item)
                {
                    OpenFile(item.FilePath);
                }
            }
        }

        private void dgvResults_SelectionChanged(object? sender, EventArgs e)
        {
            int selectedCount = GetSelectedRows().Count;
            lblSelected.Text = $"{selectedCount} selected";
        }

        private void dgvResults_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                var selected = GetSelectedItems();
                if (selected.Count > 0)
                {
                    OpenFile(selected[0].FilePath);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                CopySelectedItems(chkCopyFullPath.Checked);
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                dgvResults.SelectAll();
            }
        }

        #endregion

        #region Context Menu Events

        private void menuOpen_Click(object? sender, EventArgs e)
        {
            var selected = GetSelectedItems();
            if (selected.Count > 0)
            {
                OpenFile(selected[0].FilePath);
            }
        }

        private void menuOpenFolder_Click(object? sender, EventArgs e)
        {
            var selected = GetSelectedItems();
            if (selected.Count > 0)
            {
                OpenContainingFolder(selected[0].FilePath);
            }
        }

        private void menuCopyNames_Click(object? sender, EventArgs e)
        {
            CopySelectedItems(fullPath: false);
        }

        private void menuCopyFullPaths_Click(object? sender, EventArgs e)
        {
            CopySelectedItems(fullPath: true);
        }

        private void menuSelectAll_Click(object? sender, EventArgs e)
        {
            dgvResults.SelectAll();
        }

        #endregion

        #region Search Logic

        private class SearchProgressUpdate
        {
            public int ScannedCount { get; set; }
            public string CurrentDirectory { get; set; } = string.Empty;
            public ImageFileItem? NewItem { get; set; }
        }

        private void PerformSearch(
            string rootDirectory,
            bool recursive,
            int weeksLimit,
            IProgress<SearchProgressUpdate> progress,
            CancellationToken token)
        {
            DateTime? cutoffDate = null;
            if (weeksLimit > 0)
            {
                cutoffDate = DateTime.Now.AddDays(-7.0 * weeksLimit);
            }

            var dirsToProcess = new Queue<string>();
            dirsToProcess.Enqueue(rootDirectory);

            int scannedCount = 0;
            var stopwatch = Stopwatch.StartNew();

            while (dirsToProcess.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                string currentDir = dirsToProcess.Dequeue();

                // Report directory progress periodically
                if (stopwatch.ElapsedMilliseconds > 250)
                {
                    progress.Report(new SearchProgressUpdate
                    {
                        ScannedCount = scannedCount,
                        CurrentDirectory = currentDir
                    });
                    stopwatch.Restart();
                }

                // If recursive, discover child directories
                if (recursive)
                {
                    try
                    {
                        var subDirs = Directory.EnumerateDirectories(currentDir);
                        foreach (var subDir in subDirs)
                        {
                            token.ThrowIfCancellationRequested();
                            dirsToProcess.Enqueue(subDir);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Skip folders without permission
                    }
                    catch (DirectoryNotFoundException)
                    {
                        // Skip deleted folders
                    }
                    catch (Exception)
                    {
                        // Skip any other inaccessible folders
                    }
                }

                // Enumerate files in current directory
                IEnumerable<string> files;
                try
                {
                    files = Directory.EnumerateFiles(currentDir);
                }
                catch
                {
                    continue;
                }

                foreach (var filePath in files)
                {
                    token.ThrowIfCancellationRequested();

                    string ext = Path.GetExtension(filePath);
                    if (!TargetExtensions.Contains(ext))
                        continue;

                    FileInfo fileInfo;
                    try
                    {
                        fileInfo = new FileInfo(filePath);
                        if (!fileInfo.Exists)
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    // Check weeks limit filter
                    if (cutoffDate.HasValue && fileInfo.LastWriteTime < cutoffDate.Value)
                    {
                        continue;
                    }

                    scannedCount++;

                    // Magic header detection
                    var imageInfo = ImageFormatDetector.Detect(filePath);

                    // If it is a real JPEG, it is NOT a mismatch.
                    if (imageInfo.Format == DetectedImageFormat.Jpeg)
                    {
                        continue;
                    }

                    // If it is WebP, PNG, AVIF, BMP (or another image like GIF/TIFF)
                    // It is a mismatch!
                    if (imageInfo.Format != DetectedImageFormat.Unknown)
                    {
                        var item = new ImageFileItem
                        {
                            FileName = fileInfo.Name,
                            FilePath = fileInfo.FullName,
                            Folder = fileInfo.DirectoryName ?? string.Empty,
                            RealType = imageInfo.FormatName,
                            FileSize = fileInfo.Length,
                            Width = imageInfo.Width,
                            Height = imageInfo.Height,
                            DateModified = fileInfo.LastWriteTime
                        };

                        progress.Report(new SearchProgressUpdate
                        {
                            ScannedCount = scannedCount,
                            CurrentDirectory = currentDir,
                            NewItem = item
                        });
                    }
                }
            }

            // Final progress update
            progress.Report(new SearchProgressUpdate
            {
                ScannedCount = scannedCount,
                CurrentDirectory = "Finished"
            });
        }

        #endregion

        #region Helper Actions

        private List<DataGridViewRow> GetSelectedRows()
        {
            var rows = new HashSet<DataGridViewRow>();

            foreach (DataGridViewCell cell in dgvResults.SelectedCells)
            {
                if (cell.OwningRow != null)
                {
                    rows.Add(cell.OwningRow);
                }
            }

            return rows.OrderBy(r => r.Index).ToList();
        }

        private List<ImageFileItem> GetSelectedItems()
        {
            var items = new List<ImageFileItem>();
            foreach (var row in GetSelectedRows())
            {
                if (row.DataBoundItem is ImageFileItem item)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        private void CopySelectedItems(bool fullPath)
        {
            var selectedItems = GetSelectedItems();
            if (selectedItems.Count == 0)
            {
                MessageBox.Show(this, "Please select one or more lines in the table to copy.",
                    "No Lines Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var lines = selectedItems.Select(item => fullPath ? item.FilePath : item.FileName);
            string clipboardText = string.Join(Environment.NewLine, lines);

            try
            {
                Clipboard.SetText(clipboardText);
                lblStatus.Text = $"Copied {selectedItems.Count} {(fullPath ? "full path" : "original name")}{(selectedItems.Count == 1 ? "" : "s")} to clipboard.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Could not copy to clipboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Could not launch file:\n{ex.Message}", "Launch Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "The specified file was not found on disk.", "File Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OpenContainingFolder(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = $"/select,\"{filePath}\"",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Could not open folder:\n{ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}
