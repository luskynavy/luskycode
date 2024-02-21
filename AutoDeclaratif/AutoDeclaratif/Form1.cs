using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace AutoDeclaratif
{
    public partial class Form1 : Form
    {
        private const int DURATION_LINE = 4;
        private const int SEPARATOR_LINE = NUMBER_OF_LINES - 1;
        private const int NUMBER_OF_LINES = 6;
        private const int NUMBER_OF_WEEKS = 6;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            BindDataGridView(DateTime.Now);
        }

        /// <summary>
        /// Set data grid view for the date date
        /// </summary>
        /// <param name="date"></param>
        private void BindDataGridView(DateTime date)
        {
            //Set columns
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[10] {
                new DataColumn(" ", typeof(string)),
                new DataColumn("Lundi", typeof(string)),
                new DataColumn("Mardi", typeof(string)),
                new DataColumn("Mercredi", typeof(string)),
                new DataColumn("Jeudi", typeof(string)),
                new DataColumn("Vendredi", typeof(string)),
                new DataColumn("Samedi", typeof(string)),
                new DataColumn("Dimanche", typeof(string)),
                new DataColumn("Total", typeof(string)),
                new DataColumn("Moyenne",typeof(string)) }
            );
            dt.Columns[0].ReadOnly = true;
            dt.Columns["Total"].ReadOnly = true;
            dt.Columns["Moyenne"].ReadOnly = true;

            SetMonthData(dt, date);

            dataGridView1.DataSource = dt;

            //Disable sort for all columns
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            Font boldFont = new Font(dataGridView1[0, 0].InheritedStyle.Font, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = boldFont;

            //Set colors for read only cells and week ends

            dataGridView1.Columns[0].DefaultCellStyle.Font = boldFont;
            dataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Columns["Samedi"].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
            dataGridView1.Columns["Dimanche"].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
            dataGridView1.Columns["Total"].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Columns["Moyenne"].DefaultCellStyle.BackColor = Color.LightGray;

            for (int i = 0; i < NUMBER_OF_WEEKS; i++)
            {

                dataGridView1.Rows[i * NUMBER_OF_LINES].ReadOnly = true;

                dataGridView1.Rows[i * NUMBER_OF_LINES + DURATION_LINE].DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView1.Rows[i * NUMBER_OF_LINES + DURATION_LINE].ReadOnly = true;

                //No Separator line for last week
                if (i != NUMBER_OF_WEEKS - 1)
                {
                    dataGridView1.Rows[i * NUMBER_OF_LINES + SEPARATOR_LINE].DefaultCellStyle.BackColor = Color.White;
                    dataGridView1.Rows[i * NUMBER_OF_LINES + SEPARATOR_LINE].ReadOnly = true;
                    dataGridView1.Rows[i * NUMBER_OF_LINES + SEPARATOR_LINE].Height = dataGridView1.Rows[i * NUMBER_OF_LINES + SEPARATOR_LINE].Height * 2 / 3;
                }

                for (int j = 1; j < 10; j++)
                {
                    dataGridView1[j, i * NUMBER_OF_LINES].Style.BackColor = Color.FromArgb(255, 240, 240, 240);
                    dataGridView1[j, i * NUMBER_OF_LINES].Style.Font = boldFont;
                }
            }

            //Activate the double buffer to speed draw and resize
            typeof(DataGridView).InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               dataGridView1,
               new object[] { true });
        }

        /// <summary>
        /// Set the month data in the datatable  dt for the date date
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="date"></param>
        private static void SetMonthData(DataTable dt, DateTime date)
        {
            //Get first monday of the first week of the month
            DateTime today = date;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var firstMonday = firstDayOfMonth.AddDays(-((int)firstDayOfMonth.DayOfWeek) + 1);

            for (int i = 0; i < NUMBER_OF_WEEKS; i++)
            {
                //Build each day text
                DataRow dates = dt.NewRow();
                dates[0] = "";
                for (int day = 0; day < 7; day++)
                {
                    var dayInWeek = firstMonday.AddDays(day + i * 7);
                    dates[day + 1] = dayInWeek.ToString("dddd").Substring(0, 3) + " " + dayInWeek.ToString("dd/MM");
                }
                dates[8] = "Total";
                dates[9] = "Moyenne";

                //Set data for a week
                dt.Rows.Add(dates);
                dt.Rows.Add("Arrivée", "09:00", "09:20", "09:20", "09:20", "09:20", "", "", "", "");
                dt.Rows.Add("Pause", "00:20", "00:20", "00:40", "00:20", "00:20", "", "", "", "");
                dt.Rows.Add("Départ", "18:00", "18:20", "18:20", "18:20", "18:20", "", "", "", "");
                dt.Rows.Add("Durée", "09:00", "09:20", "09:20", "09:20", "09:20", "", "", "36,5", "7,33");

                //No Separator line for last week
                if (i != NUMBER_OF_WEEKS - 1)
                {
                    //Separator line
                    dt.Rows.Add();
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            BindDataGridView(dateTimePicker1.Value);
        }
    }
}