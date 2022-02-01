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
    /// Логика взаимодействия для DoctorEditWindow.xaml
    /// </summary>
    public partial class DoctorEditWindow : Window
    {
        ProjectEntitiesContext context;
        Doctor d;
        DocTimeTable tt;
        ObservableCollection<Ticket> docTickets;
        ObservableCollection<DocSpeciality> docSpecsColl;

        public DoctorEditWindow(Doctor _d, ObservableCollection<DocSpeciality> _specColl)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            context = new ProjectEntitiesContext();
            docSpecsColl = _specColl;
            DoctorInitialization(_d);
            combbDocSpecialityt.ItemsSource = docSpecsColl;
            tbxFirName.DataContext = _d;
            tbxSecName.DataContext = _d;
            tbxThirdName.DataContext = _d;
            tbxRoom.DataContext = _d;
        }

        private void DoctorInitialization(Doctor _d)
        {
            d = _d;
            tt = new DocTimeTable();
            
            if (String.IsNullOrEmpty(d.SecondName) == false)
            {
                tbxFirName.Text = d.FirstName;
                tbxSecName.Text = d.SecondName;
                tbxThirdName.Text = d.ThirdName;
                tbxRoom.Text = d.MyRoom.ToString();
                dateGenerateTickets.SelectedDate = DateTime.Now;
                dateGetTickets.SelectedDate = DateTime.Now;
                combbDocSpecialityt.SelectedIndex = docSpecsColl.IndexOf(docSpecsColl.Where(s => s.SpecialityID == d.SpecialityID).First());
                lboxWorkShifting.SelectedIndex = Convert.ToInt32(d.WorkShiftFirst);
                
                return;
            }
            else
            {
                btnDelete.IsEnabled = false;
                btnGenerate.IsEnabled = false;
                dateGenerateTickets.IsEnabled = false;
                dateGetTickets.IsEnabled = false;
                combbDocTickets.IsEnabled = false;
            }
        }
       
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (String.IsNullOrEmpty(tbxFirName.Text) || String.IsNullOrEmpty(tbxSecName.Text))
                { MessageBox.Show("Все поля кроме генерации талонов обязательны для заполнения!"); return; }
                //Create a doctor
            d.FirstName = tbxFirName.Text;
            d.SecondName = tbxSecName.Text;
            d.ThirdName = tbxThirdName.Text;
            d.SpecialityID = ((DocSpeciality)combbDocSpecialityt.SelectedItem).SpecialityID;

            //Create a DocTimeTable
            tt.Room = Convert.ToInt32(tbxRoom.Text);
            tt.WorkingShiftFirst = Convert.ToBoolean(lboxWorkShifting.SelectedIndex);
            tt.DocID = d.DocID;

            //If we change DocTimeTable
            if (context.DocTimeTables.Where(tCont => tCont.DocID == tt.DocID).Any() == true)
            {
                foreach (DocTimeTable dtt in d.DocTimeTables)
                {
                    dtt.Room = tt.Room;
                    dtt.WorkingShiftFirst = tt.WorkingShiftFirst;
                }
                DialogResult = true;
                return;
            }

            //If we add new DocTimeTable for a new worker
            d.DocTimeTables.Add(tt);
            DialogResult = true;
            }
            catch
            { }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnGenerateTickets_Click(object sender, RoutedEventArgs e)
        {
            Ticket t = new Ticket();
            DateTime date = (DateTime)dateGenerateTickets.SelectedDate;
            Boolean workShifting = Convert.ToBoolean(lboxWorkShifting.SelectedIndex); //false - 1я смена (6-13), true - 2я смена (14-21)

            //Do we have these tickets also?
            IQueryable<Ticket> tickDB = from q in context.Tickets where q.DocTimeTable.DocID == d.DocID select q;
            foreach (Ticket tk in tickDB)
            {
                if ((tk.VisitDateTime == date.AddHours(6)) || (tk.VisitDateTime == date.AddHours(14)))
                {
                    MessageBox.Show("Для данного расписания (врач, дата, смена) уже сгенерированы талоны. Для новой генерации удалите предыдущиe.");
                    return;
                }
            }

            for (Int32 x = 0; x < 8; x++)
            {
                t.DocTimetableID = d.DocTimeTableID;
                t.Reserved = false;
                if (workShifting == false) t.VisitDateTime = date.AddHours(6 + x);
                else t.VisitDateTime = date.AddHours(14+x);
                context.Tickets.Add(t);
                context.SaveChanges();
            }
            dateGetTickets_SelectedDateChanged(null, null);
        }

        private void dateGetTickets_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            IQueryable<Ticket> ticDB =
                from t in context.Tickets
                where
                (t.DocTimetableID == d.DocTimeTableID &
                t.VisitDateTime.Year == dateGetTickets.SelectedDate.Value.Year &
                t.VisitDateTime.Month == dateGetTickets.SelectedDate.Value.Month &
                t.VisitDateTime.Day == dateGetTickets.SelectedDate.Value.Day)
                orderby t.VisitDateTime ascending
                select t;
            docTickets = new ObservableCollection<Ticket>();
            foreach (Ticket t in ticDB)
            {
                docTickets.Add(t);
            }
            combbDocTickets.ItemsSource = docTickets;
            combbDocTickets.SelectedIndex = 0;
        }

        private void btnDeleteTickets_Click(object sender, RoutedEventArgs e)
        {
            IQueryable<Ticket> ticDB =
                from t in context.Tickets
                where
                (t.DocTimetableID == d.DocTimeTableID &
                t.VisitDateTime.Year == dateGetTickets.SelectedDate.Value.Year &
                t.VisitDateTime.Month == dateGetTickets.SelectedDate.Value.Month &
                t.VisitDateTime.Day == dateGetTickets.SelectedDate.Value.Day)
                orderby t.VisitDateTime ascending
                select t;
            foreach (Ticket t in ticDB)
            {
                if(t.Reserved == false)
                { 
                    context.Tickets.Remove(t);
                }
            }
            context.SaveChanges();
            dateGetTickets_SelectedDateChanged(null, null);
        }
    }
}
