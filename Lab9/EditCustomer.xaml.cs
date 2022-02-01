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
    /// Логика взаимодействия для EditCustomer.xaml
    /// </summary>
    public partial class EditCustomer : Window
    {
        CustomerViewModel edCustomer;

        public EditCustomer(CustomerViewModel cust)
        {
            edCustomer = cust;
            InitializeComponent();
            if (cust.CustomerID == 0)
            {
                dateBorn.Text = DateTime.Today.ToString();
            }
            else
            {
                txbName.Text = cust.FullName;
                dateBorn.Text = cust.DateOfBirth.ToString();
                dateBorn.DataContext = cust.DateOfBirth;
                txbImage.Text = cust.FileName;
                chbBonus.IsChecked = cust.HasDiscount;
                //chbBonus
            }
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
                edCustomer.FullName = txbName.Text;
                edCustomer.DateOfBirth = Convert.ToDateTime(dateBorn.Text);
                edCustomer.FileName = txbImage.Text;
                edCustomer.HasDiscount = Convert.ToBoolean(chbBonus.IsChecked);
                DialogResult = true;
            }
            catch
            {
                String errInputMessage = "***Input correct data.***";
                txbName.Text = dateBorn.Text = txbImage.Text = errInputMessage;
            }
        }
    }
}
