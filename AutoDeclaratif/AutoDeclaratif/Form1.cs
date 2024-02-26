using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoDeclaratif
{
    public partial class Form1 : Form
    {
        private const int DEPARTURE_LINE = 2;
        private const int BREAK_LINE = 3;
        private const int DURATION_LINE = 4;
        private const int SEPARATOR_LINE = NUMBER_OF_LINES - 1;
        private const int NUMBER_OF_LINES = 6;
        private const int NUMBER_OF_WEEKS = 6 - 1;

        private bool _isDeleting = false;

        private DateTime _firstDayOfMonth;
        private DateTime _firstMonday;
        private DateHoursDb _db;

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
        private void SetMonthData(DataTable dt, DateTime date)
        {
            _db = new DateHoursDb("Data Source=Hours.sqlite");

            //Get first monday of the first week of the month
            _firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

            //If first day of month is a sunday
            if (_firstDayOfMonth.DayOfWeek == DayOfWeek.Sunday)
            {
                //Go to next monday, since Sunday is rarely used
                _firstMonday = _firstDayOfMonth.AddDays(1);
            }
            else
            {
                //Remove one week and add a day for monday since sunday is number 0
                _firstMonday = _firstDayOfMonth.AddDays(-((int)_firstDayOfMonth.DayOfWeek) + 1);
            }

            //Get all the weeks
            var dateHours = _db.Get(_firstMonday, _firstMonday.AddDays(7 * NUMBER_OF_WEEKS - 1));

            //Clean DateHours without data
            DeleteEmptyDateHours(dateHours);

            //Build each week
            for (int i = 0; i < NUMBER_OF_WEEKS; i++)
            {
                //Create each line for current week
                DataRow dates = dt.NewRow();
                DataRow arrivals = dt.NewRow();
                DataRow breaks = dt.NewRow();
                DataRow departures = dt.NewRow();
                DataRow durations = dt.NewRow();

                dates[0] = "";
                arrivals[0] = "Arrivée";
                breaks[0] = "Pause";
                departures[0] = "Départ";
                durations[0] = "Durée";

                var total = 0.0;
                var ndDaysWithFullData = 0;

                //Build each day text
                for (int day = 0; day < 7; day++)
                {
                    var dayInWeek = _firstMonday.AddDays(day + i * 7);
                    var dayDatehours = dateHours.FirstOrDefault(d => d.Date == dayInWeek);

                    dates[day + 1] = dayInWeek.ToString("dddd").Substring(0, 3) + " " + dayInWeek.ToString("dd/MM");
                    arrivals[day + 1] = dayDatehours?.Arrival;
                    breaks[day + 1] = dayDatehours?.Break;
                    departures[day + 1] = dayDatehours?.Departure;

                    //Compute duration for the day and updated week total
                    if (dayDatehours != null &&
                        !string.IsNullOrEmpty(dayDatehours.Departure) &&
                        !string.IsNullOrEmpty(dayDatehours.Arrival) &&
                        !string.IsNullOrEmpty(dayDatehours.Break))
                    {
                        var duration = (TimeSpan.Parse(dayDatehours.Departure) -
                            TimeSpan.Parse(dayDatehours.Arrival) -
                            TimeSpan.Parse(dayDatehours.Break)).TotalHours;
                        durations[day + 1] = Math.Round(duration, 2);
                        total += duration;
                        ndDaysWithFullData++;
                    }
                    else
                    {
                        durations[day + 1] = "";
                    }
                }

                dates[8] = "Total";
                dates[9] = "Moyenne";

                arrivals[8] = "";
                arrivals[9] = "";

                breaks[8] = "";
                breaks[9] = "";

                departures[8] = "";
                departures[9] = "";

                durations[8] = Math.Round(total, 2);
                //Compute average day time
                durations[9] = ndDaysWithFullData != 0 ? Math.Round(total / ndDaysWithFullData, 2) : 0;

                //Set data for a week
                dt.Rows.Add(dates);
                dt.Rows.Add(arrivals);
                dt.Rows.Add(breaks);
                dt.Rows.Add(departures);
                dt.Rows.Add(durations);

                //No Separator line for last week
                if (i != NUMBER_OF_WEEKS - 1)
                {
                    //Separator line
                    dt.Rows.Add();
                }
            }
        }

        /// <summary>
        /// Delete emtpy DateHours from db
        /// </summary>
        /// <param name="dateHours"></param>
        private void DeleteEmptyDateHours(IEnumerable<DateHours> dateHours)
        {
            foreach (var dayDateHours in dateHours)
            {
                if (string.IsNullOrEmpty(dayDateHours.Departure) &&
                        string.IsNullOrEmpty(dayDateHours.Arrival) &&
                        string.IsNullOrEmpty(dayDateHours.Break))
                {
                    _db.DeleteDay(dayDateHours.Date);
                }
            }
        }

        /// <summary>
        /// Go to wanted month
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            BindDataGridView(dateTimePicker1.Value);
        }

        /// <summary>
        /// Go to previous month
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Previous_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddMonths(-1);
        }

        /// <summary>
        /// Go to next month
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddMonths(1);
        }

        /// <summary>
        /// Called when cell value is changed after been validated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var newVal = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();

            //Add 0 at begining if needed
            if (newVal.ToString().Length == 4)
            {
                newVal = "0" + newVal;
            }

            //Find day to modify
            var dayToModify = _firstMonday.AddDays(e.RowIndex / NUMBER_OF_LINES * 7 + e.ColumnIndex - 1);

            var dateHours = _db.Get(dayToModify).FirstOrDefault();

            //Create dateHours if not found
            if (dateHours == null)
            {
                dateHours = new DateHours { Date = dayToModify };
            }

            //Find time to modify
            var row = e.RowIndex % NUMBER_OF_LINES + 1;

            switch (row)
            {
                case DEPARTURE_LINE:
                    dateHours.Arrival = newVal;
                    break;

                case BREAK_LINE:
                    dateHours.Break = newVal;
                    break;

                case DURATION_LINE:
                    dateHours.Departure = newVal;
                    break;

                default:
                    break;
            }

            //Update dateHours
            _db.UpdateOrInsert(dateHours);

            //No refresh during delete
            if (!_isDeleting)
            {
                //Refresh data grid view
                BindDataGridView(_firstDayOfMonth);

                //Reposition the current cell
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            }
        }

        /// <summary>
        /// Called to validate cell change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string pattern = @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
            Match m = Regex.Match(e.FormattedValue.ToString(), pattern);

            //Cancel change if not right format
            if (!m.Success)
            {
                DataGridView dgv = (DataGridView)sender;
                dgv.CancelEdit();
            }
        }

        /// <summary>
        /// Manage keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            //Empty selected cells on delete key if cells are note readonly
            if (e.KeyCode == Keys.Delete)
            {
                if (!dataGridView1.CurrentCell.IsInEditMode)
                {
                    _isDeleting = true;

                    foreach (DataGridViewCell selected_cell in dataGridView1.SelectedCells)
                    {
                        if (!selected_cell.ReadOnly)
                        {
                            selected_cell.Value = "";
                        }
                    }

                    _isDeleting = false;

                    //Save current cell position
                    var savedRow = dataGridView1.CurrentCell.RowIndex;
                    var savedColumn = dataGridView1.CurrentCell.ColumnIndex;

                    //Refresh data grid view
                    BindDataGridView(_firstDayOfMonth);

                    //Reposition the current cell
                    dataGridView1.CurrentCell = dataGridView1[savedColumn, savedRow];
                }
            }
        }
    }
}