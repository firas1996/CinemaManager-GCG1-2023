using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CinemaManager_GCG1.Models.Cinema;

public partial class CinemaDbGcg1Context : DbContext
{
    public CinemaDbGcg1Context()
    {
    }

    public CinemaDbGcg1Context(DbContextOptions<CinemaDbGcg1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Producer> Producers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:CinemaCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producer__3214EC0767A9B3A8");

            entity.ToTable("Producer");

            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Nationality).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
