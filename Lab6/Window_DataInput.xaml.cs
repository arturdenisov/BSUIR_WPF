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

namespace Lab6
{
    /// <summary>
    /// Логика взаимодействия для Window_DataInput.xaml
    /// </summary>
    public partial class Window_DataInput : Window
    {

        public Window_DataInput()
        {
            InitializeComponent();
        }

        private void InputDataSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckCorrectInputedData() == false) return;
            ((MainWindow)Owner).integralCalcTask = new Calculation(
                    Convert.ToDouble(Txb_StartInt.Text),
                    Convert.ToDouble(Txb_StartEnd.Text),
                    Convert.ToInt32(Txb_NumInt.Text),
                    ((MainWindow)Owner).backgroundWorker);
            Close();
        }

        private Boolean CheckCorrectInputedData()
        {
            try
            {
                    Convert.ToDouble(Txb_StartInt.Text);
                    Convert.ToDouble(Txb_StartEnd.Text);
                    Convert.ToInt32(Txb_NumInt.Text);
                return true;
            }
            catch
            {
                MessageBox.Show("***DATA INPUT ERROR*** \n Reinput data and be carefull.");
                return false;
            }
        }
    }
}
