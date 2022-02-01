using System;
using System.Windows;
using Lab5;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Controls;


namespace Lab5
{
    /// <summary>
    /// Логика взаимодействия для ShapeEditor.xaml
    /// </summary>
    public partial class ShapeEditor : Window
    {
        public ShapeEditor(Int32 cmbThick, Int32 cmbFill, Int32 cmbStroke)
        {
            InitializeComponent();
            dialCmb_Thickness.SelectedIndex = cmbThick;
            dialCmb_Fill.SelectedIndex = cmbFill;
            dialCmb_Stroke.SelectedIndex = cmbStroke;
        }

        private void btnSave_click(object sender, RoutedEventArgs e)
        {
            (Owner as MainWindow).cmbThickness = dialCmb_Thickness.SelectedIndex;
            (Owner as MainWindow).cmbFillSelected = dialCmb_Fill.SelectedIndex;
            (Owner as MainWindow).cmbStrokeSelected = dialCmb_Stroke.SelectedIndex;
            Close();
        }
    }
}