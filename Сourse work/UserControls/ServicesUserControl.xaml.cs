using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DenisovArt_Kurs.DialogWindows;
using System.Xml.Linq;


namespace DenisovArt_Kurs.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ServicesUserControl.xaml
    /// </summary>
    public partial class ServicesUserControl : UserControl
    {		
        ProjectEntitiesContext context;
        IQueryable<CatalogOfService> servDB;
        IQueryable<DocSpeciality> docSpecDB;
        ObservableCollection<CatalogOfService> servObsColl;
        ObservableCollection<CatalogOfService> servSearchColl;
        ObservableCollection<DocSpeciality> docSpecsColl;
        CatalogOfService servises;

        public ServicesUserControl()
        {
            context = new ProjectEntitiesContext();
            InitializeComponent();
            dgrid.DataContext = ContextInitialization();
            combbSpecialities.ItemsSource = ComboBoxSpecialitiesInitialization();
            servises = new CatalogOfService();
           // tbxServiceID.DataContext = servises;
        }

        private ObservableCollection<CatalogOfService> ContextInitialization()
        {
            servDB = from d in context.CatalogOfServices select d;
            servObsColl = new ObservableCollection<CatalogOfService>();
            foreach (CatalogOfService d in servDB)
            {
                servObsColl.Add(d);
            }
            return servObsColl;
        }

        private ObservableCollection<DocSpeciality> ComboBoxSpecialitiesInitialization()
        {
            docSpecDB = from ds in context.DocSpecialities select ds;
            docSpecsColl = new ObservableCollection<DocSpeciality>();
            foreach (DocSpeciality ds in docSpecDB)
            {
                docSpecsColl.Add(ds);
            }
            return docSpecsColl;
        }

        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
            List<CatalogOfService> sList = new List<CatalogOfService>();
            if (String.IsNullOrEmpty(tbxServiceID.Text) == false)
            {
                foreach (CatalogOfService c in servObsColl)
                {
                    if (c.ServiceID == Convert.ToInt32(tbxServiceID.Text))
                    {
                        sList.Add(c);
                    }
                }
                FindAndSelect(sList);
                return;
            }
            if (String.IsNullOrEmpty(tbxServiceTitle.Text) == false)
            {
                foreach (CatalogOfService c in servObsColl)
                {
                    if (c.ServiceTitle == tbxServiceTitle.Text)
                    {
                        sList.Add(c);
                    }
                }
                FindAndSelect(sList);
                return;
            }
            if (combbSpecialities.SelectedItem != null)
            {
                foreach (CatalogOfService c in servObsColl)
                {
                    if (c.DocSpeciality == combbSpecialities.Text)
                    {
                        sList.Add(c);
                    }
                }
                FindAndSelect(sList);
                return;
            }
            }
            catch
            { }
        }

        private void FindAndSelect(List<CatalogOfService> sList)
        {
            servSearchColl = new ObservableCollection<CatalogOfService>();
            foreach (CatalogOfService c in sList)
            {
                servSearchColl.Add(c);
            }
            dgrid.DataContext = servSearchColl;
        }

        private void Reboot_Button_Click(object sender, RoutedEventArgs e)
        {
            if (servSearchColl != null)
            {
                servSearchColl.Clear();
                dgrid.DataContext = servObsColl;
                tbxServiceID.Text = tbxServiceTitle.Text = String.Empty;
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            CatalogOfService serv = new CatalogOfService();
            ServicesEditWindow dialog = new ServicesEditWindow(serv, docSpecsColl);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dialog.ShowDialog() == false) return;
            context.CatalogOfServices.Add(serv);
            context.SaveChanges();
            servObsColl.Add(serv);
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            CatalogOfService c;
            if (dgrid.SelectedIndex < 0)
            {
                MessageBox.Show("Укажите услугу для редактирования.");
                return;
            }
            else { c = (CatalogOfService)dgrid.SelectedItem; }
            ServicesEditWindow dialog = new ServicesEditWindow(c, docSpecsColl);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dialog.ShowDialog() == false) return;
            context.SaveChanges();
            dgrid.ItemsSource = null;
            dgrid.ItemsSource = servObsColl;

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex < 0)
            {
                MessageBox.Show("Для удаление необходимо указать услугу.");
                return;
            }

            List<CatalogOfService> removeList = dgrid.SelectedItems.Cast<CatalogOfService>().ToList();
            IQueryable<OrderedService> ordersDB = from o in context.OrderedServices select o;
            foreach (CatalogOfService s in removeList)
            {
                if (ordersDB.Where(o => o.ServiceID == s.ServiceID).Any())
                {
                    MessageBox.Show("Нельзя удалить услугу, на которую есть заказы.");
                }
                else
                {
                    context.CatalogOfServices.Remove(s);
                    context.SaveChanges();
                    servObsColl.Remove(s);
                }
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "ServicesCatalog"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            if (dlg.ShowDialog() == false) return;

            try
            {
                //check for correct file name
                XDocument.Load(dlg.FileName);
            }
            catch { }

            IEnumerable<CatalogOfService> servCatColl = context.CatalogOfServices;
            IEnumerable<Doctor> cColl = context.Doctors;
            IEnumerable<DocTimeTable> cTT = context.DocTimeTables;
            IEnumerable<Ticket> cTick = context.Tickets;

            XDocument doc = new XDocument(
                new XElement("CatalogOfServices",
                        from c in servCatColl
                        select c.ServicePassportXml()));
            doc.Save(dlg.FileName);
        }


        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "ServicesCatalog"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            if (dlg.ShowDialog() == false) return;

            try
            {
                //check for file existence
                XDocument test = XDocument.Load(dlg.FileName);
            }
            catch { }

            XDocument xDoc = XDocument.Load(dlg.FileName);
            IEnumerable<XElement> mainContainer = xDoc.Descendants();
            List<XElement> elementContainer = new List<XElement>();

            //Export collection from mainContainer to elemntContainer (can routing by index)
            foreach (XElement elem in mainContainer)
            {
                elementContainer.Add(elem);
            }

            //Find, create and add new services from XML-file
            List<CatalogOfService> tempList = new List<CatalogOfService>();
            for (Int32 i = 0; i < elementContainer.Count; i++)
            {
                if (elementContainer[i].Name.ToString() == "Service")
                {
                    //Creation
                    CatalogOfService sNew = new CatalogOfService
                    {
                        ServiceID = Convert.ToInt32(elementContainer[i + 1].Value),
                        ServiceTitle = elementContainer[i + 2].Value.ToString(),
                        Price = Convert.ToDecimal(elementContainer[i + 3].Value),
                        NeedTicket = Convert.ToBoolean(elementContainer[i + 4].Value),
                    };
                    Int32 sNewDocSpeciality = Convert.ToInt32(elementContainer[i + 5].Value);
                    DocSpeciality spec = (from o in context.DocSpecialities
                                          where o.SpecialityID == sNewDocSpeciality
                                          select o).First<DocSpeciality>();
                    sNew.DocSpecialities.Add(spec);
                    tempList.Add(sNew);
                }
            }
            
            foreach(CatalogOfService t in tempList)
            {
                if (context.CatalogOfServices.Where(d => (d.ServiceID == t.ServiceID || d.ServiceTitle == t.ServiceTitle)).Any<CatalogOfService>() == false)
                {
                    context.CatalogOfServices.Add(t);
                    context.SaveChanges();
                    servObsColl.Add(t);
                }

            }
        }

       }
}

