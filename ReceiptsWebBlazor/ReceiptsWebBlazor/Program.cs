using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
			//Add db to dependency injection
			builder.Services.AddDbContextFactory<ReceiptsContext>(options =>
				options.UseSqlServer(connectionString));

			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();

			builder.Services.AddLocalization();
			//For culture controller
			builder.Services.AddControllers();

			builder.Services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[] { "en", "fr" };
				options.SetDefaultCulture(supportedCultures[0])
					.AddSupportedCultures(supportedCultures)
					.AddSupportedUICultures(supportedCultures);
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			//Localization methods
			var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
			if (localizationOptions != null)
			{
				app.UseRequestLocalization(localizationOptions.Value);
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAntiforgery();
			//For culture controller
			app.MapControllers();

			app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode();

			app.Run();
		}
	}
}