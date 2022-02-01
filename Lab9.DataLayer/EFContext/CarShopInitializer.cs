using System;
using System.Collections.Generic;
using System.Data.Entity;
using Lab9.DataLayer.Entities;

namespace Lab9.DataLayer.EFContext
{
    public class CarShopInitializer : DropCreateDatabaseIfModelChanges<CarShopContext>
    {
        protected override void Seed(CarShopContext context)
        {
            Int32 x = 1;
            context.Cars.AddRange(new Car[]
            {
                new Car{
                    Model = "Abarth500X", BasePrice=25000, Commence=new DateTime(2017, 01, 20),
                    Customers=new List<Customer> {
                                new Customer{ FullName = "Денисов А.Ю.", DateOfBirth=new DateTime(1987, 12, 21), FileName="1.png", PersonalPrice=20000, CarID=1 },
                                new Customer{ FullName = "Белов А.А.", DateOfBirth=new DateTime(1989, 11, 09), FileName="2.png", PersonalPrice=20000, CarID=1 },
                                new Customer{ FullName = "Соколова Г.Н.", DateOfBirth=new DateTime(1950, 03, 07), FileName="3.png", PersonalPrice=20000, CarID=1},
                                new Customer{ FullName = "Анышева А.А.", DateOfBirth=new DateTime(1980, 03, 07), FileName="7.png", PersonalPrice=20000, CarID=1},
                                new Customer{ FullName = "Самсонов Л.Г.", DateOfBirth=new DateTime(1960, 03, 07), FileName="8.png", PersonalPrice=20000, CarID=1},
                            }
                        },
                new Car{
                    Model = "AudiA82", BasePrice=37000, Commence=new DateTime(2017, 02, 28),
                    Customers=new List<Customer>
                            {
                                new Customer{ FullName = "Денисова Н.Ф.", DateOfBirth=new DateTime(1989, 02, 03), FileName="4.png", PersonalPrice=29600, CarID=2 },
                                new Customer{ FullName = "Лашук И.В.", DateOfBirth=new DateTime(1980, 07, 19), FileName="5.png", PersonalPrice=29600, CarID=2 },
                                new Customer{ FullName = "Кацук К.Н.", DateOfBirth=new DateTime(1976, 05, 11), FileName="6.png", PersonalPrice=29600, CarID=2},
                            }
                        },
                new Car{
                    Model = "Honda Fit", BasePrice=27000, Commence=new DateTime(2015, 01, 28),
                    Customers=new List<Customer>
                            {
                                new Customer{ FullName = "Дубровкин А.К.", DateOfBirth=new DateTime(1989, 02, 03), FileName="10.png", PersonalPrice=21600, CarID=3 },
                                new Customer{ FullName = "Парфенов К.Л.", DateOfBirth=new DateTime(1980, 07, 19), FileName="11.png", PersonalPrice=21600, CarID=3 },
                                new Customer{ FullName = "Стечин Р.А.", DateOfBirth=new DateTime(1976, 05, 11), FileName="12.png", PersonalPrice=21600, CarID=3},
                            }
                        }
            });
            context.SaveChanges();
        }
    }
}