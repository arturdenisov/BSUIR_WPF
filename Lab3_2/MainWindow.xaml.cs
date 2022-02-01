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
using System.IO;

namespace Lab3_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private List<Person> database;
        private Person personTemplate;
        private int recordNumber = 0;

        public MainWindow()
        {
            database = new List<Person>();
            personTemplate = new Person();
            InitializeComponent();
            AppGrid.DataContext = personTemplate;
        }

        private void AddToDatabase_Click(object sender, RoutedEventArgs e)
        {
            //Check for empty controls
            foreach (UIElement ui in AppGrid.Children)
            {
                switch (ui.GetType().ToString())
                {
                    case "System.Windows.Controls.TextBox":
                        {
                            if ((ui as TextBox).Text == String.Empty || Validation.GetHasError((DependencyObject)ui))
                            {
                                MessageBox.Show("***Проверьте правильность заполнения ВСЕХ полей.***");
                                return;
                            }
                            break;
                        }
                    case "System.Windows.Controls.ComboBox":
                        {
                            if ((ui as ComboBox).Text == String.Empty || Validation.GetHasError((DependencyObject)ui))
                            {
                                MessageBox.Show("***Проверьте правильность заполнения ВСЕХ полей.***");
                                return;
                            }
                            break;
                        }
                }
            }

            //Add info about new person
            ListBoxItem lbxi = new ListBoxItem();
            personTemplate.PersID = ++recordNumber;
            lbxi.Content = personTemplate.Passport();
            Lbox.Items.Add(lbxi);
            database.Add(personTemplate);
            personTemplate = new Person();
            AppGrid.DataContext = personTemplate;

            //Reset all controls
            ResetInputControls();
        }

        private void ResetInputControls()
        {
            Txb_SecondName.Text = String.Empty;
            Txb_City.Text = String.Empty;
            Txb_Status.Text = String.Empty;
            Txb_Street.Text = String.Empty;
            Txb_HouseNum.Text = String.Empty;
        }

        private void SaveInTextFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.FileName = "Lab3.2_saved"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Start to write a file
            StreamWriter writer = new StreamWriter(dlg.OpenFile());

            //Write full daatabase in a file
            foreach (Person person in database)
            {
                writer.Write(person.Passport());
            }
            writer.Close();
        }
    }
}
