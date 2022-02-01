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
    public partial class VisitListsUserControl : UserControl
    {

        ProjectEntitiesContext context;
        MainWindow owner;
        IQueryable<VisitList> visitsDB;
        ObservableCollection<VisitList> visitObsColl;
        ObservableCollection<VisitList> visitSearchColl;

        public VisitListsUserControl(MainWindow mW)
        {			
            owner = mW;
            InitializeComponent();
            dgrid.DataContext = ContextInitialization();
        }

        private ObservableCollection<VisitList> ContextInitialization()
        {
            context = new ProjectEntitiesContext();
            visitsDB = from c in context.VisitLists
                        select c;
            visitObsColl = new ObservableCollection<VisitList>();
            foreach (VisitList c in visitsDB)
            {
                visitObsColl.Add(c);
            }
            return visitObsColl;
        }

        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<VisitList> vList = new List<VisitList>();
                if (String.IsNullOrEmpty(tbxOrderId.Text) == false)
                {
                    foreach (VisitList c in visitObsColl)
                    {
                        if (c.VisitListID.ToString() == tbxOrderId.Text)
                        {
                            vList.Add(c);
                        }
                    }
                    FindAndSelect(vList);
                    return;
                }
                if (String.IsNullOrEmpty(tbxClientId.Text) == false)
                {
                    foreach (VisitList c in visitObsColl)
                    {
                        if (c.ClientID.ToString() == tbxClientId.Text)
                        {
                            vList.Add(c);
                        }
                    }
                    FindAndSelect(vList);
                    return;
                }
                if (String.IsNullOrEmpty(tbxClientName.Text) == false)
                {
                    foreach (VisitList c in visitObsColl)
                    {
                        if (c.ClintName == tbxClientName.Text)
                        {
                            vList.Add(c);
                        }
                    }
                    FindAndSelect(vList);
                    return;
                }
            }          
            catch
            { }
        }

        private void FindAndSelect(List<VisitList> vList)
        {
            visitSearchColl = new ObservableCollection<VisitList>();
            foreach (VisitList c in vList)
            {
                visitSearchColl.Add(c);
            }
            dgrid.DataContext = visitSearchColl;
        }

        private void Reboot_Button_Click(object sender, RoutedEventArgs e)
        {
            if (visitSearchColl != null)
            {
                visitSearchColl.Clear();
                dgrid.DataContext = visitObsColl;
                tbxClientId.Text = tbxClientName.Text = tbxOrderId.Text = String.Empty;
            }
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            //Check if visit list selected
            if(dgrid.SelectedItem == null) { MessageBox.Show("Для редактирования надо указать лист посещения."); return; }
            VisitList v = (VisitList)dgrid.SelectedItem;

            //Set a client info in the status bar
            owner.StatusBar_ClientID.Text = v.Client.ClientID.ToString();
            owner.StatusBar_ClientFullName.Text = v.Client.SecondName + " " + v.Client.FitstName + " " + v.Client.ThirdName;
            owner.StatusBar_ClientID.Style = owner.StatusBar_ClientFullName.Style = (Style)FindResource("StatusBar_ACTIVE");

            //Create DialogWindow for visitList edition
            VisitListsEditWindow vW = new VisitListsEditWindow(v.Client, v, context);
            if (vW.ShowDialog() == false) return;
            context.SaveChanges();
            dgrid.DataContext = null;
            dgrid.DataContext = visitObsColl;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            //Check is a client selected
            try { Convert.ToInt32(owner.StatusBar_ClientID.Text); }
            catch { MessageBox.Show("Для оформления заказа выберите клиента из базы данных КЛИЕНТЫ."); return; }

            Int32 check = Convert.ToInt32(owner.StatusBar_ClientID.Text);
            Client cSelected = (from c in context.Clients
                                where c.ClientID == check
                                select c).First();
            VisitList vl = new VisitList();

            VisitListsEditWindow vW = new VisitListsEditWindow(cSelected, vl, context);
            if (vW.ShowDialog() == false) return;
            context.VisitLists.Add(vl);
            context.SaveChanges();
            visitObsColl.Add(vl);
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedItem == null) {  MessageBox.Show("Выберите лист для удаления"); return; }
            List<VisitList> vList = dgrid.SelectedItems.Cast<VisitList>().ToList();
            foreach (VisitList vl in vList)
            {
                foreach (OrderedService o in vl.OrderedServices)
                {
                    if(o.TicketID != null) o.Ticket.Reserved = false;
                }
                context.VisitLists.Remove(vl);
                visitObsColl.Remove(vl);
            }
            context.SaveChanges();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Orders"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            if (dlg.ShowDialog() == false) return;

            try
            {
                //check for correct file name
                XDocument.Load(dlg.FileName);
            }
            catch { }

            IEnumerable<VisitList> vlColl = context.VisitLists;
            IEnumerable<OrderedService> osTT = context.OrderedServices;
            IEnumerable<Ticket> cTick = context.Tickets;

            XDocument doc = new XDocument(
                new XElement("Orders",
                    new XElement("VisitLists",
                        from c in vlColl
                        select c.VisitListPassportXml()),
                     new XElement("OrderedServices",
                        from tt in osTT
                        select tt.OrderedServPassportXml()),
                    new XElement("Tickets",
                        from t in cTick
                        where t.Reserved == true
                        select t.TickPassportXml())));
            doc.Save(dlg.FileName);

        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Orders"; // Default file name
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

            List<VisitList> vlColl = new List<VisitList>();
            List<OrderedService> osColl = new List<OrderedService>();
            List<Ticket> tickColl = new List<Ticket>();

            //Find and create new VisitLists, OrderedServices and Tickets from XML-file
            for (Int32 i = 0; i < elementContainer.Count; i++)
            {
                VisitList vNew;
                OrderedService osNew;
                Ticket tick;

                if (elementContainer[i].Name.ToString() == "VisitList")
                {
                    vNew = new VisitList
                    {
                        VisitListID = Convert.ToInt32(elementContainer[i + 1].Value.ToString()),
                        ClientID = Convert.ToInt32(elementContainer[i + 2].Value.ToString()),
                        VisitDate = Convert.ToDateTime(elementContainer[i + 3].Value.ToString()),
                        PriceTotal = Convert.ToDecimal(elementContainer[i + 4].Value.ToString()),
                    };
                    vlColl.Add(vNew);
                }
                if (elementContainer[i].Name.ToString() == "OrderedService")
                {
                    string x = elementContainer[i + 3].Value.ToString();

                    //If ticket != null
                    if (String.IsNullOrEmpty(x) == false)
                    {
                        osNew = new OrderedService
                        {
                            ServiceID = Convert.ToInt32(elementContainer[i + 1].Value),
                            VisitListID = Convert.ToInt32(elementContainer[i + 2].Value),
                            TicketID = Convert.ToInt32(elementContainer[i + 3].Value)
                        };
                        osColl.Add(osNew);
                    }
                    else
                    {
                        osNew = new OrderedService
                        {
                            ServiceID = Convert.ToInt32(elementContainer[i + 1].Value),
                            VisitListID = Convert.ToInt32(elementContainer[i + 2].Value),
                            TicketID = null
                        };
                        osColl.Add(osNew);

                        osColl.Add(osNew);
                    }
                    
                }

                if (elementContainer[i].Name.ToString() == "Ticket")
                {
                    tick = new Ticket
                    {
                        TicketID = Convert.ToInt32(elementContainer[i + 1].Value),
                        DocTimetableID = Convert.ToInt32(elementContainer[i + 2].Value),
                        VisitDateTime = Convert.ToDateTime(elementContainer[i + 3].Value),
                        Reserved = Convert.ToBoolean(elementContainer[i + 4].Value)
                    };
                    tickColl.Add(tick);
                }
            }
            //Add Tickets to OrderedServices
            for (Int32 i = 0; i < osColl.Count; i++)
            {
                foreach (Ticket tick in tickColl)
                {
                    if (tick.TicketID == osColl[i].TicketID)
                    {
                        osColl[i].Ticket = tick;
                    }
                }
            }
            //Add OrderedServices to VisitLists
            for (Int32 i = 0; i < vlColl.Count; i++)
            {
                foreach (OrderedService tt in osColl)
                {
                    if (tt.VisitListID == vlColl[i].VisitListID)
                    {
                        vlColl[i].OrderedServices.Add(tt);
                    }
                }
            }
            //Add VisitLists to the Context
            foreach (VisitList v in vlColl)
            {
                if (context.VisitLists.Where(d => d.VisitListID == v.VisitListID).Any<VisitList>() == false)
                {
                    context.VisitLists.Add(v);
                    context.SaveChanges();
                    visitObsColl.Add(v);
                }
            }
        }
    }
}