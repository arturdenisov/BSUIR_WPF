using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab9.DataLayer.Entities;



namespace Lab9.DataLayer.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<Car> Cars { get; }
        IRepository<Customer> Customers { get; }
        void Save();
    }
}
