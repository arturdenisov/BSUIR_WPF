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
    /// Логика взаимодействия для DocSpecialityEditWindow.xaml
    /// </summary>
    public partial class DocSpecialityEditWindow : Window
    {		
        DocSpeciality spec;
        ObservableCollection<DocSpeciality> docSpecsColl;
        ObservableCollection<Doctor> docObsColl;
        ProjectEntitiesContext context;

        public DocSpecialityEditWindow(ObservableCollection<DocSpeciality> _docSpecsColl, ProjectEntitiesContext _context, ObservableCollection<Doctor> _docObsColl)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            context = _context;
            spec = new DocSpeciality();
            InitializeComponent();
            docSpecsColl = _docSpecsColl;
            docObsColl = _docObsColl;
            dgrid.DataContext = docSpecsColl;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            spec.SpecialityTitle = tbxSpecTitle.Text;
            //spec.SpecialityID = 999;
            foreach (DocSpeciality dc in docSpecsColl)
            {
                if(dc.SpecialityTitle==spec.SpecialityTitle)
                {
                    MessageBox.Show("Данная специальность уже существует.");
                    DialogResult = true;
                    return;
                }
            }
            context.DocSpecialities.Add(spec);
            context.SaveChanges();
            docSpecsColl.Add(spec);
            dgrid.SelectedItem = 0;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex < 0) return;
            spec = (DocSpeciality)dgrid.SelectedItem;
                spec.SpecialityTitle = tbxSpecTitle.Text;
            context.SaveChanges();
            dgrid.DataContext = null;
            dgrid.DataContext = docSpecsColl;
            dgrid.SelectedItem = 0;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex < 0) return;
            List<DocSpeciality> listSpec = new List<DocSpeciality>();


            foreach (DocSpeciality elem in dgrid.SelectedItems)
            {
                listSpec.Add(elem);
            }
            foreach (DocSpeciality elemDel in listSpec)
            {
                if( docObsColl.Where(d=> d.SpecialityID ==  elemDel.SpecialityID).Any() == false)
                { 
                    context.DocSpecialities.Remove(elemDel);
                    docSpecsColl.Remove(elemDel);
                }
                else
                {
                    MessageBox.Show("Удаление невозможно, пока по данной специальности числятся специалисты.");
                }
            }
            context.SaveChanges();
            dgrid.DataContext = null;
            dgrid.DataContext = docSpecsColl;
            if(dgrid.SelectedIndex >= 0) dgrid.SelectedItem = 0;
        }
    }
}
