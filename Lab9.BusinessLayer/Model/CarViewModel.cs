using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Lab9.BusinessLayer.Model
{
    public class CarViewModel
    {
        //Main properties
        public Int32 CarID { get; set; }
        public String Model { get; set; }
        public DateTime Commence { get; set; }
        public Decimal BasePrice { get; set; }

        //Navigation properties
        public ObservableCollection<CustomerViewModel> Customers { get; set; }

        //XmlPacking
        public XElement XmlSerialization()
        {
            XElement xCar =
                    new XElement("Car",
                        new XElement("CarID", CarID.ToString()),
                        new XElement("Model", Model.ToString()),
                        new XElement("Commence", Commence.ToString()),
                        new XElement("BasePrice", BasePrice.ToString())
                            );
            return xCar;
        }
    }
}
