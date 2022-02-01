using System.Windows;
using DenisovArt_Kurs.UserControls;
using DenisovArt_Kurs.DialogWindows;
using System.Windows.Controls;


namespace DenisovArt_Kurs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientsUserControl clientUC;
        VisitListsUserControl visitUC;
        DoctorsUserControl doctorUC;
        ServicesUserControl servUC;
        ChartsUserControl chartsUC;

        public MainWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            
        }

        private void MenuRegister_Click(object sender, RoutedEventArgs e)
        {
            CleanUserControls();
        }

        private void MenuBarCharts_Click(object sender, RoutedEventArgs e)
        {
            CleanUserControls();
            chartsUC = new ChartsUserControl(true);
            Grid.SetRow(chartsUC, 1);
            mainGrid.Children.Add(chartsUC);
        }

        private void MenuPieCharts_Click(object sender, RoutedEventArgs e)
        {
            CleanUserControls();
            chartsUC = new ChartsUserControl(false);
            Grid.SetRow(chartsUC, 1);
            mainGrid.Children.Add(chartsUC);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClientUserControl_Click(object sender, RoutedEventArgs e)
        {
            clientUC = new ClientsUserControl(this);
            Grid.SetRow(clientUC,1);
            mainGrid.Children.Add(clientUC);
        }

        private void CleanUserControls()
        {
            if (clientUC != null) mainGrid.Children.Remove(clientUC);
            if (visitUC != null) mainGrid.Children.Remove(visitUC);
            if (doctorUC != null) mainGrid.Children.Remove(doctorUC);
            if (servUC != null) mainGrid.Children.Remove(servUC);
            if (chartsUC != null) mainGrid.Children.Remove(chartsUC);
        }

        private void VisitListsControl_Click (object sender, RoutedEventArgs e)
        {
            visitUC = new VisitListsUserControl(this);
            Grid.SetRow(visitUC, 1);
            mainGrid.Children.Add(visitUC);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutW = new AboutWindow();
            aboutW.Owner = this;
            aboutW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            aboutW.ShowDialog();
        }

        private void DoctosControl_Click(object sender, RoutedEventArgs e)
        {
            doctorUC = new DoctorsUserControl();
            Grid.SetRow(doctorUC, 1);
            mainGrid.Children.Add(doctorUC);
        }

        private void ServicesControl_Click(object sender, RoutedEventArgs e)
        {
            servUC = new ServicesUserControl();
            Grid.SetRow(servUC, 1);
            mainGrid.Children.Add(servUC);
        }
    }
}
