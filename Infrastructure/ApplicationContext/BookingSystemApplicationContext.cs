using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ApplicationContext
{
    public class BookingSystemApplicationContext : DbContext
    {

        //tables
        public DbSet<Train> Trains { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Schedule> Schedule { get; set; }


        public BookingSystemApplicationContext()
        {

        }

        public BookingSystemApplicationContext(DbContextOptions<BookingSystemApplicationContext> options) : base(options) { 
        
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Schedule>().HasIndex(u => new { u.RouteId, u.TrainId, u.Date}).IsUnique();

        }



        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            base.OnConfiguring(dbContextOptionsBuilder);
        }

    }
}
