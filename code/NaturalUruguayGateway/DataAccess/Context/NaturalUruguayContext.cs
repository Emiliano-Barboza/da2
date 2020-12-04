using System;
using System.IO;
using Domain.Constants;
using NaturalUruguayGateway.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NaturalUruguayGateway.DataAccess.Context
{
    public class NaturalUruguayContext : DbContext
    {   
        public NaturalUruguayContext() { }
        public NaturalUruguayContext(DbContextOptions options) : base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var directory = Directory.GetCurrentDirectory();
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(directory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("NaturalUruguayDB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetModelRoleUser(modelBuilder);
            SetModelSession(modelBuilder);
            SetModelUser(modelBuilder);
            SetModelRegion(modelBuilder);
            SetModelSpot(modelBuilder);
            SetModelCategory(modelBuilder);
            SetModelLodgment(modelBuilder);
            SetModelBooking(modelBuilder);
            SetModelBookingStatus(modelBuilder);
            SetModelReview(modelBuilder);
        }    
        
        private void SetModelRoleUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = 1,
                    Name = "Super admin"
                },
                new UserRole
                {
                    Id = 2,
                    Name = "Admin"
                }
            );
        }
        
        private void SetModelSession(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>();
        }
        
        private void SetModelUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "super@admin.com",
                Name = "Master of universe",
                Password = "c73faf16b04d54ad594fbc919cd4cd93",
                RoleId = 1
            });
        }
        
        private void SetModelRegion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>()
                .HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Region>()
                .HasMany(c => c.Spots)
                .WithOne(e => e.Region)
                .IsRequired();
            
            modelBuilder.Entity<Region>().HasData(
                new Region
                {
                    Id = 1,
                    Name = "Región metropolitana"
                },
                new Region
                {
                    Id = 2,
                    Name = "Región Centro Sur"
                },
                new Region
                {
                    Id = 3,
                    Name = "Región Este"
                },
                new Region
                {
                    Id = 4,
                    Name = "Región Litoral Norte"
                },
                new Region
                {
                    Id = 5,
                    Name = "Región “Corredor Pájaros Pintados”"
                }
            );
            
        }
        
        private void SetModelSpot(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Spot>()
                .HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Spot>()
                .Property(x => x.Description).HasMaxLength(2000);
            modelBuilder.Entity<Spot>()
                .HasMany(c => c.Lodgments)
                .WithOne(e => e.Spot)
                .IsRequired();
        }
        
        private void SetModelCategory(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<CategorySpot>()
                .HasKey(x => new { x.CategoryId, x.SpotId });  
            modelBuilder.Entity<CategorySpot>()
                .HasOne(x => x.Category)
                .WithMany(b => b.CategorySpots)
                .HasForeignKey(bc => bc.CategoryId);  
            modelBuilder.Entity<CategorySpot>()
                .HasOne(bc => bc.Spot)
                .WithMany(c => c.CategorySpots)
                .HasForeignKey(bc => bc.SpotId);
            
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Ciudades"
                },
                new Category
                {
                    Id = 2,
                    Name = "Pueblos"
                },
                new Category
                {
                    Id = 3,
                    Name = "Áreas protegidas"
                },
                new Category
                {
                    Id = 4,
                    Name = "Playas"
                }
            );
        }
        
        private void SetModelLodgment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lodgment>()
                .HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Lodgment>()
                .Property(x => x.Description).HasMaxLength(2000);
            modelBuilder.Entity<Lodgment>()
                .Property(e => e.Images)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
            modelBuilder.Entity<Lodgment>()
                .HasMany(c => c.Reviews)
                .WithOne(e => e.Lodgment)
                .IsRequired();
            modelBuilder.Entity<Lodgment>()
                .HasMany(c => c.Bookings)
                .WithOne(e => e.Lodgment)
                .IsRequired();
        }
        
        private void SetModelBooking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasIndex(x => x.ConfirmationCode)
                .IsUnique();
        }
        
        private void SetModelReview(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasIndex(x => x.ConfirmationCode)
                .IsUnique();
        }
        
        private void SetModelBookingStatus(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingStatus>()
                .HasMany(c => c.Bookings)
                .WithOne(e => e.BookingStatus)
                .IsRequired();
            
            modelBuilder.Entity<BookingStatus>().HasData(
                new BookingStatus
                {
                    Id = 1,
                    Name = BookingStatusName.Created
                },
                new BookingStatus
                {
                    Id = 2,
                    Name = BookingStatusName.PendingPayment
                },
                new BookingStatus
                {
                    Id = 3,
                    Name = BookingStatusName.Accepted
                },
                new BookingStatus
                {
                    Id = 4,
                    Name = BookingStatusName.Rejected
                },
                new BookingStatus
                {
                    Id = 5,
                    Name = BookingStatusName.Expired
                }
            );
        }

    }
}