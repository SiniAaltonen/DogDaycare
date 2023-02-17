using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DogDC.Models
{
    public partial class DogdcDBContext : DbContext
    {
        public DogdcDBContext()
        {
        }

        public DogdcDBContext(DbContextOptions<DogdcDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<BookingStatus> BookingStatuses { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ContactForm> ContactForms { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Pet> Pets { get; set; } = null!;
        public virtual DbSet<Utility> Utilities { get; set; } = null!;

    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.UtilityId).HasColumnName("UtilityID");

                entity.HasOne(d => d.Utility)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UtilityId)
                    .HasConstraintName("FK__Booking__Utility__68487DD7");
            });

            modelBuilder.Entity<BookingStatus>(entity =>
            {
                entity.ToTable("BookingStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.BookingStatuses)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__BookingSt__Booki__693CA210");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.BookingStatuses)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__BookingSt__Custo__6A30C649");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContactForm>(entity =>
            {
                entity.ToTable("ContactForm");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Message)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Zipcode)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pet");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BlobUrl)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("BlobURL");

                entity.Property(e => e.Breed)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Diet)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Pet__CustomerID__6B24EA82");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__Pet__GroupID__6C190EBB");
            });

            modelBuilder.Entity<Utility>(entity =>
            {
                entity.ToTable("Utility");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Utilities)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Utility__Categor__6D0D32F4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
