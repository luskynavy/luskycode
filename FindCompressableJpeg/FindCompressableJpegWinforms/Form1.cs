using System.Diagnostics;
using System.Reflection;

namespace FindCompressableJpegWinforms
{
    public partial class Form1 : Form
    {
        private string dirString;

        public Form1()
        {
            InitializeComponent();
            InitDataGrid();

            //Activate the double buffer to speed draw and resize
            typeof(DataGridView).InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               dataGridView1,
               new object[] { true });
        }

        private void InitDataGrid()
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length <= 1)
            {
                dirString = ".";
            }
            else
            {
                dirString = args[1];
            }

            var dir = new DirectoryInfo(dirString);
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                if (FilterJpeg(file))
                {
                    int width, height;
                    GetBitmapHeightWidth(file, out height, out width);

                    var nbPixels = height * width / 1024;
                    var sizeFor1024Pixel = file.Length / (nbPixels != 0 ? nbPixels : 1);

                    string[] row = { file.Name, file.Length.ToString(), sizeFor1024Pixel.ToString(), $"{width} x {height}" };
                    dataGridView1.Rows.Add(row);
                }
            }

            dataGridView1.Sort(dataGridView1.Columns[2], System.ComponentModel.ListSortDirection.Descending);
        }

        /// <summary>
        /// Filter image name
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private static bool FilterJpeg(FileInfo f)
        {
            return f.Name.ToLower().EndsWith(".jpg") || f.Name.ToLower().EndsWith(".jpeg") || f.Name.ToLower().EndsWith(".jfif");
        }

        /// <summary>
        /// Get height and width with Bitmap
        /// </summary>
        /// <param name="f"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        private static void GetBitmapHeightWidth(FileInfo f, out int height, out int width)
        {
            /*using (var img = new Bitmap(f.FullName))
            {
                if (img != null)
                {
                    height = img.Height;
                    width = img.Width;
                }
            }*/

            //read only the header, no additionnal reference, faster
            using (var file = new FileStream(f.FullName, FileMode.Open, FileAccess.Read))
            {
                using (Image img = Image.FromStream(stream: file,
                                                    useEmbeddedColorManagement: false,
                                                    validateImageData: false))
                {
                    width = (int)img.PhysicalDimension.Width;
                    height = (int)img.PhysicalDimension.Height;
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var name = dataGridView1.CurrentRow.Cells[0].Value;

                ProcessStartInfo psi = new ProcessStartInfo(dirString + "\\" + name);
                psi.UseShellExecute = true;
                Process.Start(psi);
            }
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //Sort file size and ratio numerically
            if (e.Column.Index == 1 || e.Column.Index == 2)
            {
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;//pass by the default sorting
            }
        }
    }
}