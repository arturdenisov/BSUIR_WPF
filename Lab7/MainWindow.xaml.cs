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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.ObjectModel;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<CarDB> cars = new ObservableCollection<CarDB>();

        public MainWindow()
        {
           InitializeComponent();
           listB.ItemsSource = cars;
            try
            {
                LoadSqlDb();
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить данные из базы данных SQL. Проверьте подключение.");
            }
        }


        private void Btn_InsertCar_Cick(object sender, RoutedEventArgs e)
        {
            try
            {
                (listB.SelectedItem as CarDB).Insert();
                LoadSqlDb();
            }
            catch
            {
                MessageBox.Show("For insert please input new data in the LAST row.", "***ERROR***");
            }
        }

        private void Btn_DeleteCar_click(object sender, RoutedEventArgs e)
        {
            try
            {
                (listB.SelectedItem as CarDB).Delete();
                LoadSqlDb();
            }
            catch
            {
                MessageBox.Show("For deletion please check a definite row.", "***ERROR***");
            }
        }

        private void Btn_UpdateCar_click(object sender, RoutedEventArgs e)
        {
            try
            {
                (listB.SelectedItem as CarDB).Update();
                LoadSqlDb();
            }
            catch
            {
                MessageBox.Show("For update please check a definite row.", "***ERROR***");
            }
        }

        private void LoadSqlDb()
        {
            cars.Clear();
            foreach (CarDB carItem in CarDB.LoadData())
            {
                cars.Add(carItem);
            }
        }

    }
}
