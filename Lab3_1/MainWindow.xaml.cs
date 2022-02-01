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
using System.Collections.ObjectModel;

namespace Lab3_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        //Array of textboxes and corresponded Error_labels
        ObservableCollection<string> results;
        Calculator values;

        public MainWindow()
        {
            InitializeComponent();
            values = new Calculator();
            grid.DataContext = values;
            results = new ObservableCollection<string>();
            Lbox.DataContext = results;
        }

        //Start calculations
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in grid.Children.OfType<TextBox>())
            {
                if (Validation.GetHasError((DependencyObject)tb))
                {
                    MessageBox.Show("***Корректно заполните ВСЕ поля***\n");
                    return;
                }
            }

            //Clear previous results
            results.Clear();

            //Set new values
            List<String> helper = values.Calculate();
            foreach (String elem in helper)
            {
                results.Add(elem);
            }
        }
    }
}
