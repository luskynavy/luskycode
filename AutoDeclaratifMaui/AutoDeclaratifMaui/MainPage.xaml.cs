using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace AutoDeclaratifMaui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //akgul.Maui.DataGrid
            InitAkgulDataGrid();

            //UraniumUI DataGrid
            InitUraniumUiDataGrid();

            BindingContext = this;
            InitializeComponent();
        }

        #region akgul.Maui.DataGrid

        private readonly HttpClient httpClient = new();

        public bool IsRefreshing { get; set; }
        public ObservableCollection<Monkey> Items { get; set; } = new();
        public Command RefreshCommand { get; set; }
        public Monkey SelectedItem { get; set; }

        private void InitAkgulDataGrid()
        {
            RefreshCommand = new Command(async () =>
            {
                // Simulate delay
                await Task.Delay(2000);
                await LoadData();
                IsRefreshing = false;
                OnPropertyChanged(nameof(IsRefreshing));
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(2000);
            await LoadData();
        }

        private async Task LoadData()
        {
            var data = await httpClient.GetFromJsonAsync<Monkey[]>("https://montemagno.com/monkeys.json");

            Items.Clear();

            foreach (Monkey monkey in data)
            {
                Items.Add(monkey);
            }
        }

        public class Monkey
        {
            public string Name { get; set; }
            public string Location { get; set; }
            public string Details { get; set; }
            public string Image { get; set; }
            public int Population { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        #endregion akgul.Maui.DataGrid

        #region UraniumUI DataGrid

        private static Random random = new();

        public ObservableCollection<Student> Items2 { get; } = new();

        private void InitUraniumUiDataGrid()
        {
            for (int i = 0; i < 10; i++)
            {
                Items2.Add(new Student
                {
                    Id = i,
                    Name = "Person " + i,
                    Age = random.Next(14, 85),
                });
            }
        }

        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }

        #endregion UraniumUI DataGrid
    }
}