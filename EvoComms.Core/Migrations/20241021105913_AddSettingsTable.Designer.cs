﻿// <auto-generated />
using System;
using EvoComms.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EvoComms.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241021105913_AddSettingsTable")]
    partial class AddSettingsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("EvoComms.Core.Database.Models.Clocking", b =>
                {
                    b.Property<int>("ClockingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MacAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModelName")
                        .HasColumnType("TEXT");

                    b.HasKey("ClockingId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Clockings");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClockingId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HanvonImg")
                        .HasColumnType("TEXT");

                    b.Property<string>("HanvonTemplate")
                        .HasColumnType("TEXT");

                    b.Property<string>("TimyImg")
                        .HasColumnType("TEXT");

                    b.Property<string>("TimyTemplate")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Exception")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Logger")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TimeStamp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.OutputType", b =>
                {
                    b.Property<int>("OutputTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("OutputTypeId");

                    b.ToTable("OutputTypes");

                    b.HasData(
                        new
                        {
                            OutputTypeId = 1,
                            Description = "BioTime output type",
                            Name = "BioTime"
                        },
                        new
                        {
                            OutputTypeId = 2,
                            Description = "InTime output type",
                            Name = "InTime"
                        },
                        new
                        {
                            OutputTypeId = 3,
                            Description = "InfoTime output type",
                            Name = "InfoTime"
                        });
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.Settings", b =>
                {
                    b.Property<int>("SettingsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFirstRun")
                        .HasColumnType("INTEGER");

                    b.HasKey("SettingsId");

                    b.ToTable("Settings");

                    b.HasData(
                        new
                        {
                            SettingsId = 1,
                            IsFirstRun = true
                        });
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.TimySettings", b =>
                {
                    b.Property<int>("TimySettingsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ListenPort")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OutputPath")
                        .HasColumnType("TEXT");

                    b.Property<int>("OutputTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TimySettingsId");

                    b.HasIndex("OutputTypeId");

                    b.ToTable("TimySettings");

                    b.HasData(
                        new
                        {
                            TimySettingsId = 1,
                            Enabled = true,
                            ListenPort = 7788,
                            OutputPath = "C:/Info",
                            OutputTypeId = 1
                        });
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.TimyTerminal", b =>
                {
                    b.Property<int>("TimyTerminalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MacAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModelName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TimyTerminalId");

                    b.ToTable("TimyTerminals");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.Clocking", b =>
                {
                    b.HasOne("EvoComms.Core.Database.Models.Employee", "Employee")
                        .WithMany("Clockings")
                        .HasForeignKey("EmployeeId");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.TimySettings", b =>
                {
                    b.HasOne("EvoComms.Core.Database.Models.OutputType", "OutputType")
                        .WithMany()
                        .HasForeignKey("OutputTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OutputType");
                });

            modelBuilder.Entity("EvoComms.Core.Database.Models.Employee", b =>
                {
                    b.Navigation("Clockings");
                });
#pragma warning restore 612, 618
        }
    }
}
