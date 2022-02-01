using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.DataLayer.Interfaces
{
    //Realize CRUD functions (Create-Read-Update-Delete)
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(Int32 id);
        void Create(T t);
        void Update(T t);
        void Delete(Int32 id);
    }
}
