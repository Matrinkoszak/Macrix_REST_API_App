using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Macrix_REST_API_App.Models;

public partial class MacrixContext : DbContext
{
    public MacrixContext()
    {
    }

    public MacrixContext(DbContextOptions<MacrixContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> People { get; set; }

//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DataSource=.\\Database\\macrix.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("person");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.FirstName).HasColumnType("TEXT (32)");
            entity.Property(e => e.LastName).HasColumnType("TEXT (32)");
            entity.Property(e => e.PostalCode).HasColumnType("TEXT (6)");
            entity.Property(e => e.StreetName).HasColumnType("TEXT (32)");
            entity.Property(e => e.Town).HasColumnType("TEXT (32)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
