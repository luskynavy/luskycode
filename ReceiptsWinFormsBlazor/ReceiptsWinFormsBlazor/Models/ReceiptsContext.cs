﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReceiptsBlazorWinForms.Models;

public partial class ReceiptsContext : DbContext
{
    public ReceiptsContext()
    {
    }

    public ReceiptsContext(DbContextOptions<ReceiptsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}