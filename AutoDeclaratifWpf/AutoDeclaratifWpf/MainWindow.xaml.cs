﻿using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoDeclaratifWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int DEPARTURE_LINE = 2;
        private const int BREAK_LINE = 3;
        private const int DURATION_LINE = 4;
        private const int SEPARATOR_LINE = NUMBER_OF_LINES - 1;
        private const int NUMBER_OF_LINES = 6;
        private const int NUMBER_OF_WEEKS = 6 - 1;

        private DateTime _firstDayOfMonth;
        private DateTime _firstMonday;
        private DateHoursDb? _db;
        private bool _isinEditMode = false;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MyWindow_Loaded;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dateTimePicker1.SelectedDate = DateTime.Now;

            BindDataGridView(DateTime.Now);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dateTimePicker1.SelectedDate != null)
            {
                dateTimePicker1.SelectedDate = dateTimePicker1.SelectedDate.Value.AddMonths(-1);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (dateTimePicker1.SelectedDate != null)
            {
                dateTimePicker1.SelectedDate = dateTimePicker1.SelectedDate.Value.AddMonths(1);
            }
        }

        private void BindDataGridView(DateTime date)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[10] {
                new DataColumn("Type", typeof(string)),
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

            dataGridView1.ItemsSource = dt.DefaultView;

            //Disable sort for all columns
            foreach (var col in dataGridView1.Columns)
            {
                col.CanUserSort = false;
            }

            //Create styles/setters
            var boldFont = new Style();
            var boldFontWeightSetter = new Setter { Property = Control.FontWeightProperty, Value = FontWeights.Bold };
            boldFont.Setters.Add(boldFontWeightSetter);

            var lightGrayFont = new Style();
            var lightGraySetter = new Setter { Property = Control.BackgroundProperty, Value = Brushes.LightGray };
            lightGrayFont.Setters.Add(lightGraySetter);

            var whiteFont = new Style();
            var whiteSetter = new Setter { Property = Control.BackgroundProperty, Value = Brushes.White };
            whiteFont.Setters.Add(whiteSetter);

            var lightGrayFont2 = new Style();
            var lightGraySetter2 = new Setter { Property = Control.BackgroundProperty, Value = new SolidColorBrush(Color.FromRgb(240, 240, 240)) };
            lightGrayFont2.Setters.Add(lightGraySetter2);

            var boldLigtGrayFont = new Style();
            boldLigtGrayFont.Setters.Add(boldFontWeightSetter);
            boldLigtGrayFont.Setters.Add(lightGraySetter);

            //Set colors for read only cells and week ends
            dataGridView1.Columns[0].CellStyle = boldLigtGrayFont;
            dataGridView1.Columns[6/*"Samedi"*/].CellStyle = lightGrayFont2;
            dataGridView1.Columns[7/*"Dimanche"*/].CellStyle = lightGrayFont2;
            dataGridView1.Columns[8/*"Total"*/].CellStyle = lightGrayFont;
            dataGridView1.Columns[9/*"Moyenne"*/].CellStyle = lightGrayFont;

            for (int i = 0; i < NUMBER_OF_WEEKS; i++)
            {
                dataGridView1.GetRow(i * NUMBER_OF_LINES).Style = boldLigtGrayFont;

                //No Separator line for last week
                if (i != NUMBER_OF_WEEKS - 1)
                {
                    dataGridView1.GetRow(i * NUMBER_OF_LINES + DURATION_LINE).Style = lightGrayFont;
                    dataGridView1.GetRow(i * NUMBER_OF_LINES + SEPARATOR_LINE).Style = whiteFont; //dont work, overriden by column color
                    dataGridView1.GetRow(i * NUMBER_OF_LINES + SEPARATOR_LINE).Height = dataGridView1.GetRow(i * NUMBER_OF_LINES + SEPARATOR_LINE).ActualHeight * 2 / 3;
                }
            }
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
                    _db?.DeleteDay(dayDateHours.Date);
                }
            }
        }

        private void dateTimePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateTimePicker1.SelectedDate != null)
            {
                BindDataGridView(dateTimePicker1.SelectedDate.Value);
            }
        }

        private void dataGridView1_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            _isinEditMode = true;

            var currentRowIndex = dataGridView1.Items.IndexOf(dataGridView1.CurrentItem);
            if (IsRowReadOnly(currentRowIndex))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// True if row with this row index is read only
        /// </summary>
        /// <param name="currentRowIndex"></param>
        /// <returns></returns>
        private static bool IsRowReadOnly(int currentRowIndex)
        {
            return currentRowIndex % NUMBER_OF_LINES == 0 ||
                            currentRowIndex % NUMBER_OF_LINES == DURATION_LINE ||
                            currentRowIndex % NUMBER_OF_LINES == SEPARATOR_LINE;
        }

        private void dataGridView1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            _isinEditMode = false;

            var newVal = ((TextBox)e.EditingElement).Text;

            string pattern = @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
            Match m = Regex.Match(newVal, pattern);

            //Cancel change if not right format
            if (!string.IsNullOrEmpty(newVal) && !m.Success)
            {
                var previousValue = ((DataRowView)e.Row.Item).Row.ItemArray[e.Column.DisplayIndex];

                ((TextBox)e.EditingElement).Text = previousValue?.ToString();
            }
            //Update
            else
            {
                //Add 0 at begining if needed
                if (newVal.ToString().Length == 4)
                {
                    newVal = "0" + newVal;
                    ((TextBox)e.EditingElement).Text = newVal;
                }

                var currentColumnIndex = e.Column.DisplayIndex;
                var currentRowIndex = dataGridView1.Items.IndexOf(dataGridView1.SelectedItem);

                UpdateDbAndDuration(newVal, currentColumnIndex, currentRowIndex);
            }
        }

        /// <summary>
        /// Update db and refresh display
        /// </summary>
        /// <param name="newVal"></param>
        /// <param name="currentColumnIndex"></param>
        /// <param name="currentRowIndex"></param>
        private void UpdateDbAndDuration(string newVal, int currentColumnIndex, int currentRowIndex)
        {
            //Find day to modify
            var dayToModify = _firstMonday.AddDays(currentRowIndex / NUMBER_OF_LINES * 7 + currentColumnIndex - 1);

            var dateHours = _db?.Get(dayToModify).FirstOrDefault();

            //Create dateHours if not found
            if (dateHours == null)
            {
                dateHours = new DateHours { Date = dayToModify };
            }

            //Find time to modify
            var row = currentRowIndex % NUMBER_OF_LINES + 1;

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
            _db?.UpdateOrInsert(dateHours);

            //Refresh data grid view
            UpdateDuration(dateHours, currentColumnIndex, currentRowIndex);
        }

        /// <summary>
        /// Update duration
        /// </summary>
        /// <param name="dayDatehours">date hours for computing duration</param>
        /// <param name="columnIndex">column index of modified cell</param>
        /// <param name="rowIndex">row index of modified cell</param>
        private void UpdateDuration(DateHours dayDatehours, int columnIndex, int rowIndex)
        {
            //Get duration row
            var rowDurationIndex = rowIndex - rowIndex % NUMBER_OF_LINES + DURATION_LINE;
            var rowDuration = dataGridView1.GetRow(rowDurationIndex);

            if (!string.IsNullOrEmpty(dayDatehours.Departure) &&
                        !string.IsNullOrEmpty(dayDatehours.Arrival) &&
                        !string.IsNullOrEmpty(dayDatehours.Break))
            {
                //Compute duration
                var duration = (TimeSpan.Parse(dayDatehours.Departure) -
                            TimeSpan.Parse(dayDatehours.Arrival) -
                            TimeSpan.Parse(dayDatehours.Break)).TotalHours;

                //Update duration cell
                UpdateCell(columnIndex, rowDuration, duration);
            }
            else
            {
                //Update duration cell
                UpdateCell(columnIndex, rowDuration, null);
            }

            //Get week days
            var nbDayToMonday = (int)dayDatehours.Date.DayOfWeek - 1;
            if (nbDayToMonday == -1)
            {
                nbDayToMonday = 6;
            }
            var dateMonday = dayDatehours.Date.AddDays(-nbDayToMonday);
            var dateSunday = dateMonday.AddDays(6);

            var week = _db?.Get(dateMonday, dateSunday);

            //Compute total duration of week
            var totalDuration = 0.0;
            var ndDaysWithFullData = 0;
            foreach (var day in week)
            {
                if (!string.IsNullOrEmpty(day.Departure) &&
                    !string.IsNullOrEmpty(day.Arrival) &&
                    !string.IsNullOrEmpty(day.Break))
                {
                    //Compute duration
                    var durationDay = (TimeSpan.Parse(day.Departure) -
                                TimeSpan.Parse(day.Arrival) -
                                TimeSpan.Parse(day.Break)).TotalHours;

                    totalDuration += durationDay;
                    ndDaysWithFullData++;
                }
            }

            //Set total duration
            UpdateCell(8, rowDuration, totalDuration);

            //Compute average duration
            var averageDuration = ndDaysWithFullData != 0 ? Math.Round(totalDuration / ndDaysWithFullData, 2) : 0;

            //Set average duration
            UpdateCell(9, rowDuration, averageDuration);
        }

        /// <summary>
        /// Update cell in datagrid
        /// </summary>
        /// <param name="columnIndex">column index of modified cell</param>
        /// <param name="row">row of modified cell</param>
        /// <param name="value"></param>
        private void UpdateCell(int columnIndex, DataGridRow row, double? value)
        {
            var cell = dataGridView1.GetCell(row, columnIndex);
            if (cell != null)
            {
                var textBlock = (TextBlock)cell.Content;
                if (value != null)
                {
                    textBlock.Text = Math.Round((double)value, 2).ToString();
                }
                else
                {
                    textBlock.Text = "";
                }
            }
        }

        private void dataGridView1_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                if (_isinEditMode == false)
                {
                    var currentColumnIndex = dataGridView1.CurrentColumn.DisplayIndex;
                    var currentRowIndex = dataGridView1.Items.IndexOf(dataGridView1.CurrentItem);
                    var rowSelected = dataGridView1.GetRow(currentRowIndex);

                    //only for not read only cells
                    if (!dataGridView1.CurrentColumn.IsReadOnly && !IsRowReadOnly(currentRowIndex))
                    {
                        var cellDuration = dataGridView1.GetCell(rowSelected, currentColumnIndex);
                        if (cellDuration != null)
                        {
                            var textBlock = (TextBlock)cellDuration.Content;
                            textBlock.Text = "";

                            UpdateDbAndDuration("", currentColumnIndex, currentRowIndex);
                        }
                    }
                }
            }
        }
    }
}