using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab9.DataLayer.Interfaces;
using Lab9.DataLayer.Entities;
using Lab9.DataLayer.EFContext;
using System.Data.Entity;

namespace Lab9.DataLayer.Repositories
{
    class CustomerRepository : IRepository<Customer>
    {
        CarShopContext context;
        public CustomerRepository(CarShopContext context)
        {
            this.context = context;
        }
        public void Create(Customer t)
        {
            context.Customers.Add(t);
        }
        public void Delete(Int32 id)
        {
            Customer cust = context.Customers.Find(id);
            context.Customers.Remove(cust);
        }
        public Customer Get(Int32 id)
        {
            return context.Customers.Find(id);
        }
        public void Update(Customer t)
        {
            context.Entry<Customer>(t).State = EntityState.Modified;
        }
        public IEnumerable<Customer> GetAll()
        {
            return context.Customers.Include(g => g.Cars);
        }
    }
}
