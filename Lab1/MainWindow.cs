using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lab1
{
    class MainWindow : Window
    {
        private Grid grid;
        private Label LbX, LbY, LbZ;
        private TextBox TbX, TbY, TbZ, TbOut;
        private Button BtnCalculation;

        public MainWindow()
        {
            //Window properties
            this.Title = "Денисов А.Ю., лабораторная №1";
            this.Height = 550;
            this.Width = 500;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //Composition definition
            grid = new Grid();
            // grid.ShowGridLines = true;

            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.MinHeight = 400;
            grid.MaxHeight = 700;
            grid.MinWidth = 500;
            grid.MaxWidth = 500;

            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            RowDefinition rowDef5 = new RowDefinition();
            RowDefinition rowDef6 = new RowDefinition();
            RowDefinition rowDef7 = new RowDefinition();
            RowDefinition rowDef8 = new RowDefinition();
            RowDefinition rowDef9 = new RowDefinition();

            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);
            grid.RowDefinitions.Add(rowDef3);
            grid.RowDefinitions.Add(rowDef4);
            grid.RowDefinitions.Add(rowDef5);
            grid.RowDefinitions.Add(rowDef6);
            grid.RowDefinitions.Add(rowDef7);

            //строка для рисунка
            grid.RowDefinitions.Add(rowDef8);
            grid.RowDefinitions.Add(rowDef9);

            var converter = new GridLengthConverter();
            rowDef1.Height = (GridLength)converter.ConvertFromString("50");
            rowDef2.Height = (GridLength)converter.ConvertFromString("50");
            rowDef3.Height = (GridLength)converter.ConvertFromString("50");
            rowDef4.Height = (GridLength)converter.ConvertFromString("50");
            rowDef5.Height = (GridLength)converter.ConvertFromString("50");
            rowDef6.Height = (GridLength)converter.ConvertFromString("50");
            rowDef7.Height = (GridLength)converter.ConvertFromString("50");
            rowDef8.Height = (GridLength)converter.ConvertFromString("50");
            rowDef9.Height = (GridLength)converter.ConvertFromString("*");
            

            ColumnDefinition colDef1 = new ColumnDefinition();
            colDef1.MaxWidth = 200;
            colDef1.MinWidth = 150;
            ColumnDefinition colDef2 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);

            //CONTROLS
            //Labels
            LbX = new Label();
            LbX.Content = "X:";
            Grid.SetRow(LbX, 0);
            Grid.SetColumn(LbX, 0);

            LbY = new Label();
            LbY.Content = "Y:";
            Grid.SetRow(LbY, 2);
            Grid.SetColumn(LbY, 0);

            LbZ = new Label();
            LbZ.Content = "Z:";
            Grid.SetRow(LbZ, 4);
            Grid.SetColumn(LbZ, 0);

            LbX.VerticalAlignment = LbY.VerticalAlignment = LbZ.VerticalAlignment = VerticalAlignment.Bottom;
            LbX.Margin = LbY.Margin = LbZ.Margin = new Thickness(15, 5, 5, 5);

            grid.Children.Add(LbX);
            grid.Children.Add(LbY);
            grid.Children.Add(LbZ);

            //TextBox
            TbX = new TextBox();
            Grid.SetRow(TbX, 1);
            Grid.SetColumn(TbX, 0);
            Grid.SetZIndex(TbX, 0);

            TbY = new TextBox();
            Grid.SetRow(TbY, 3);
            Grid.SetColumn(TbY, 0);
            Grid.SetZIndex(TbY, 1);

            TbZ = new TextBox();
            Grid.SetRow(TbZ, 5);
            Grid.SetColumn(TbZ, 0);
            Grid.SetZIndex(TbZ, 2);
           
            TbX.TextWrapping = TbY.TextWrapping = TbZ.TextWrapping = TextWrapping.NoWrap;
            TbX.Margin = TbY.Margin = TbZ.Margin = new Thickness(15, 0, 5, 0);

            grid.Children.Add(TbX);
            grid.Children.Add(TbY);
            grid.Children.Add(TbZ);

            TbOut = new TextBox();
            TbOut.MinWidth = 150;
            TbOut.MaxWidth = 200;
            TbOut.MinHeight = 200;
            TbOut.MaxHeight = 250;
            TbOut.Margin = new Thickness(15, 0, 5, 50);
            TbOut.Text = "Output";
            TbOut.IsReadOnly = true;
            TbOut.TextWrapping = TextWrapping.Wrap;
            Grid.SetRowSpan(TbOut, 8);
            Grid.SetColumn(TbOut, 1);
            Grid.SetZIndex(TbOut, 4);

            grid.Children.Add(TbOut);

            //Buttons
            BtnCalculation = new Button();
            BtnCalculation.Content = "Calculation";
            BtnCalculation.Margin = new Thickness(25, 15, 25, 0);
            Grid.SetRow(BtnCalculation, 6);
            BtnCalculation.VerticalAlignment = VerticalAlignment.Top;
            BtnCalculation.IsDefault = true;
            Grid.SetRow(BtnCalculation, 6);

            BtnCalculation.Click += OnClick;

            grid.Children.Add(BtnCalculation);


            //Image
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,,/L1.png"));
            img.Height = 120;
            Grid.SetRow(img, 7);
            Grid.SetRowSpan(img, 2);
            Grid.SetColumnSpan(img, 2);
            grid.Children.Add(img);

            this.Content = grid;
            this.Show();

        }


        private void OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                double x = Convert.ToDouble(TbX.Text);
                double y = Convert.ToDouble(TbY.Text);
                double z = Convert.ToDouble(TbZ.Text);
                Calculator Calculator = new Calculator(x, y, z);
                TbOut.Text = Calculator.DoCalculation();
            }
            catch
            {
                MessageBox.Show("Input data again!");
            }
        }


    }
}
