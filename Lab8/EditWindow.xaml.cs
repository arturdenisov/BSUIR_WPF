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

namespace Lab8
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        Car carNew;

        public EditWindow()
        {
            InitializeComponent();
        }

        public EditWindow(Car car):this()
        {
            carNew = car;
            if (String.IsNullOrEmpty(car.Model) == false)
            {
                tbGuarantee.Text    = car.YearsOfGuarantee.ToString();
                tbModel.Text        = car.Model.ToString();
                tbPrice.Text        = car.Price.ToString();
                tbProductin.Text    = car.YearOfProduction.ToString();
            }
        }

        private void btnCancel_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnOk_click(object sender, RoutedEventArgs e)
        {
            try
            {
                carNew.Model = Convert.ToString(tbModel.Text);
                carNew.Price = Convert.ToInt32(tbPrice.Text);
                carNew.YearOfProduction = Convert.ToInt32(tbProductin.Text);
                carNew.YearsOfGuarantee = Convert.ToInt32(tbGuarantee.Text);
                DialogResult = true;
            }
            catch
            {
                String errInputMessage = "***Input correct data.***";
                tbGuarantee.Text = tbModel.Text = tbPrice.Text = tbProductin.Text = errInputMessage;
            }
        }
    }
}
