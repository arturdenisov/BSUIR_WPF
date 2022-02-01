using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab9.DataLayer.Entities;
using Lab9.DataLayer.Interfaces;
using Lab9.DataLayer.EFContext;
using System.Windows;

namespace Lab9.DataLayer.Repositories
{
    public class EntityUnitOfWork : IUnitOfWork
    {
        CarShopContext context;
        CarsRepository carsRepository;
        CustomerRepository customersRepository;

        public EntityUnitOfWork(string connectionString)
        {
            context = new CarShopContext(connectionString);
        }

        public IRepository<Car> Cars
        {
            get
            {
                if (carsRepository == null)
                    carsRepository = new CarsRepository(context);
                return carsRepository;
            }
        }

        public IRepository<Customer> Customers
        {
            get
            {
                if (customersRepository == null)
                    customersRepository = new CustomerRepository(context);
                return customersRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        //IDisposable
        private Boolean disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
