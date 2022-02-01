using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lab9.BusinessLayer.Interfaces;
using Lab9.BusinessLayer.Model;
using Lab9.DataLayer.EFContext;
using Lab9.DataLayer.Entities;
using Lab9.DataLayer.Interfaces;
using Lab9.DataLayer.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Lab9.BusinessLayer.Services
{
    public class CarServise : ICarService
    {
        IUnitOfWork dataBase;

        public CarServise(string name)
        {
            dataBase = new EntityUnitOfWork(name);

            //AutoMapper configure
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Car, CarViewModel>();
                cfg.CreateMap<CarViewModel, Car>();
                cfg.CreateMap<Customer, CustomerViewModel>();
                cfg.CreateMap<CustomerViewModel, Customer>();
            });
        }

        public void AddCustomerToCar(Int32 carID, CustomerViewModel customer)
        {
            var car = dataBase.Cars.Get(carID);
            
            //Reflection of CustomerViewModel-object on Customer-object
            var custom = Mapper.Map<Customer>(customer);

            //Define a car and a price for a customer
            custom.CarID = car.CarID;
            custom.PersonalPrice = customer.HasDiscount == true
                ? car.BasePrice * (decimal)0.8
                : car.BasePrice;

            //Add a customer
            car.Customers.Add(custom);

            //Save changes
            dataBase.Save();
        }

        public void CreateCar(CarViewModel car)
        {
            var newCar = Mapper.Map<Car>(car);
            dataBase.Cars.Create(newCar);
            dataBase.Save();
        }

        public void DeleteCar(Int32 carId)
        {
            dataBase.Cars.Delete(carId);
            dataBase.Save();
        }

        public CarViewModel Get(Int32 id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<CarViewModel> GetAll()
        {
            //List<Car> reflection on ObservableCollection<CarViewModel>
            var cars = Mapper.Map <ObservableCollection<CarViewModel>> (dataBase.Cars.GetAll());
            foreach (CarViewModel car in cars)
            {
                foreach (CustomerViewModel cust in car.Customers)
                {
                    CheckDiscount(cust);
                }
            }
            return cars;
        }

        private void CheckDiscount(CustomerViewModel custVM)
        {
            if (custVM.CustomerID > 0)
            {
                Customer customer = dataBase.Customers.Get(custVM.CustomerID);
                if (customer.PersonalPrice < customer.Cars.BasePrice) custVM.HasDiscount = true;
                else custVM.HasDiscount = false;
            }
        }

        public void RemoveCustomerFromCar(Int32 carID, Int32 customerID)
        {
            dataBase.Customers.Delete(customerID);
            dataBase.Save();
        }

        public void UpdateCar(CarViewModel car)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(CustomerViewModel updatedCust)
        {
            Customer c = dataBase.Customers.Get(updatedCust.CustomerID);
            c.DateOfBirth = updatedCust.DateOfBirth;
            c.FileName = updatedCust.FileName;
            c.FullName = updatedCust.FullName;

            //New discount info
            Car car = dataBase.Cars.Get(c.CarID);
            c.PersonalPrice = updatedCust.HasDiscount == true
                ? car.BasePrice * (decimal)0.8
                : car.BasePrice;
            dataBase.Save();
        }
    }
}




