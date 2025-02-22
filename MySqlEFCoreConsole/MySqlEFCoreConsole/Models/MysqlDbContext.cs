using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MySqlEFCoreConsole.Models;

public partial class MysqlDbContext : DbContext
{
    public MysqlDbContext()
    {
    }

    public MysqlDbContext(DbContextOptions<MysqlDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Foreuro> Foreuros { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Database=receipts2;user=root;password=;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Foreuro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("foreuro");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.MyEuroColumn)
                .HasMaxLength(100)
                .HasDefaultValueSql("'NULL'");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.DateReceipt).HasColumnType("datetime");
            entity.Property(e => e.FullData)
                .HasMaxLength(100)
                .HasDefaultValueSql("'''0'''");
            entity.Property(e => e.Group)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''");
            entity.Property(e => e.Price).HasPrecision(20, 6);
            entity.Property(e => e.SourceLine).HasColumnType("int(11)");
            entity.Property(e => e.SourceName)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
