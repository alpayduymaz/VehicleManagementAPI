using Entity.Users;
using Entity.Vehicle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core.Entity
{
    public class EntityContext : DbContext
    {

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-H0M8STK\\MSSQL;database=CaseDB; integrated security=true;");
        }
        
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedUser)
                .WithOne()
                .HasForeignKey<User>(u => u.CreatedUserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.LastUpdatedUser)
                .WithOne()
                .HasForeignKey<User>(u => u.LastUpdatedUserId);
        }
    }
}
