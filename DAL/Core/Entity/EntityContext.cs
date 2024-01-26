using Entity.Users;
using Entity.Vehicle;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core.Entity
{
    public class EntityContext : DbContext
    {
        
        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-H0M8STK\\MSSQL;database=CaseDB; integrated security=true;");
        }
        */
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Boat> Boats { get; set; }
        public DbSet<Buse> Buses { get; set; }
        public DbSet<Categories> Categories { get; set; }

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
