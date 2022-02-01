
using System.Data.Entity;

namespace Lab8
{
    class AutoCarDbInit : DropCreateDatabaseIfModelChanges<AutoCarDbEntities>
    {
        //Initialize context with primarily data
        protected override void Seed(AutoCarDbEntities context)
        {
            context.Cars.AddRange(
                new Car[]
                    {
                        new Car { ID = 100, Model="Mercedes", Price = 15000, YearOfProduction = 2012, YearsOfGuarantee = 5},
                        new Car { ID = 101, Model="Volvo", Price = 9000, YearOfProduction = 2003, YearsOfGuarantee = 6},
                        new Car { ID = 102, Model="BMW", Price = 11000, YearOfProduction = 2007, YearsOfGuarantee = 7},
                        new Car { ID = 103, Model="Lada", Price = 7000, YearOfProduction = 2011, YearsOfGuarantee = 4},
                    }
                );
        }
    }
}
