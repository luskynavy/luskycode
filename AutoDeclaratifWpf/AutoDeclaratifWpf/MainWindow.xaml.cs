using System.Data;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

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

        private bool _isUpdating = false;

        private DateTime _firstDayOfMonth;
        private DateTime _firstMonday;
        private DateHoursDb _db;

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

            _isUpdating = true;

            dataGridView1.ItemsSource = dt.DefaultView;

            _isUpdating = false;

            //Disable sort for all columns
            foreach (var col in dataGridView1.Columns)
            {
                col.CanUserSort = false;
            }

            var boldFont = new Style();
            Setter fontWeightSetter = new Setter { Property = Control.FontWeightProperty, Value = FontWeights.Bold };
            boldFont.Setters.Add(fontWeightSetter);

            var lightGrayFont = new Style();
            //Setter lightGraySetter = new Setter { Property = Control.BackgroundProperty, Value = "LightGray" };
            //lightGrayFont.Setters.Add(lightGraySetter);

            dataGridView1.ColumnHeaderStyle = boldFont;

            //Set colors for read only cells and week ends

            dataGridView1.Columns[0].CellStyle = boldFont;
            dataGridView1.Columns[0].CellStyle = lightGrayFont;
            //dataGridView1.Columns[6/*"Samedi"*/].CellStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
            //dataGridView1.Columns[7/*"Dimanche"*/].CellStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
            dataGridView1.Columns[8/*"Total"*/].CellStyle = lightGrayFont;
            dataGridView1.Columns[9/*"Moyenne"*/].CellStyle = lightGrayFont;
            /*
            for (int i = 0; i < NUMBER_OF_WEEKS; i++)
            {
                dataGridView1.Rows Rows[i * NUMBER_OF_LINES].ReadOnly = true;

                dataGridView1.Rows Rows[i * NUMBER_OF_LINES + DURATION_LINE].CellStyle = lightGrayFont;
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
            */
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

        private void dateTimePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateTimePicker1.SelectedDate != null)
            {
                BindDataGridView(dateTimePicker1.SelectedDate.Value);
            }
        }

        /*
        private void dateTimePicker1_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            if (dateTimePicker1.DisplayMode == CalendarMode.Month)
            {
                dateTimePicker1.DisplayMode = CalendarMode.Year;
            }
        }
        */
    }
}