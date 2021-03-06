﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NaturalUruguayGateway.DataAccess.Context;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    [DbContext(typeof(NaturalUruguayContext))]
    [Migration("20201126213156_UpdateDataBaseMigration")]
    partial class UpdateDataBaseMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("AmountGuest")
                        .HasColumnType("tinyint");

                    b.Property<int>("BookingStatusId")
                        .HasColumnType("int");

                    b.Property<long>("CheckIn")
                        .HasColumnType("bigint");

                    b.Property<long>("CheckOut")
                        .HasColumnType("bigint");

                    b.Property<string>("ConfirmationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LodgmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("StatusDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookingStatusId");

                    b.HasIndex("ConfirmationCode")
                        .IsUnique();

                    b.HasIndex("LodgmentId");

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.BookingStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BookingStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Creada"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Pendiente Pago"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Aceptada"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Rechazada"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Expirada"
                        });
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Ciudades"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Pueblos"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Áreas protegidas"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Playas"
                        });
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.CategorySpot", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("SpotId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId", "SpotId");

                    b.HasIndex("SpotId");

                    b.ToTable("CategorySpot");
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Lodgment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("AmountOfStars")
                        .HasColumnType("tinyint");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("ContactInformation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("Images")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("SpotId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("SpotId");

                    b.ToTable("Lodgment");
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Region");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Región metropolitana"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Región Centro Sur"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Región Este"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Región Litoral Norte"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Región “Corredor Pájaros Pintados”"
                        });
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("AmountOfStars")
                        .HasColumnType("tinyint");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConfirmationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LodgmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConfirmationCode")
                        .IsUnique();

                    b.HasIndex("LodgmentId");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Spot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("RegionId");

                    b.ToTable("Spot");
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("RoleId");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "super@admin.com",
                            IsDeleted = false,
                            Name = "Master of universe",
                            Password = "c73faf16b04d54ad594fbc919cd4cd93",
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Super admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Booking", b =>
                {
                    b.HasOne("NaturalUruguayGateway.Domain.BookingStatus", "BookingStatus")
                        .WithMany("Bookings")
                        .HasForeignKey("BookingStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NaturalUruguayGateway.Domain.Lodgment", "Lodgment")
                        .WithMany("Bookings")
                        .HasForeignKey("LodgmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.CategorySpot", b =>
                {
                    b.HasOne("NaturalUruguayGateway.Domain.Category", "Category")
                        .WithMany("CategorySpots")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NaturalUruguayGateway.Domain.Spot", "Spot")
                        .WithMany("CategorySpots")
                        .HasForeignKey("SpotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Lodgment", b =>
                {
                    b.HasOne("NaturalUruguayGateway.Domain.Spot", "Spot")
                        .WithMany("Lodgments")
                        .HasForeignKey("SpotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Review", b =>
                {
                    b.HasOne("NaturalUruguayGateway.Domain.Lodgment", "Lodgment")
                        .WithMany("Reviews")
                        .HasForeignKey("LodgmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Session", b =>
                {
                    b.HasOne("NaturalUruguayGateway.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.Spot", b =>
                {
                    b.HasOne("NaturalUruguayGateway.Domain.Region", "Region")
                        .WithMany("Spots")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NaturalUruguayGateway.Domain.User", b =>
                {
                    b.HasOne("NaturalUruguayGateway.Domain.UserRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
