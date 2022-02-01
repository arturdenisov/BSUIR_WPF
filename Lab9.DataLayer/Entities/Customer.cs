using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.DataLayer.Entities
{
    public class Customer
    {
        //Main properties
        public Int32 CustomerID { get; set; }
        public String FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String FileName { get; set; }
        public Decimal PersonalPrice { get; set; }
        //Navigation properties
        public Int32 CarID { get; set; }
        public Car Cars { get; set; }
    }
}
