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
    class CarsRepository : IRepository<Car>
    {
        CarShopContext context;
        public CarsRepository(CarShopContext context)
        {
            this.context = context;
        }
        public void Create(Car t)
        {
            context.Cars.Add(t);
        }
        public void Delete(Int32 id)
        {
            Car car = context.Cars.Find(id);
            context.Cars.Remove(car);
       }
        public Car Get(Int32 id)
        {
            return context.Cars.Find(id);
        }
        public void Update(Car t)
        {
            context.Entry<Car>(t).State = EntityState.Modified;
        }
        public IEnumerable<Car> GetAll()
        {
            return context.Cars.Include(g => g.Customers);
        }
    }
}
