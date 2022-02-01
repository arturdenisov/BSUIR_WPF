using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab9.BusinessLayer.Model;
using System.Collections.ObjectModel;

namespace Lab9.BusinessLayer.Interfaces
{
    public interface ICarService
    {
        ObservableCollection<CarViewModel> GetAll();
        CarViewModel Get(Int32 id);
        void AddCustomerToCar(Int32 carID, CustomerViewModel customer);
        void CreateCar(CarViewModel car);
        void DeleteCar(Int32 carID);
        void UpdateCar(CarViewModel car);
        void UpdateCustomer(CustomerViewModel cust);
    }
}
