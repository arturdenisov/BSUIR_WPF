using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.DataLayer.Entities
{
    public class Car
    {
        //Main properties
        public Int32 CarID { get; set; }
        public String Model { get; set; }
        public DateTime Commence { get; set; }
        public Decimal BasePrice { get; set; }
        //Navigation properties
        public List<Customer> Customers { get; set; }
    }
}
