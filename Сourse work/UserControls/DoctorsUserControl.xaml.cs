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
    /// Логика взаимодействия для DoctorsUserControl.xaml
    /// </summary>	
    public partial class DoctorsUserControl : UserControl
    {
        MainWindow owner;
        ProjectEntitiesContext context;
        IQueryable<Doctor> docDB;
        IQueryable<DocSpeciality> docSpecDB;
        ObservableCollection<Doctor> docsObsColl;
        ObservableCollection<Doctor> docsSearchColl;
        ObservableCollection<DocSpeciality> docSpecsColl;
        Doctor docchel;

        public DoctorsUserControl()
        {
            context = new ProjectEntitiesContext();
            InitializeComponent();
            dgrid.DataContext = ContextInitialization();
            combbSpecialities.ItemsSource = ComboBoxSpecialitiesInitialization();
            docchel = new Doctor();
        }

        private ObservableCollection<Doctor> ContextInitialization()
        {
            docDB = from d in context.Doctors select d;
            docsObsColl = new ObservableCollection<Doctor>();
            foreach (Doctor d in docDB)
            {
                docsObsColl.Add(d);
            }
            return docsObsColl;
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
            List<Doctor> cList = new List<Doctor>();
            if (String.IsNullOrEmpty(tbxDocID.Text) == false)
            {
                foreach (Doctor c in docsObsColl)
                {
                    if (c.DocID == Convert.ToInt32(tbxDocID.Text))
                    {
                        cList.Add(c);
                    }
                }
                FindAndSelect(cList);
                return;
            }
            if (String.IsNullOrEmpty(tbxSecName.Text) == false)
            {
                foreach (Doctor c in docsObsColl)
                {
                    if (c.SecondName == tbxSecName.Text)
                    {
                        cList.Add(c);
                    }
                }
                FindAndSelect(cList);
                return;
            }
            if (combbSpecialities.SelectedItem != null)
            {
                foreach (Doctor c in docsObsColl)
                {
                    if (c.SpecialityTitle == combbSpecialities.Text)
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

        private void FindAndSelect(List<Doctor> cList)
        {
            docsSearchColl = new ObservableCollection<Doctor>();
            foreach (Doctor c in cList)
            {
                docsSearchColl.Add(c);
            }
            dgrid.DataContext = docsSearchColl;
        }

        private void Reboot_Button_Click(object sender, RoutedEventArgs e)
        {
            if (docsSearchColl != null)
            {
                docsSearchColl.Clear();
                dgrid.DataContext = docsObsColl;
                tbxDocID.Text = tbxSecName.Text = String.Empty;
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Doctor d = new Doctor();
            DoctorEditWindow dialog = new DoctorEditWindow(d, docSpecsColl);
            dialog.Owner = owner;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dialog.ShowDialog() == false) return;
            context.Doctors.Add(d);
            context.SaveChanges();
            docsObsColl.Add(d);
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            Doctor d;
            if (dgrid.SelectedIndex < 0)
            {
                MessageBox.Show("Укажите работника для редактирования.");
                return;
            }
            else { d = (Doctor)dgrid.SelectedItem; }
            DoctorEditWindow dialog = new DoctorEditWindow(d, docSpecsColl);
            dialog.Owner = owner;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (dialog.ShowDialog() == false) return;
            context.SaveChanges();
            dgrid.ItemsSource = null;
            dgrid.ItemsSource = docsObsColl;
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex < 0)
            {
                MessageBox.Show("Для удаление необходимо указать клиента.");
                return;
            }

            List<Doctor> removeList = dgrid.SelectedItems.Cast<Doctor>().ToList();
            IQueryable<Ticket> ticketDB = from t in context.Tickets select t;
            foreach (Doctor d in removeList)
            {
                if (ticketDB.Where(t => (t.Reserved == true) && (t.DocTimeTable.DocID == d.DocID)).Any())
                {
                    MessageBox.Show("Нельзя удалить врача, у которого есть заказы в базе данных.");
                }
                else
                {
                    context.Doctors.Remove(d);
                    context.SaveChanges();
                    docsObsColl.Remove(d);
                }
            }
        }

        private void ChangeSpec_Button_Click(object sender, RoutedEventArgs e)
        {
            DocSpecialityEditWindow ew = new DocSpecialityEditWindow(docSpecsColl, context, docsObsColl);
            ew.Owner = owner;
            ew.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (ew.ShowDialog() == false)
            {
                dgrid.DataContext = null;
                dgrid.DataContext = docsObsColl;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "DoctorsList"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension
            if (dlg.ShowDialog() == false) return;

            try
            {
                //check for correct file name
                XDocument.Load(dlg.FileName);
            }
            catch { }

            IEnumerable<Doctor> cColl = context.Doctors;
            IEnumerable<DocTimeTable> cTT = context.DocTimeTables;
            IEnumerable<Ticket> cTick = context.Tickets;

            XDocument doc = new XDocument(
                new XElement("DoctorsInfo",
                    new XElement("Doctors",
                        from c in cColl
                        select c.DocPassportXml()),
                     new XElement("DocTimeTables",
                        from tt in cTT
                        select tt.DocTimeTablePassportXml()),
                    new XElement("Tickets",
                        from t in cTick
                        select t.TickPassportXml())));
            doc.Save(dlg.FileName);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "DoctorsList"; // Default file name
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

            List<Doctor> docColl = new List<Doctor>();
            List<DocTimeTable> ttColl = new List<DocTimeTable>();
            List<Ticket> tickColl = new List<Ticket>();

            //Find and create new Doctors, DocTimeTables and Tickets from XML-file
            for (Int32 i = 0; i < elementContainer.Count; i++)
            {
                Doctor cNew;
                DocTimeTable ttNew;
                Ticket tick;

                if (elementContainer[i].Name.ToString() == "Doctor")
                {
                    cNew = new Doctor
                    {
                        DocID = Convert.ToInt32(elementContainer[i + 1].Value.ToString()),
                        FirstName = elementContainer[i + 2].Value.ToString(),
                        SecondName = elementContainer[i + 3].Value.ToString(),
                        ThirdName = elementContainer[i + 4].Value.ToString(),
                        SpecialityID = Convert.ToInt32(elementContainer[i + 5].Value.ToString())
                    };
                    docColl.Add(cNew);
                }
                if (elementContainer[i].Name.ToString() == "DocTimeTable")
                {
                    ttNew = new DocTimeTable
                    {
                        DocTimeTableID = Convert.ToInt32(elementContainer[i + 1].Value),
                        WorkingShiftFirst = Convert.ToBoolean(elementContainer[i + 2].Value),
                        Room = Convert.ToInt32(elementContainer[i + 3].Value),
                        DocID = Convert.ToInt32(elementContainer[i + 4].Value)
                    };
                    ttColl.Add(ttNew);
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

            //Add Tickets to DocTimeTables
            for(Int32 i=0; i<ttColl.Count; i++)
            {
                foreach (Ticket tick in tickColl)
                {
                    if(tick.DocTimetableID == ttColl[i].DocTimeTableID)
                    {
                        ttColl[i].Tickets.Add(tick);
                    }
                }
            }

            //Add DocTimeTables to Doctors
            for (Int32 i = 0; i < docColl.Count; i++)
            {
                foreach (DocTimeTable tt in ttColl)
                {
                    if (tt.DocID == docColl[i].DocID)
                    {
                        docColl[i].DocTimeTables.Add(tt);
                    }
                }
            }

            //Add Doctors to the Context
            foreach(Doctor doc in docColl)
            {
                if (context.Doctors.Where(d=> ( d.DocID == doc.DocID ||
                               (d.FirstName == doc.FirstName &
                               d.SecondName == doc.SecondName &
                               d.ThirdName == doc.ThirdName &
                               d.SpecialityID == doc.SpecialityID))).Any<Doctor>() == false)
                {
                    context.Doctors.Add(doc);
                    context.SaveChanges();
                    docsObsColl.Add(doc);
                }
            }
        }
    }
}