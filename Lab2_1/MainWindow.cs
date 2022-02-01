using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lab2_1
{
    class MainWindow : Window
    {
        private Grid gridMain;
        private Button btnCalculations;
        private Label lbX, lbY;
        private TextBox tbX, tbY, tbResult;
        private RadioButton rdbSin, rdbCos, rdbTng;
        private CheckBox chkbMin, chkbMax;

        private GroupBox grpbx_SCT;
        private StackPanel stkpl_grpbxSCT;

        public MainWindow()

        {
            //свойства окна, контроллеров и подписка на события
            SetWindowProperties();
            SetMainContainerStructure();
            SetAdditionalContainerStructure();
        }


        private void SetWindowProperties()
        {
            //Title
            this.Title = "Denisov, SVPP L2 №1";
            //Height
            this.Height = 500;
            //Width
            this.Width = 450;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void SetMainContainerStructure()
        {

            //GRID_MAIN
            gridMain = new Grid();
            gridMain.Height = 450;
            gridMain.Width = 450;

            //grid's rows and columns
            RowDefinition row0 = new RowDefinition();
            RowDefinition row1 = new RowDefinition();
            RowDefinition row2 = new RowDefinition();
            RowDefinition row3 = new RowDefinition();
            RowDefinition row4 = new RowDefinition();
            RowDefinition row5 = new RowDefinition();

            var converter = new GridLengthConverter();
            row0.Height = (GridLength)converter.ConvertFromString("70");
            row1.Height = (GridLength)converter.ConvertFromString("70");
            row2.Height = (GridLength)converter.ConvertFromString("70");
            row3.Height = (GridLength)converter.ConvertFromString("70");
            row4.Height = (GridLength)converter.ConvertFromString("70");
            row5.Height = (GridLength)converter.ConvertFromString("*");

            ColumnDefinition column0 = new ColumnDefinition();
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            ColumnDefinition column3 = new ColumnDefinition();
            gridMain.RowDefinitions.Add(row0);
            gridMain.RowDefinitions.Add(row1);
            gridMain.RowDefinitions.Add(row2);
            gridMain.RowDefinitions.Add(row3);
            gridMain.RowDefinitions.Add(row4);
            gridMain.RowDefinitions.Add(row5);
            gridMain.ColumnDefinitions.Add(column0);
            gridMain.ColumnDefinitions.Add(column1);
            gridMain.ColumnDefinitions.Add(column2);
            gridMain.ColumnDefinitions.Add(column3);

            //gridMain.ShowGridLines = true;
            this.Content = gridMain;

            //CONTROLS

            //Image
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,,/Task1.png"));
            img.Height = 120;
            Grid.SetRow(img, 4);
            Grid.SetRowSpan(img, 2);
            Grid.SetColumnSpan(img, 4);
            gridMain.Children.Add(img);

            //ButtonCalculation
            btnCalculations = new Button();
            btnCalculations.Content = "Calculation";
            Grid.SetRow(btnCalculations, 2);
            Grid.SetColumn(btnCalculations, 0);
            Grid.SetColumnSpan(btnCalculations, 2);
            btnCalculations.Margin = new Thickness(55, 25, 55, 25);
            gridMain.Children.Add(btnCalculations);
            btnCalculations.Click += OnClick;


            //Labels

            //Labels (1) lbX
            lbX = new Label();
            lbX.Content = "X= ";
            Grid.SetRow(lbX, 0);
            Grid.SetColumn(lbX, 0);
            lbX.Margin = new Thickness(5, 25, 0, 0);
            gridMain.Children.Add(lbX);

            //Labels (2) lbY
            lbY = new Label();
            lbY.Content = "Y= ";
            Grid.SetRow(lbY, 1);
            Grid.SetColumn(lbY, 0);
            lbY.Margin = new Thickness(5, 25, 0, 0);
            gridMain.Children.Add(lbY);

            //TextBoxes
            //TextBox (1) tbX
            tbX = new TextBox();
            Grid.SetRow(tbX, 0);
            Grid.SetColumn(tbX, 0);
            Grid.SetColumnSpan(tbX, 2);
            tbX.Margin = new Thickness(55, 25, 55, 25);
            gridMain.Children.Add(tbX);

            //TextBox (2) tbY
            tbY = new TextBox();
            Grid.SetRow(tbY, 1);
            Grid.SetColumn(tbY, 0);
            Grid.SetColumnSpan(tbY, 2);
            tbY.Margin = new Thickness(55, 25, 55, 25);
            gridMain.Children.Add(tbY);


            //TextBox_result
            tbResult = new TextBox();
            tbResult.TextWrapping = TextWrapping.Wrap;
            tbResult.IsReadOnly = true;
            Grid.SetRow(tbResult, 2);
            Grid.SetRowSpan(tbResult, 2);
            Grid.SetColumn(tbResult, 2);
            Grid.SetColumnSpan(tbResult, 2);
            tbResult.Margin = new Thickness(5, 15, 30, 15);
            gridMain.Children.Add(tbResult);

            //CheckBoxes
            //CheckBox (1) chkbMin
            chkbMin = new CheckBox();
            chkbMin.Content = "min";
            Grid.SetRow(chkbMin, 0);
            Grid.SetColumn(chkbMin, 3);
            chkbMin.Margin = new Thickness(25, 15, 0, 0);
            gridMain.Children.Add(chkbMin);

            //CheckBox (2) chkbMax
            chkbMax = new CheckBox();
            chkbMax.IsChecked = true;
            chkbMax.Content = "max";
            Grid.SetRow(chkbMax, 0);
            Grid.SetColumn(chkbMax, 3);
            chkbMax.Margin = new Thickness(25, 45, 0, 0);
            gridMain.Children.Add(chkbMax);
        }

        private void SetAdditionalContainerStructure()
        {
            //STACK_PANEL
            stkpl_grpbxSCT = new StackPanel();

            //RadioButtons
            //RadioButton (1) rdbSin
            rdbSin = new RadioButton();
            rdbSin.IsChecked = true;
            rdbSin.Content = "sin(x)";
            stkpl_grpbxSCT.Children.Add(rdbSin);


            //RadioButton (2) rdbCos
            rdbCos = new RadioButton();
            rdbCos.Content = "cos(x)";
            stkpl_grpbxSCT.Children.Add(rdbCos);

            //RadioButton (3) rdbTng
            rdbTng = new RadioButton();
            rdbTng.Content = "tn(x)";
            stkpl_grpbxSCT.Children.Add(rdbTng);

            rdbSin.Margin = rdbCos.Margin = rdbTng.Margin = new Thickness(25, 15, 0, 0);

            //GROUP BOX
            grpbx_SCT = new GroupBox();
            grpbx_SCT.Header = "FN(X)";
            Grid.SetRow(grpbx_SCT, 0);
            Grid.SetRowSpan(grpbx_SCT, 2);
            Grid.SetColumn(grpbx_SCT, 2);
            grpbx_SCT.Content = stkpl_grpbxSCT;
            gridMain.Children.Add(grpbx_SCT);

        }

        //Обработчик событие считывает данные, делает расчеты и возвращает результат в приложение
        private void OnClick(object sender, RoutedEventArgs e)
        {
            //читает из окна числовые данные пользователя
            Double[] parameters = new Double[3];
            try
            {
                parameters[0] = Convert.ToDouble(tbX.Text);
                parameters[1] = Convert.ToDouble(tbY.Text);
            }
            catch
            {
                MessageBox.Show("Input data again!");
                return;
            }

            //читает из окна вид функции
            String function;
            if (rdbSin.IsChecked == true) function = "SIN";
            if (rdbCos.IsChecked == true) function = "COS";
            else function = "TNG";

            //обработка данных пользователя и вывод полученного результата в окно
            ReturnData(Calculator.DoCalculation(parameters, function));
        }

        //Возвращает полученный результат в окно согласно поставленной задаче
        private void ReturnData(double result)
        {
            tbResult.Text = "Lab2. \n" + Calculator.Branch;
            tbResult.Text += String.Format("\nResult = {0:0.000}", result);
            if (chkbMax.IsChecked == true)
            {
                tbResult.Text += String.Format("\nMax value = {0:0.000}", Calculator.MaxValue);
            }
            if (chkbMin.IsChecked == true)
            {
                tbResult.Text += String.Format("\nMin value = {0:0.000}", Calculator.MinValue);
            }
        }

    }
}






