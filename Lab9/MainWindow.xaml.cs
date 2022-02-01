using System;
using System.Windows;
using Lab9.BusinessLayer.Interfaces;
using Lab9.BusinessLayer.Model;
using Lab9.BusinessLayer.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Diagnostics;

namespace Lab9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<CarViewModel> cars;
        ICarService carService;

        public MainWindow()
        {
            InitializeComponent();
            carService = new CarServise("TestDbConnection");
            cars = carService.GetAll();
            cBoxGroup.DataContext = cars;
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            CarViewModel car = new CarViewModel();
            var dialog = new EditCar(car);
            var result = dialog.ShowDialog();
            if (result == true)
            {
                carService.CreateCar(car);
                cars = carService.GetAll();
                cBoxGroup.DataContext = cars;
                cBoxGroup.SelectedIndex = 0;
            }
        }

        private void DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            if (cBoxGroup.SelectedIndex >= 0)
            {
                Int32 SelectedIndex = cBoxGroup.SelectedIndex;
                CarViewModel delCar = (CarViewModel)cBoxGroup.SelectedItem;
                cars.Remove(delCar);
                carService.DeleteCar(delCar.CarID);
                cBoxGroup.SelectedIndex = SelectedIndex - 1;
            }
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
                CustomerViewModel customer = new CustomerViewModel();
                var dialog = new EditCustomer(customer);
                var result = dialog.ShowDialog();
                if (result == true)
                {
                    var selectedCar = (CarViewModel)cBoxGroup.SelectedItem;
                    var selectedItemIndex = cBoxGroup.SelectedIndex;
                    carService.AddCustomerToCar(selectedCar.CarID, customer);
                    cBoxGroup.DataContext = carService.GetAll();
                    cBoxGroup.SelectedIndex = selectedItemIndex;
                }
        }

        private void EditCustomer_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CustomerViewModel customer = (CustomerViewModel)lbCustomers.SelectedItem;
            var dialog = new EditCustomer(customer);
            var result = dialog.ShowDialog();
            if (result == true)
            {
                var selectedCar = (CarViewModel)cBoxGroup.SelectedItem;
                var selectedItemIndex = cBoxGroup.SelectedIndex;
                carService.UpdateCustomer(customer);
                cBoxGroup.DataContext = carService.GetAll();
                cBoxGroup.SelectedIndex = selectedItemIndex;
            }
        }


        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (lbCustomers.SelectedIndex >= 0)
            {
                CustomerViewModel delCustomer = (CustomerViewModel)lbCustomers.SelectedItem;
                CarViewModel selectedCar = (CarViewModel)cBoxGroup.SelectedItem;
                var selectedItemIndex = cBoxGroup.SelectedIndex;
                Int32 selectedCarIndex = cBoxGroup.SelectedIndex; 
                selectedCar.Customers.Remove(delCustomer);
                ((CarServise)carService).RemoveCustomerFromCar(selectedCar.CarID, delCustomer.CustomerID);
                cBoxGroup.DataContext = carService.GetAll();
                cBoxGroup.SelectedIndex = selectedItemIndex;
            }
        }

        private void SaveXml_Click(object sender, RoutedEventArgs e)
        {
            //Set properties of dialog window
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Lab9_DenisovAY";
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";
            if (dlg.ShowDialog() == false) return;
            try
            {
                XDocument.Load(dlg.FileName);
            }
            catch { }

            //Get full list of elements for XML-conversion
            ObservableCollection<CarViewModel> cars = carService.GetAll();

            //Form XML-doc
            XDocument doc = new XDocument(new XElement("Lab9_DataBase"));
            doc.Element("Lab9_DataBase").Add(new XElement("Cars"));
            foreach (CarViewModel car in cars)
            {
                doc.Element("Lab9_DataBase").Element("Cars").Add(
                    new XElement(car.XmlSerialization()));
                if(car.Customers.Count > 0)
                {
                    XNode xnode = doc.Element("Lab9_DataBase").Element("Cars").LastNode;
                    ((XContainer)xnode).Add(new XElement("Customers"));


                    foreach (CustomerViewModel customer in car.Customers)
                    {
                        ((XContainer)xnode).Element("Customers").Add(
                            new XElement(customer.XmlSerialization()));
                    }
                }
            }

            //Save the XML-doc
            doc.Save(dlg.FileName);
        }  

        private void LoadXMml_Click(object sender, RoutedEventArgs e)
        {
            //Import database from XML-file
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Orders"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            if (dlg.ShowDialog() == false) return;

            try
            {
                //check for file existence
                XDocument test = XDocument.Load(dlg.FileName);
            }
            catch { }

            XDocument xDoc = XDocument.Load(dlg.FileName);
            IEnumerable<XElement> mainContainer = xDoc.Descendants();
            List<XElement> elementContainer = new List<XElement>();

            //Form collection from mainContainer to elemntContainer (can routing by index)
            foreach (XElement elem in mainContainer)
            {
                elementContainer.Add(elem);
            }

            ObservableCollection<CarViewModel> _existedCars = carService.GetAll();
            ObservableCollection<CustomerViewModel> customers = new ObservableCollection<CustomerViewModel>();
            CarViewModel car = new CarViewModel();

            //Find and create new Cars and Customers from XML-file
            for (Int32 i = 0; i < elementContainer.Count; i++)
            {
                
                if (elementContainer[i].Name.ToString() == "Car")
                {
                    car = new CarViewModel
                    {
                        CarID = Convert.ToInt32(elementContainer[i + 1].Value.ToString()),
                        Model = elementContainer[i + 2].Value.ToString(),
                        Commence = Convert.ToDateTime(elementContainer[i + 3].Value.ToString()),
                        BasePrice = Convert.ToDecimal(elementContainer[i + 4].Value.ToString()),
                    };
                    AddCarToContext(car, _existedCars);
                }
                if (elementContainer[i].Name.ToString() == "Customer")
                {
                    CustomerViewModel cust = new CustomerViewModel
                    {
                        CustomerID = Convert.ToInt32(elementContainer[i + 1].Value.ToString()),
                        FullName = elementContainer[i + 2].Value.ToString(),
                        DateOfBirth = Convert.ToDateTime(elementContainer[i + 3].Value.ToString()),
                        FileName = elementContainer[i + 4].Value.ToString(),
                        HasDiscount = Convert.ToBoolean(elementContainer[i + 5].Value.ToString())
                    };
                    AddCustomerToCContext(cust, car.Model);
                }
            }
        }

        private void AddCarToContext(CarViewModel car, ObservableCollection<CarViewModel> _existedCars)
        {
            if (_existedCars.Where(exc => exc.Model == car.Model).Any<CarViewModel>() == false)
            {
                carService.CreateCar(car);
                RebootDBContext();
            }
        }

        private void AddCustomerToCContext(CustomerViewModel cust, string model)
        {
            //Find a car to which we will add a customer
            CarViewModel car = carService.GetAll().Single(c => c.Model == model);
            ObservableCollection<CustomerViewModel> _existedCustomers = car.Customers; 

            if (_existedCustomers.Where(exc => exc.FullName == cust.FullName).Any<CustomerViewModel>() == false)
            {
                //Add customer to the car
                carService.AddCustomerToCar(car.CarID, cust);
                RebootDBContext();
            }
        }

        private void RebootDBContext()
        {
            Int32 selectedItemIndex = cBoxGroup.SelectedIndex;
            cBoxGroup.DataContext = carService.GetAll();
            if (cBoxGroup.SelectedIndex < 0) cBoxGroup.SelectedIndex = 0;
            else cBoxGroup.SelectedIndex = selectedItemIndex;
        }
    }
}
