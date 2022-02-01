using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DenisovArt_Kurs.DialogWindows;
using System.Xml.Linq;
using System.ComponentModel;

namespace DenisovArt_Kurs.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ClientsUserControl.xaml
    /// </summary>
    public partial class ClientsUserControl : UserControl //, IDataErrorInfo
    {
        ProjectEntitiesContext context;
        MainWindow owner;
        ClientEditWindow cW;
        IQueryable<Client> clientsDB;
        ObservableCollection<Client> clientsObsColl;
        ObservableCollection<Client> clientsSearchColl;
        Client chel; //для привязки к текстбоксам - используется при валидации

        public ClientsUserControl(MainWindow mW)
        {
            owner = mW;
            InitializeComponent();
            dgrid.DataContext = ContextInitialization();
            chel = new Client();
        }
        
        private ObservableCollection<Client> ContextInitialization()
        {
            context = new ProjectEntitiesContext();

            clientsDB = from c in context.Clients
                        select c;
            clientsObsColl = new ObservableCollection<Client>();
            foreach (Client c in clientsDB)
            {
                clientsObsColl.Add(c);
            }
            return clientsObsColl;
        }

        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Client> cList = new List<Client>();
                if (String.IsNullOrEmpty(tbxClientID.Text) == false)
                {
                    foreach (Client c in clientsObsColl)
                    {
                        if (c.ClientID == Convert.ToInt32(tbxClientID.Text))
                        {
                            cList.Add(c);
                        }
                    }
                    FindAndSelect(cList);
                    return;
                }
                if (String.IsNullOrEmpty(tbxPassport.Text) == false)
                {
                    foreach (Client c in clientsObsColl)
                    {
                        if (c.PassportNumber == tbxPassport.Text)
                        {
                            cList.Add(c);
                        }
                    }
                    FindAndSelect(cList);
                    return;
                }
                if (String.IsNullOrEmpty(tbxSecondary.Text) == false)
                {
                    foreach (Client c in clientsObsColl)
                    {
                        if (c.SecondName == tbxSecondary.Text)
                        {
                            cList.Add(c);
                        }
                    }
                    FindAndSelect(cList);
                    return;
                }
            }
            catch
            { }

        }

        private void FindAndSelect(List<Client> cList)
        {
            clientsSearchColl = new ObservableCollection<Client>();
            foreach (Client c in cList)
            {
                clientsSearchColl.Add(c);
            }
            dgrid.DataContext = clientsSearchColl;
        }

        private void SelectClient_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex == -1) { MessageBox.Show("В таблице не выбран клиент!"); return; }
            Client c = (Client)dgrid.SelectedItem;
            owner.StatusBar_ClientFullName.Text = c.SecondName + " " + c.FitstName + " " + c.ThirdName;
            owner.StatusBar_ClientID.Text = c.ClientID.ToString();
            owner.StatusBar_ClientID.Style = owner.StatusBar_ClientFullName.Style = (Style)FindResource("StatusBar_ACTIVE");
        }

        private void Reboot_Button_Click(object sender, RoutedEventArgs e)
        {
            if(clientsSearchColl != null)
            { 
                clientsSearchColl.Clear();
                dgrid.DataContext = clientsObsColl;
                tbxClientID.Text = tbxPassport.Text = tbxSecondary.Text = String.Empty;
            }
            
            owner.StatusBar_ClientFullName.Text = "xxxxxxx";
            owner.StatusBar_ClientID.Text = "xxxxxxx";
            owner.StatusBar_ClientID.Style = owner.StatusBar_ClientFullName.Style = (Style)FindResource("StatusBar_PASSIVE");
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Client c = new Client();
            cW = new ClientEditWindow(c);
            cW.Owner = owner;
            cW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (cW.ShowDialog() == true)
            {
                context.Clients.Add(c);
                context.SaveChanges();
                clientsObsColl.Add(c);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Client c;
            if (dgrid.SelectedItem != null)
            {
                c = (Client)dgrid.SelectedItem;
                cW = new ClientEditWindow(c);
                cW.Owner = owner;
                cW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (cW.ShowDialog() == true)
                {
                    context.SaveChanges();
                    dgrid.DataContext = null;
                    dgrid.DataContext = clientsObsColl;
                }
            }
            else { MessageBox.Show("Редактирование невозможно! В таблице не выбран клиент!"); return; }
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgrid.SelectedIndex < 0)
                {
                    MessageBox.Show("Для удаление необходимо указать клиента.");
                    return;
                }
                List<Client> removeList = dgrid.SelectedItems.Cast<Client>().ToList();
                IQueryable<VisitList> visitListDB = from v in context.VisitLists select v;
                foreach (Client c in removeList)
                {
                    if (visitListDB.Where(v => v.ClientID == c.ClientID).Any())
                    {
                        MessageBox.Show("Нельзя удалить клиента, у которого есть заказы.");
                    }
                    else
                    { 
                        context.Clients.Remove(c);
                        context.SaveChanges();
                        clientsObsColl.Remove(c);
                    }
                }
            }
            catch { MessageBox.Show("Удаление невозможно, пока у клиентов есть заказы."); }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "ClientsList"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            if (dlg.ShowDialog() == false) return;

            try
            {
                //check for correct file name
                XDocument.Load(dlg.FileName);
            }
            catch { }

            IEnumerable<Client> cColl = context.Clients;
            XDocument doc = new XDocument(new XElement("Clients",
                    from c in cColl
                    select c.ClientPassportXml()));
            doc.Save(dlg.FileName);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "ClientsList"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            if (dlg.ShowDialog() == false) return;

            try
            {
                //check for file existence
                XDocument test = XDocument.Load(dlg.FileName);
            }
            catch {}

            XDocument xDoc = XDocument.Load(dlg.FileName);
            IEnumerable<XElement> mainContainer = xDoc.Descendants();
            List<XElement> elementContainer = new List<XElement>();

            //Export collection from mainContainer to elemntContainer (can routing by index)
            foreach (XElement elem in mainContainer)
            {
                elementContainer.Add(elem);
            }

            //Find, create and add new clients from XML-file
            for (Int32 i = 0; i < elementContainer.Count; i++)
            {
                if (elementContainer[i].Name.ToString() == "Client")
                {
                    if (clientsObsColl.Where(q => q.ClientID == Convert.ToInt32(elementContainer[i + 1].Value)).Any())  continue;
                    //Creation
                    Client cNew = new Client
                    {
                        ClientID = Convert.ToInt32(elementContainer[i + 1].Value),
                        RegisterDate = DateTime.Now,
                        FitstName = elementContainer[i + 3].Value,
                        SecondName = elementContainer[i + 4].Value,
                        ThirdName = elementContainer[i + 5].Value,
                        PassportNumber = elementContainer[i + 6].Value,
                        ContactInformation = elementContainer[i + 7].Value,
                        BirthdayDate = Convert.ToDateTime(elementContainer[i + 8].Value)
                    };
                    context.Clients.Add(cNew);
                    context.SaveChanges();
                    clientsObsColl.Add(cNew);
                }
            }
        }
    }
}

