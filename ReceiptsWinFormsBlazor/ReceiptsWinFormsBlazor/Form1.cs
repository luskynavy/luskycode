using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReceiptsBlazorWinForms.Components;
using ReceiptsBlazorWinForms.Models;

namespace ReceiptsBlazorWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var services = new ServiceCollection();

            //Get the connectionString from appsettings.json
            //Must install nuget Microsoft.Extensions.Configuration & Microsoft.Extensions.Configuration.Json
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json", false, true);
            IConfigurationRoot configuration = builder.Build();
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextFactory<ReceiptsContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddLocalization();

            //Debug with browser with about:debugging, only work with .net maui ?
            //services.AddBlazorWebViewDeveloperTools();

            services.AddWindowsFormsBlazorWebView();
            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();

            blazorWebView1.RootComponents.Add<App>("#app");
        }
    }
}