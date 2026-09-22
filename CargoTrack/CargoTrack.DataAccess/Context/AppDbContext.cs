using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.Sender)
                .WithMany(m => m.SentCargos)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.Receiver)
                .WithMany(m => m.ReceivedCargos)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.OriginBranch)
                .WithMany(m => m.OriginCargos)
                .HasForeignKey(c => c.OriginBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
              .HasOne(c => c.DestinationBranch)
              .WithMany(m => m.DestinationCargos)
              .HasForeignKey(c => c.DestinationBranchId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoMovement>()
                .HasOne(c => c.Cargo)
                .WithMany(m => m.CargoMovements)
                .HasForeignKey(m => m.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoMovement>()
                .HasOne(c => c.Branch)
                .WithMany(e => e.CargoMovements)
                .HasForeignKey(c => c.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoMovement>()
                .HasOne(c => c.Employee)
                .WithMany(e => e.CargoMovements)
                .HasForeignKey(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoMovement>()
                .HasOne(c => c.TransferCenter)
                .WithMany(t => t.CargoMovements)
                .HasForeignKey(c => c.TransferCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Employee)
                .WithMany(e => e.Deliveries)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DeliveryException>()
               .HasOne(d => d.Employee)
               .WithMany(e => e.DeliveryExceptions)
               .HasForeignKey(d => d.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DeliveryException>()
               .HasOne(d => d.Cargo)
               .WithMany(e => e.DeliveryExceptions)
               .HasForeignKey(d => d.CargoId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<TransferCenter>()
                .HasOne(t => t.City)
                .WithMany(c => c.TransferCenters)
                .HasForeignKey(t => t.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Branch>()
                .HasOne(t => t.City)
                .WithMany(c => c.Branches)
                .HasForeignKey(t => t.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
               .HasOne(t => t.Branch)
               .WithMany(c => c.Employees)
               .HasForeignKey(t => t.BranchId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
               .HasOne(t => t.User)
               .WithMany(c => c.Addresses)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
               .HasOne(t => t.User)
               .WithMany(u => u.AuditLogs)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasOne(x => x.Branch)
                .WithMany(x => x.Managers)
                .HasForeignKey(x => x.BranchId)
                .IsRequired(false);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<CargoMovement> CargoMovements { get; set; }
        public DbSet<CargoPrice> CargoPrices { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryException> DeliveryExceptions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TransferCenter> TransferCenters { get; set; }
    }
}
