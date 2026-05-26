using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Models;

namespace AppDbContext;

public partial class TravelWebPageContext : DbContext
{
    public TravelWebPageContext()
    {
    }

    public TravelWebPageContext(DbContextOptions<TravelWebPageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accommodation> Accommodations { get; set; }

    public virtual DbSet<AdditionalInfo> AdditionalInfos { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<DatePrice> DatePrices { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<Gallary> Gallaries { get; set; }

    public virtual DbSet<Itinerary> Itineraries { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Username=postgres;Password=123;Database=TravelWebPage;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accommodation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accommodation_pkey");

            entity.ToTable("accommodation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(50)
                .HasColumnName("abbreviation");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.NoOfNight).HasColumnName("noOfNight");
            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");

            entity.HasOne(d => d.Tour).WithMany(p => p.Accommodations)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("accommodation_tourId_fkey");
        });

        modelBuilder.Entity<AdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("additionalInfo_pkey");

            entity.ToTable("additionalInfo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.SubTitle)
                .HasMaxLength(255)
                .HasColumnName("subTitle");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Tour).WithMany(p => p.AdditionalInfos)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("additionalInfo_tourId_fkey");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookings_pkey");

            entity.ToTable("bookings");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DatePriceId).HasColumnName("datePriceId");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .HasColumnName("gender");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.NoOfTravellers).HasColumnName("noOfTravellers");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.DatePrice).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.DatePriceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("bookings_datePriceId_fkey");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookings_tourId_fkey");
        });

        modelBuilder.Entity<DatePrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("datePrice_pkey");

            entity.ToTable("datePrice");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate)
                .HasMaxLength(255)
                .HasColumnName("endDate");
            entity.Property(e => e.Price)
                .HasMaxLength(200)
                .HasColumnName("price");
            entity.Property(e => e.StartDate)
                .HasMaxLength(255)
                .HasColumnName("startDate");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Tour).WithMany(p => p.DatePrices)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("datePrice_tourId_fkey");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("destinations_pkey");

            entity.ToTable("destinations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Destinationcategory)
                .HasMaxLength(100)
                .HasColumnName("destinationcategory");
            entity.Property(e => e.PhotoCategory)
                .HasMaxLength(100)
                .HasColumnName("photoCategory");
        });

        modelBuilder.Entity<Gallary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gallary_pkey");

            entity.ToTable("gallary");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PhotoPath)
                .HasMaxLength(100)
                .HasColumnName("photoPath");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Tour).WithMany(p => p.Gallaries)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gallary_tourId_fkey");
        });

        modelBuilder.Entity<Itinerary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("itineraries_pkey");

            entity.ToTable("itineraries");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DayNumber).HasColumnName("dayNumber");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Tour).WithMany(p => p.Itineraries)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("itineraries_tourId_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reviews_pkey");

            entity.ToTable("reviews");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Authoremail)
                .HasMaxLength(100)
                .HasColumnName("authoremail");
            entity.Property(e => e.Authorname)
                .HasMaxLength(100)
                .HasColumnName("authorname");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Isgenuine)
                .HasDefaultValue(false)
                .HasColumnName("isgenuine");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Reviewtext).HasColumnName("reviewtext");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("services_pkey");

            entity.ToTable("services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IncludeServices)
                .HasMaxLength(500)
                .HasColumnName("includeServices");
            entity.Property(e => e.NotIncludeServices)
                .HasMaxLength(500)
                .HasColumnName("notIncludeServices");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Tour).WithMany(p => p.Services)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("services_tourId_fkey");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tours_pkey");

            entity.ToTable("tours");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accommodation)
                .HasMaxLength(200)
                .HasColumnName("accommodation");
            entity.Property(e => e.Altitude)
                .HasMaxLength(200)
                .HasColumnName("altitude");
            entity.Property(e => e.BestSeasons)
                .HasMaxLength(500)
                .HasColumnName("bestSeasons");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DestinationId).HasColumnName("destinationId");
            entity.Property(e => e.Duration)
                .HasMaxLength(100)
                .HasColumnName("duration");
            entity.Property(e => e.Grade)
                .HasMaxLength(200)
                .HasColumnName("grade");
            entity.Property(e => e.PhotoPath)
                .HasMaxLength(500)
                .HasColumnName("photoPath");
            entity.Property(e => e.Region)
                .HasMaxLength(200)
                .HasColumnName("region");
            entity.Property(e => e.RouteMapPhotoPath)
                .HasMaxLength(500)
                .HasColumnName("routeMapPhotoPath");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Destination).WithMany(p => p.Tours)
                .HasForeignKey(d => d.DestinationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tours_destinationId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Admin'::character varying")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
