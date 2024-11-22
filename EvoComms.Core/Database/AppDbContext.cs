using System;
using System.IO;

using EvoComms.Core.Database.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EvoComms.Core.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
#if DEBUG
                var baseDirectory = AppContext.BaseDirectory;
                DbPath = Path.Combine(baseDirectory, "Data", "EvoComms.sqlite");
#else
            DbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "ClockingSystems",
                "EvoComms",
                "Data",
                "EvoComms.sqlite"
            );
#endif

            Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);
        }

        public string DbPath { get; }
        public DbSet<ClockingMachine> ClockingMachines { get; set; }
        public DbSet<Clocking> Clockings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
            options.LogTo(Console.WriteLine, LogLevel.Information);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clocking>(entity =>
            {
                // Configure primary key
                entity.HasKey(e => e.Id);

                // Configure relationship with Employee
                entity.HasOne(c => c.Employee)
                    .WithMany(e => e.Clockings)
                    .HasForeignKey(c => c.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                // Configure relationship with ClockingMachine
                entity.HasOne(c => c.ClockingMachine)
                    .WithMany(m => m.Clockings)
                    .HasForeignKey(c => c.ClockingMachineId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ClockedAt).IsRequired();
                entity.Property(e => e.ReceivedAt).IsRequired();
            });
        }
    }
}