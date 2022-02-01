using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace Lab8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AutoCarDbEntities context;

        public MainWindow()
        {
            context = new AutoCarDbEntities("TestDbConnection");
            InitializeComponent();
            InitCarsList();

        }

        private void InitCarsList()
        {
            context.Cars.Load();
            dGrid.DataContext = context.Cars.Local;
        }

        private void dGrid_NumerateLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void dGridReload()
        {
            //reload DataContext
            dGrid.DataContext = null;
            dGrid.DataContext = context.Cars.Local;
        }

        //Не позволяет добавлять объекты или их редактировать через изменение отдельной ячейки
        private void dGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Событие возникает после окончания редактирования ячейки ДО его утверждения или отмены системой
            dGridReload();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((dGrid.SelectedItem is Car) == false ) return;
            if (((Car)dGrid.SelectedItem).ID == 0) return;
            else
            {
                Car car = dGrid.SelectedItem as Car;
                EditWindow ew = new EditWindow(car);
                var result = ew.ShowDialog();
                if (result == true && car != null)
                {
                    context.SaveChanges();
                    ew.Close();
                    dGridReload();
                }
                else
                {
                    //return starting value
                    context.Entry(car).Reload();
                    dGridReload();
                }
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            Car car = new Car();
            EditWindow ew = new EditWindow(car);
            var result = ew.ShowDialog();
            if (result == true)
            {
                context.Cars.Add(car);
                context.SaveChanges();
                ew.Close();
                dGridReload();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((dGrid.SelectedItem is Car) == false) return;
            else
            {
                Car car = dGrid.SelectedItem as Car;
                if (car != null)
                {
                    context.Cars.Remove(car);
                    context.SaveChanges();
                    dGridReload();
                }
            }
        }

    }
}
