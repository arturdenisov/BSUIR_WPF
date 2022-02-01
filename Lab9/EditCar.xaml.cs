using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab9.BusinessLayer.Model;

namespace Lab9
{
    /// <summary>
    /// Логика взаимодействия для EditCar.xaml
    /// </summary>
    public partial class EditCar : Window
    {
        CarViewModel addCar = new CarViewModel();

        public EditCar(CarViewModel car)
        {
            InitializeComponent();
            dateSelling.Text = DateTime.Today.ToString();
            addCar = car;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addCar.Model = txModel.Text;
                addCar.Commence = Convert.ToDateTime(dateSelling.Text);
                addCar.BasePrice = Convert.ToDecimal(txbPrice.Text);
                DialogResult = true;
            }
            catch
            {
                String errInputMessage = "***Input correct data.***";
                txModel.Text = dateSelling.Text = txbPrice.Text = errInputMessage;
            }
        }
    }
}
