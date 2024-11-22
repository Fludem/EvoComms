﻿// <auto-generated />
using System;
using EvoComms.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EvoComms.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("EvoComms.Core.Database.Models.Clocking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ClockedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("ClockingMachineId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ReceivedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClockingMachineId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Clockings");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.ClockingMachine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ClockingMachines");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClockingId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.LogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Exception")
                        .HasColumnType("TEXT");

                    b.Property<string>("LogLevel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Logger")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.Clocking", b =>
                {
                    b.HasOne("EvoComms.Core.Database.Models.ClockingMachine", "ClockingMachine")
                        .WithMany("Clockings")
                        .HasForeignKey("ClockingMachineId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EvoComms.Core.Database.Models.Employee", "Employee")
                        .WithMany("Clockings")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ClockingMachine");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.ClockingMachine", b =>
                {
                    b.Navigation("Clockings");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.Employee", b =>
                {
                    b.Navigation("Clockings");
                });
#pragma warning restore 612, 618
        }
    }
}
