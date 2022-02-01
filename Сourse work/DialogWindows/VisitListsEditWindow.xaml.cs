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

namespace DenisovArt_Kurs.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для VisitListsEditWindow.xaml
    /// </summary>
    public partial class VisitListsEditWindow : Window
    {
        Client c;
        VisitList vl;
        ProjectEntitiesContext context;
        IQueryable<CatalogOfService> servicesDB;
        IQueryable<DocSpeciality> specialitiesDB;
        IQueryable<Ticket> ticketsDB;
        IQueryable<Doctor> doctorDB;
        ObservableCollection<CatalogOfService> servObsColl;
        ObservableCollection<CatalogOfService> servSearchColl;
        ObservableCollection<OrderedService> orderedServColl;
        ObservableCollection<DocSpeciality> specObsColl;
        ObservableCollection<Ticket> ticketsObsColl;
        ObservableCollection<Doctor> docObsColl;
        CatalogOfService catalog;

        public VisitListsEditWindow(Client _c, VisitList _vl, ProjectEntitiesContext _context)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            AdditionalInit(_c, _vl, _context);

            if (c.ClientID > 0) VisitListEditionInit();
            else DeactivateButtons();

            CalculateTotalPriceInit();

            dgrid_services.ItemsSource = servObsColl;
            combbSpecialities.ItemsSource = specObsColl;
            dgrid_order.ItemsSource = orderedServColl;
            combbSelectDate.SelectionChanged += combbSelectDate_SelectionChanged;
            combbSelectDoctor.SelectionChanged += combbSelectDoctor_SelectionChanged;

            //catalog = new CatalogOfService();
            //tbxServiceID.DataContext = catalog;
        }

        private void AdditionalInit(Client _c, VisitList _vl, ProjectEntitiesContext _context)
        {
            //Initialize main structures    
            c = _c;
            vl = _vl;
            context = _context;

            doctorDB = from d in context.Doctors select d;
            docObsColl = new ObservableCollection<Doctor>();
            foreach (Doctor d in doctorDB) { docObsColl.Add(d); }

            servicesDB = from c in context.CatalogOfServices select c;
            servObsColl = new ObservableCollection<CatalogOfService>();
            foreach (CatalogOfService serv in servicesDB) {servObsColl.Add(serv);}

            specialitiesDB = from c in context.DocSpecialities select c;
            specObsColl = new ObservableCollection<DocSpeciality>();
            foreach (DocSpeciality serv in specialitiesDB) { specObsColl.Add(serv); }

            ticketsDB = from c in context.Tickets select c;
            ticketsObsColl = new ObservableCollection<Ticket>();
            foreach (Ticket t in ticketsDB) { ticketsObsColl.Add(t); }

            orderedServColl = new ObservableCollection<OrderedService>();
        }

        private void VisitListEditionInit()
        {
            foreach (OrderedService o in vl.OrderedServices)
            {
               orderedServColl.Add(o);
            }
        }

        private void CalculateTotalPriceInit()
        {
            if (orderedServColl.Count == 0 ) tbxTotalPrice.Text = "0 руб.";
            Decimal result = 0;
            foreach (OrderedService o in orderedServColl)
            {
                result = result + o.Price;
            }
            tbxTotalPrice.Text = result.ToString() + " руб.";
        }

        private void DeactivateButtons()
        {
            btnDeleteService.IsEnabled = btnAddService.IsEnabled = false;
            combbSelectDate.IsEnabled = combbSelectDoctor.IsEnabled = combbSelectTicket.IsEnabled = false;
            btnSaveTicket.IsEnabled = btnDeleteTicket.IsEnabled = false;
            combbSelectDoctor.ItemsSource = null;
            combbSelectTicket.ItemsSource = null;
        }

        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
            List<CatalogOfService> servList = new List<CatalogOfService>();
            if (String.IsNullOrEmpty(tbxServiceID.Text) == false)
            {
                foreach (CatalogOfService c in servObsColl)
                {
                    
                    if (c.ServiceID.ToString() == tbxServiceID.Text)
                    {
                        servList.Add(c);
                    }
                }
                FindAndSelect(servList);
                return;
            }
            if (String.IsNullOrEmpty(tbxServiceTitle.Text) == false)
            {
                foreach (CatalogOfService c in servObsColl)
                {
                    if (c.ServiceTitle == tbxServiceTitle.Text)
                    {
                        servList.Add(c);
                    }
                }
                FindAndSelect(servList);
                return;
            }
            if (combbSpecialities.SelectedItem != null)
            {
                foreach (CatalogOfService c in servObsColl)
                {
                    if (c.DocSpeciality == combbSpecialities.Text)
                    {
                        servList.Add(c);
                    }
                }
                FindAndSelect(servList);
                return;
            }
            }
            catch
            { }
        }

        private void FindAndSelect(List<CatalogOfService> servList)
        {
            servSearchColl = new ObservableCollection<CatalogOfService>();
            foreach (CatalogOfService c in servList)
            {
                servSearchColl.Add(c);
            }
            dgrid_services.ItemsSource = servSearchColl;
        }

        private void Reboot_Button_Click(object sender, RoutedEventArgs e)
        {
            servSearchColl.Clear();
            dgrid_services.ItemsSource = servObsColl;
            tbxServiceID.Text = tbxServiceTitle.Text = String.Empty;
            combbSpecialities.SelectedIndex = 0;
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_services.SelectedItem == null) {
                MessageBox.Show("Чтобы добавить услугу в лист посещения необходимо ее выбрать.");
                return; };
            CatalogOfService cs = (CatalogOfService)dgrid_services.SelectedItem;

            OrderedService o = new OrderedService
            {
                ServiceID = cs.ServiceID,
                VisitListID = vl.VisitListID,
                TicketID = null,
                CatalogOfService = cs,
                Ticket =null,
                VisitList = vl
            };

            orderedServColl.Add(o);
            btnDeleteService.IsEnabled = true;
            CalculateTotalPriceInit();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_order.SelectedItem == null)
            {
                MessageBox.Show("Чтобы удалить услугу из листа посещения необходимо ее выбрать.");
                return;
            }
            OrderedService o = (OrderedService)dgrid_order.SelectedItem;
            orderedServColl.Remove(o);
            dgrid_order_SelectedCellsChanged(null,null);
            CalculateTotalPriceInit();
        }

        private void SaveTicket_Click(object sender, RoutedEventArgs e)
        {
            //Obtain selected ordered service to save a ticket
            OrderedService o = (OrderedService)dgrid_order.SelectedItem;

            //Check if any ticket is selcted
            if (combbSelectTicket.SelectedItem == null)
            {
                MessageBox.Show("Выберите талон.");
                return;
            }
            
            //Add ticket to the ordered service
            Ticket t = (Ticket)combbSelectTicket.SelectedItem;
            o.Ticket = t;
            //Mark that ticket is reserved
            o.Ticket.Reserved = true;

            //Refresh binding links with dgrid_ordered services
            //OrderedService old_o = orderedServColl.Single(old  => old.OrderServiceID == o.OrderServiceID);
            //old_o = o;
            dgrid_order.ItemsSource = null;
            dgrid_order.ItemsSource = orderedServColl;
            
        }

        private void DeleteTicket_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_order.SelectedItem == null)
            {
                MessageBox.Show("Вы не выбрали талон!");
                return;
            }

            //Obtain selected ordered service to save a ticket
            OrderedService o = (OrderedService)dgrid_order.SelectedItem;

            //Set that ticket is free and delete ticket from the ordered service
            if (o.Ticket == null || o.Ticket.Reserved == false)
            {
                MessageBox.Show("Для данной услуги талон отсутствует!");
                return;
            }

            o.Ticket.Reserved = false;        
            o.Ticket = null;
            
            //Refresh binding links with dgrid_ordered services
            dgrid_order.ItemsSource = null;
            dgrid_order.ItemsSource = orderedServColl;

        }

        private void btnVisitListResult_Click(object sender, RoutedEventArgs e)
        {
            vl.ClientID = c.ClientID;
            vl.VisitDate = DateTime.Now;
            Decimal result = 0;
            foreach (OrderedService o in orderedServColl)
            {
                if (o.NeedTicket == true && o.Ticket == null)
                {
                    MessageBox.Show("Нельзя сформировать заказ, не забронировав талон.");
                    return;
                }
                result = result + o.Price;
            }
            vl.PriceTotal = result;
            vl.Client = c;
            foreach (OrderedService o in orderedServColl)
            {
                vl.OrderedServices.Add(o);
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrderedService ordered in orderedServColl)
            {
                if (ordered.Ticket != null)
                {
                    ordered.Ticket.Reserved = false;
                    ordered.Ticket = null;
                }
            }
            Close();
        }

        private void dgrid_services_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgrid_services.SelectedItem == null) {btnAddService.IsEnabled = false;}
            else { btnAddService.IsEnabled = true; }
        }

        private void dgrid_order_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                if (orderedServColl.Count == 0 || dgrid_order.SelectedIndex < 0)
                {
                    DeactivateButtons();
                    return;
                }
                btnDeleteService.IsEnabled = true;
                OrderedService o = (OrderedService)dgrid_order.SelectedItem;
                if (o.NeedTicket & o.ServiceID > 0) SelectDefiniteTicket(o);
            }
            catch { }
        }

        private void SelectDefiniteTicket(OrderedService o)
        {            
            //Check do we have even 1 doctor
            bool tempIQ = (from d in docObsColl where d.SpecialityTitle == o.Speciality select d).Any();
            if (tempIQ == false) {return;}

            //Activate controls
            combbSelectDate.IsEnabled = combbSelectDoctor.IsEnabled = combbSelectTicket.IsEnabled = true;
            btnSaveTicket.IsEnabled = btnDeleteTicket.IsEnabled = true;

            //Get Doctors and put them into Combobox
            combbSelectDoctor.ItemsSource = (from d in docObsColl where d.SpecialityTitle == o.Speciality select d);

            //Escape problems with null-objects
            if (combbSelectDoctor.SelectedIndex < 0) combbSelectDoctor.SelectedIndex = 0;
            if (combbSelectDate.SelectedIndex < 0) combbSelectDate.SelectedIndex = 0;

            //Initiate combbox with tickets limnited with  selected speciality, date and doctors
            Doctor selectedDoctor = (Doctor)combbSelectDoctor.SelectedItem;

            DateTime selectedDateLowLimit = DateTime.Now;
            DateTime selectedDateHighLimit = new DateTime();
            if (combbSelectDate.SelectedIndex == 0) selectedDateHighLimit = DateTime.Now.AddYears(1);
            if (combbSelectDate.SelectedIndex == 1) selectedDateHighLimit = DateTime.Now.AddDays(5);
            if (combbSelectDate.SelectedIndex == 2) selectedDateHighLimit = DateTime.Now.AddDays(10);

            combbSelectTicket.ItemsSource = (from t in ticketsObsColl
                                             where t.DoctorSpeciality == o.Speciality &
                                             t.DoctorName == selectedDoctor.SecondName
                                             & t.VisitDateTime >= selectedDateLowLimit &
                                             t.VisitDateTime < selectedDateHighLimit &
                                             t.Reserved == false
                                             select t);
            combbSelectTicket.SelectedIndex = 0;
        }

        private void combbSelectDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           dgrid_order_SelectedCellsChanged(null, null);
        }

        private void combbSelectDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgrid_order_SelectedCellsChanged(null, null);
        }
    }
}
