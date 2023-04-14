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


        public BookingSystemApplicationContext()
        {

        }

        public BookingSystemApplicationContext(DbContextOptions<BookingSystemApplicationContext> options) : base(options) { 
        
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            base.OnConfiguring(dbContextOptionsBuilder);
        }

    }
}
