using System;
using System.Data.Entity;

namespace Lab8
{
    class AutoCarDbEntities:DbContext
    {
        public AutoCarDbEntities(String connectionString) : base(connectionString)
        {
           Database.SetInitializer(new AutoCarDbInit());
        }

        public DbSet<Car> Cars { get; set; }
    }

}
