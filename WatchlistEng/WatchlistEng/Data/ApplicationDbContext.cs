using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WatchlistEng.Models;

namespace WatchlistEng.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<UserMovie> UserMovies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserMovie>()
            .HasKey(t => new { t.UserId, t.MovieId });
        }

        public DbSet<WatchlistEng.Models.MovieViewModel>? MovieViewModel { get; set; }
    }
}