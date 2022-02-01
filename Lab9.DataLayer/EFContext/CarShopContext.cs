using System;
using System.Data.Entity;
using Lab9.DataLayer.Entities;

namespace Lab9.DataLayer.EFContext
{
    public class CarShopContext : DbContext
    {
        public CarShopContext(String connectionString) : base(connectionString)
        {
            Database.SetInitializer(new CarShopInitializer());

        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Customer> Customers { get; set; }
    }
}

