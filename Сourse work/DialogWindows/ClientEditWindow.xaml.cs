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

namespace DenisovArt_Kurs.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для ClientEditWindow.xaml
    /// </summary>
    public partial class ClientEditWindow : Window
    {
        Client c;
       	   
        public ClientEditWindow(Client _client)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            c = _client;
           
            if (String.IsNullOrEmpty(c.PassportNumber) == false)
            {
                if (c.BirthdayDate  != null)
                {
                    DateTime dt = c.BirthdayDate.Value;
                    dateBorn.Text =c.BirthdayDate.ToString();
                }
                tbxContacts.Text = c.ContactInformation;
                tbxFirName.Text = c.FitstName;                
                tbxSecName.Text = c.SecondName;
                tbxThirdName.Text = c.ThirdName;
                tbxPassport.Text = c.PassportNumber;
            }
            tbxFirName.DataContext = c;
            tbxSecName.DataContext = c;
            tbxThirdName.DataContext = c;
            tbxPassport.DataContext = c;

        } 

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(tbxFirName.Text) || String.IsNullOrEmpty(tbxSecName.Text) || String.IsNullOrEmpty(tbxContacts.Text)
                    || String.IsNullOrEmpty(tbxPassport.Text) || String.IsNullOrEmpty(dateBorn.Text))
                { MessageBox.Show("Все поля кроме отчества клиента обязательны для заполнения!"); return; }
            c.MyFitstName= tbxFirName.Text;
            c.MYSecondName= tbxSecName.Text;
            c.MyThirdName = tbxThirdName.Text;
            c.ContactInformation = tbxContacts.Text;
            c.MyPassportNumber = tbxPassport.Text;
            c.RegisterDate = DateTime.Now;
            c.BirthdayDate = Convert.ToDateTime(dateBorn.Text);
            DialogResult = true;
            }
            catch
            { }
        }

    }
}
