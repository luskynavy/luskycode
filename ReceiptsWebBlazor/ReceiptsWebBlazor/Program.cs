using Microsoft.EntityFrameworkCore;
using ReceiptsWebBlazor.Components;
using ReceiptsWebBlazor.Models;

namespace ReceiptsWebBlazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //Ajoute la bdd à l'injection de dépendance
            /*builder.Services.AddDbContext<ReceiptsContext>(options =>
                options.UseSqlServer(connectionString));*/
            builder.Services.AddDbContextFactory<ReceiptsContext>(options =>
                options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}