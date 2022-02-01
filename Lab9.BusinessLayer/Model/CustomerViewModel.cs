using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Lab9.DataLayer.Entities;
using Lab9.DataLayer.Interfaces;
using Lab9.DataLayer.Repositories;
using System.Diagnostics;

namespace Lab9.BusinessLayer.Model
{
    public class CustomerViewModel
    {
        //Main properties
        public Int32 CustomerID { get; set; }
        public String FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String FileName { get; set; }
        public Boolean HasDiscount { get; set; }

        //XmlPacking
        public XElement XmlSerialization()
        {
            XElement xCustomert =
                    new XElement("Customer",
                        new XElement("CustomerID", CustomerID.ToString()),
                        new XElement("FullName", FullName.ToString()),
                        new XElement("DateOfBirth", DateOfBirth.ToString()),
                        new XElement("FileName", FileName.ToString()),
                        new XElement("HasDiscount", HasDiscount.ToString())
                            );
            return xCustomert;
        }
    }
}