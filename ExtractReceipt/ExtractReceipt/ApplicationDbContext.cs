using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ExtractReceipt
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Get the connectionString from appsettings.json
            //Must install nuget Microsoft.Extensions.Configuration & Microsoft.Extensions.Configuration.Json
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json", false, true);
            IConfigurationRoot configuration = builder.Build();
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*modelBuilder.Entity<Product>()
            .HasKey(t => new { t.Id });*/
        }
    }
}