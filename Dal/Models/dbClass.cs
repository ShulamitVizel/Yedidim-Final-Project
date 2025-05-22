using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

public partial class dbClass : DbContext
{
    public dbClass()
    {
    }

    public dbClass(DbContextOptions<dbClass> options)
        : base(options)
    {
    }

    public virtual DbSet<Call> Calls { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Volunteer> Volunteers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename='C:\\Users\\shv32\\OneDrive\\שולחן העבודה\\newYedidim\\Yedidim\\Dal\\Data\\database.mdf';Integrated Security=True; Connect Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Call>(entity =>
        {
            entity.HasKey(e => e.CallId).HasName("PK__Calls__3214EC07DA0A4C9B");

            entity.Property(e => e.CallId)
                .ValueGeneratedNever()
                .HasColumnName("CallID");
            entity.Property(e => e.CallLatitude).HasColumnName("CallLatitude ");
            entity.Property(e => e.CallTime).HasColumnType("datetime");
            entity.Property(e => e.CallType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.FinalVolunteerId).HasColumnName("FinalVolunteerID");

            entity.HasOne(d => d.Client).WithMany(p => p.Calls)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calls__ClientID__4CA06362");

            entity.HasOne(d => d.FinalVolunteer).WithMany(p => p.Calls)
                .HasForeignKey(d => d.FinalVolunteerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calls__FinalVolu__4D94879B");

            entity.HasMany(d => d.Volunteers).WithMany(p => p.CallsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "CallVolunteer",
                    r => r.HasOne<Volunteer>().WithMany()
                        .HasForeignKey("VolunteerId")
                        .HasConstraintName("FK__CallVolun__Volun__4BAC3F29"),
                    l => l.HasOne<Call>().WithMany()
                        .HasForeignKey("CallId")
                        .HasConstraintName("FK__CallVolun__CallI__4AB81AF0"),
                    j =>
                    {
                        j.HasKey("CallId", "VolunteerId").HasName("PK__CallVolu__869639761721D362");
                        j.ToTable("CallVolunteers");
                        j.IndexerProperty<int>("CallId").HasColumnName("CallID");
                        j.IndexerProperty<int>("VolunteerId").HasColumnName("VolunteerID");
                    });
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.ClientId)
                .ValueGeneratedNever()
                .HasColumnName("ClientID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Volunteer>(entity =>
        {
            entity.HasKey(e => e.VolunteerId).HasName("PK__Voluntee__3214EC073FDA8C3F");

            entity.Property(e => e.VolunteerId)
                .ValueGeneratedNever()
                .HasColumnName("VolunteerID");
            entity.Property(e => e.Level)
                .HasMaxLength(10)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VolunteerLatitude).HasColumnName("VolunteerLatitude ");
            entity.Property(e => e.VolunteerLongitude).HasColumnName("VolunteerLongitude ");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
