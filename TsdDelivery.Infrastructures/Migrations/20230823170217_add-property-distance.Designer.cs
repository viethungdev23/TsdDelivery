﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TsdDelivery.Infrastructures;

#nullable disable

namespace TsdDelivery.Infrastructures.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230823170217_add-property-distance")]
    partial class addpropertydistance
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Distance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("DriverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModificationBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PickUpDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecipientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReciveLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReservationStatus")
                        .HasColumnType("int");

                    b.Property<string>("SendLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotallPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.ReservationDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReservationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ReservationId");

                    b.HasIndex("ServiceId");

                    b.ToTable("ReservationDetail");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModificationBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModificationBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("VehicleTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.ShippingRate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("KmFrom")
                        .HasColumnType("int");

                    b.Property<int>("KmTo")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModificationBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("ShippingRate");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModificationBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageVehicle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ModificationBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameVehicle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("VehicleLoad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("VehicleTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.VehicleType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeleteBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModificationBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VehicleTypeImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleTypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("VehicleType");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Reservation", b =>
                {
                    b.HasOne("TsdDelivery.Domain.Entities.User", "Driver")
                        .WithMany("ReservationDrivers")
                        .HasForeignKey("DriverId");

                    b.HasOne("TsdDelivery.Domain.Entities.User", "User")
                        .WithMany("ReservationUsers")
                        .HasForeignKey("UserId");

                    b.OwnsOne("TsdDelivery.Domain.Entities.Goods", "Goods", b1 =>
                        {
                            b1.Property<Guid>("ReservationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal?>("Height")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<decimal?>("Length")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Weight")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<decimal?>("Width")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("ReservationId");

                            b1.ToTable("Reservation");

                            b1.WithOwner()
                                .HasForeignKey("ReservationId");
                        });

                    b.Navigation("Driver");

                    b.Navigation("Goods")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.ReservationDetail", b =>
                {
                    b.HasOne("TsdDelivery.Domain.Entities.Reservation", "Reservation")
                        .WithMany("reservationDetails")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TsdDelivery.Domain.Entities.Service", "Service")
                        .WithMany("reservationDetails")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Service", b =>
                {
                    b.HasOne("TsdDelivery.Domain.Entities.VehicleType", "VehicleType")
                        .WithMany("services")
                        .HasForeignKey("VehicleTypeId");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.ShippingRate", b =>
                {
                    b.HasOne("TsdDelivery.Domain.Entities.Service", "Service")
                        .WithMany("shippingRates")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Service");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.User", b =>
                {
                    b.HasOne("TsdDelivery.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Vehicle", b =>
                {
                    b.HasOne("TsdDelivery.Domain.Entities.User", "User")
                        .WithMany("Vehicles")
                        .HasForeignKey("UserId");

                    b.HasOne("TsdDelivery.Domain.Entities.VehicleType", "VehicleType")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeId");

                    b.Navigation("User");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Reservation", b =>
                {
                    b.Navigation("reservationDetails");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.Service", b =>
                {
                    b.Navigation("reservationDetails");

                    b.Navigation("shippingRates");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.User", b =>
                {
                    b.Navigation("ReservationDrivers");

                    b.Navigation("ReservationUsers");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("TsdDelivery.Domain.Entities.VehicleType", b =>
                {
                    b.Navigation("Vehicles");

                    b.Navigation("services");
                });
#pragma warning restore 612, 618
        }
    }
}
