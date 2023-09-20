using Azure.Core;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReceiptsWeb.Models;

namespace ReceiptsWeb
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			//Ajoute la bdd à l'injection de dépendance
			builder.Services.AddDbContext<ReceiptsContext>(options =>
				options.UseSqlServer(connectionString));

			//Localization
			builder.Services.AddMvc()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.AddDataAnnotationsLocalization();

			builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

			builder.Services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[] { "en", "fr" };
				options.SetDefaultCulture(supportedCultures[0])
					.AddSupportedCultures(supportedCultures)
					.AddSupportedUICultures(supportedCultures);
			});

			// Add services to the container.
			var mvcBuilder = builder.Services.AddControllersWithViews();
			mvcBuilder.AddRazorRuntimeCompilation();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}

			//Localization methods
			var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
			if (localizationOptions != null)
			{
				app.UseRequestLocalization(localizationOptions.Value);
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}