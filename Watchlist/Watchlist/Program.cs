using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Watchlist.Data;

//From https://openclassrooms.com/fr/courses/8028391-developpez-une-application-asp-net-core-avec-le-modele-mvc/8107036-tirez-un-maximum-de-ce-cours

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//NOTE : à modifier par rapport au tuto
//code original
/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/

//code du tuto qui ne compile pas
/*services.AddIdentity<Utilisateur, IdentityRole>(options =>
{
   options.User.RequireUniqueEmail = false;
})
   .AddDefaultUI(UIFramework.Bootstrap4)
 .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders();*/

//code ok ? compile mais login ne marche pas
/*builder.Services.AddIdentity<Utilisateur, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();*/

//compile et login ok
builder.Services.AddDefaultIdentity<Utilisateur>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

//NOTE : à rajouter sinon ça fait une erreur au lancement sur MapRazorPages...
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();