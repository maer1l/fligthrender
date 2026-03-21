using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using fligthrender.Models;

namespace fligthrender.Data;

public partial class FlightrenderContext : DbContext
{
    public FlightrenderContext()
    {
    }

    public FlightrenderContext(DbContextOptions<FlightrenderContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Plane> Planes { get; set; }

    public virtual DbSet<Planespicture> Planespictures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__manufact__5E5A8E2705A4BEE1");
        });

        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasKey(e => e.PlaneId).HasName("PK__planes__4D11D7FD8E8E3EE9");

            entity.HasOne(d => d.Brand).WithMany(p => p.Planes).HasConstraintName("FK_planes_manufacturers");
        });

        modelBuilder.Entity<Planespicture>(entity =>
        {
            entity.HasKey(e => new { e.PlaneId, e.Path });

            entity.HasOne(d => d.Plane).WithMany()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_planespictures_planes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
