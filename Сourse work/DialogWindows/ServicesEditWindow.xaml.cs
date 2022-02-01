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
using System.Collections.ObjectModel;
using DenisovArt_Kurs.UserControls;

namespace DenisovArt_Kurs.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для ServicesEditWindow.xaml
    /// </summary>
    public partial class ServicesEditWindow : Window
    {
        ProjectEntitiesContext context;
        ObservableCollection<DocSpeciality> docSpecsColl;
        CatalogOfService serv;
        DocSpeciality speciality;

        public ServicesEditWindow(CatalogOfService _cat, ObservableCollection<DocSpeciality> _specColl)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            context = new ProjectEntitiesContext();
            docSpecsColl = _specColl;
            InitializeComponent();
            ServiceInit(_cat);
            combbSpecialities.ItemsSource = docSpecsColl;
            tbxPrice.DataContext = _cat;
        }

        private void ServiceInit(CatalogOfService _cat)
        {
            serv = _cat;
            speciality = new DocSpeciality();
            if (String.IsNullOrEmpty(serv.ServiceTitle) == false)
            {
                tbxPrice.Text = serv.Price.ToString();
                tbxServiceTitle.Text = serv.ServiceTitle;
                combbSpecialities.SelectedIndex = docSpecsColl.IndexOf(docSpecsColl.Where(s => s.SpecialityID == serv.DocSpecialityID).First());
                combbTicketNeed.SelectedIndex = Convert.ToInt32(serv.NeedTicket);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(tbxServiceTitle.Text) || String.IsNullOrEmpty(tbxPrice.Text))
                { MessageBox.Show("Все поля обязательны для заполнения!"); return; }
                if (Convert.ToInt32(tbxPrice.Text) < 0)
                { MessageBox.Show("Стоимость не может быть отрицательной!"); return; }

                //Initiate a service.properties
                serv.Price = Convert.ToDecimal(tbxPrice.Text);
                serv.ServiceTitle = tbxServiceTitle.Text;
                serv.NeedTicket = Convert.ToBoolean(combbTicketNeed.SelectedIndex);

                //Initiate a docSpecialities.properties 
                speciality = (DocSpeciality)combbSpecialities.SelectedItem;

                serv.DocSpecialities.Clear();
                serv.DocSpecialities.Add(speciality);
                DialogResult = true;
            }
            catch
            { }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
    }
}
