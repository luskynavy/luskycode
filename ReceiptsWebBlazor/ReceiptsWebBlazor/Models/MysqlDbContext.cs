using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ReceiptsWebBlazor.Models;

public partial class MysqlDbContext : DbContext
{
    public MysqlDbContext()
    {
    }

    public MysqlDbContext(DbContextOptions<MysqlDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=receipts;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.7.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("products")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.DateReceipt).HasColumnType("datetime");
            entity.Property(e => e.FullData)
                .HasMaxLength(100)
                .HasDefaultValueSql("'0'");
            entity.Property(e => e.Group)
                .HasMaxLength(100)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Price).HasPrecision(20, 6);
            entity.Property(e => e.SourceLine).HasColumnType("int(11)");
            entity.Property(e => e.SourceName)
                .HasMaxLength(100)
                .HasDefaultValueSql("''");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
