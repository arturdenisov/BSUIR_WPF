using System;
using System.Windows;
using System.Xaml;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;


namespace Lab2_2
{
    class InterfaceCalculator : Window
    {
        BrainCalculator Brain;
        Grid gridWindow;
        TextBox Data;
        RadioButton rGradus, rRadian, rGrad;
        GroupBox GroupUnits;
        StackPanel Trigonometry;
        Button b0, b1, b2, b3, b4, b5, b6, b7, b8, b9,
        bPlus, bMultiplic, bDivis, bDifferen,
        bPercent, bProportion, bComma, bResult,
        bSign, bSqrt,
        bBack, bCFullDelete, bCELastDelete,
        bSin, bCos, bTg, bCtg, bAsin, bAcos, bAtg, bActg;

        public InterfaceCalculator()
        {
            Brain = new BrainCalculator();
            SetProperties();
            SetElements();
            CalcEventHandlers();
        }

        //свойства окна
        private void SetProperties()
        {
            this.Title = "Денисов А.Ю. гр.8032: Калькулятор";
            Uri iconUri = new Uri("pack://application:,,,/CXZ.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            this.MaxHeight = this.MinHeight = this.Height = 500;
            this.MaxWidth = this.MinWidth = this.Width = 400;
            this.Background = System.Windows.Media.Brushes.AliceBlue;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void SetElements()
        {
            MyGrid();
            this.Content = gridWindow;
            MyDataTextbox();
            MyRadioButton();
            MyTrigonometry();
            MyDeleteButton();
            MyNumeral();
            MyBinarOperation();
            MyUnarOperation();
        }

        private void MyGrid()
        {
            gridWindow = new Grid();
            gridWindow.MaxHeight = gridWindow.MinHeight = gridWindow.Height = 500;
            gridWindow.MaxWidth = gridWindow.MinWidth = gridWindow.Width = 400;
            //grid's rows and columns
            RowDefinition row0 = new RowDefinition();
            RowDefinition row1 = new RowDefinition();
            RowDefinition row2 = new RowDefinition();
            RowDefinition row3 = new RowDefinition();
            RowDefinition row4 = new RowDefinition();
            RowDefinition row5 = new RowDefinition();
            RowDefinition row6 = new RowDefinition();
            RowDefinition row7 = new RowDefinition();
            RowDefinition row8 = new RowDefinition();
            RowDefinition row9 = new RowDefinition();
            ColumnDefinition column0 = new ColumnDefinition();
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            ColumnDefinition column3 = new ColumnDefinition();
            ColumnDefinition column4 = new ColumnDefinition();
            ColumnDefinition column5 = new ColumnDefinition();
            ColumnDefinition column6 = new ColumnDefinition();
            ColumnDefinition column7 = new ColumnDefinition();
            ColumnDefinition column8 = new ColumnDefinition();
            ColumnDefinition column9 = new ColumnDefinition();
            ColumnDefinition column10 = new ColumnDefinition();
            ColumnDefinition column11 = new ColumnDefinition();
            gridWindow.RowDefinitions.Add(row0);
            gridWindow.RowDefinitions.Add(row1);
            gridWindow.RowDefinitions.Add(row2);
            gridWindow.RowDefinitions.Add(row3);
            gridWindow.RowDefinitions.Add(row4);
            gridWindow.RowDefinitions.Add(row5);
            gridWindow.RowDefinitions.Add(row6);
            gridWindow.RowDefinitions.Add(row7);
            gridWindow.RowDefinitions.Add(row8);
            gridWindow.RowDefinitions.Add(row9);
            gridWindow.ColumnDefinitions.Add(column0);
            gridWindow.ColumnDefinitions.Add(column1);
            gridWindow.ColumnDefinitions.Add(column2);
            gridWindow.ColumnDefinitions.Add(column3);
            gridWindow.ColumnDefinitions.Add(column4);
            gridWindow.ColumnDefinitions.Add(column5);
            gridWindow.ColumnDefinitions.Add(column6);
            gridWindow.ColumnDefinitions.Add(column7);
            gridWindow.ColumnDefinitions.Add(column8);
            gridWindow.ColumnDefinitions.Add(column9);
            gridWindow.ColumnDefinitions.Add(column10);
            gridWindow.ColumnDefinitions.Add(column11);
            gridWindow.VerticalAlignment = VerticalAlignment.Center;
            gridWindow.HorizontalAlignment = HorizontalAlignment.Center;

            // gridWindow.ShowGridLines = true;
        }

        private void RadiobuttonStyle(RadioButton rX)
        {
            rX.FontSize = 14;
            rX.FontWeight = FontWeights.Bold;
            rX.Padding = new Thickness(5, -3, 0, 0);
            rX.Margin = new Thickness(0, 10, 0, 0);
            rX.FontFamily = new FontFamily("Times New Roman");
        }

        private void MyButtonStyle(Button b)
        {
            b.FontSize = 25;
            b.FontWeight = FontWeights.Bold;
            b.FontFamily = new FontFamily("Times New Roman");
        }

        private Button CreateButton(string content, string name)
        {
            Button b = new Button();
            b.Content = content;
            b.Name = name;
            MyButtonStyle(b);
            b.Margin = new Thickness(5, 5, 5, 5);
            return b;
        }

        private Image CreateButtonImage(string uri)
        {
            BitmapImage btim = new BitmapImage();
            btim.BeginInit();
            Uri u = new Uri(uri);
            btim.UriSource = u;
            btim.EndInit();
            Image im = new Image();
            im.Source = btim;
            return im;
        }

        private void MyDataTextbox()
        {
            Data = new TextBox();
            Data.IsReadOnly = true;
            Data.Text = "0";
            Data.FontFamily = new FontFamily("Times New Roman");
            Data.BorderBrush = System.Windows.Media.Brushes.DarkGray;
            Data.BorderThickness = new Thickness(2, 2, 2, 2);
            Data.TextAlignment = TextAlignment.Right;
            Data.FontSize = 60;
            Data.FontWeight = FontWeights.Bold;
            Data.Padding = new Thickness(0, -3, 0, 0);
            Data.MaxLines = 1;

            Grid.SetRow(Data, 0);
            Grid.SetColumn(Data, 0);
            Grid.SetColumnSpan(Data, 12);
            Grid.SetRowSpan(Data, 2);
            gridWindow.Children.Add(Data);
            Data.Margin = new Thickness(25, 25, 25, 5);
        }

        private void MyRadioButton()
        {
            Trigonometry = new StackPanel();
            Trigonometry.Orientation = Orientation.Vertical;
            Trigonometry.VerticalAlignment = VerticalAlignment.Top;
            rGradus = new RadioButton();
            rGradus.Content = "градусы";
            RadiobuttonStyle(rGradus);
            rGradus.TabIndex = 24;
            Trigonometry.Children.Add(rGradus);

            rRadian = new RadioButton();
            rRadian.Content = "радианы";
            RadiobuttonStyle(rRadian);
            rRadian.TabIndex = 25;
            Trigonometry.Children.Add(rRadian);

            rGrad = new RadioButton();
            rGrad.Content = "грады";
            RadiobuttonStyle(rGrad);
            rGrad.TabIndex = 26;
            Trigonometry.Children.Add(rGrad);

            GroupUnits = new GroupBox();
            GroupUnits.FontFamily = new FontFamily("Times New Roman");
            GroupUnits.Header = "Тригонометрия";
            GroupUnits.Margin = new Thickness(8, 0, -5, 0);
            Grid.SetRow(GroupUnits, 2);
            Grid.SetColumn(GroupUnits, 0);
            Grid.SetRowSpan(GroupUnits, 2);
            Grid.SetColumnSpan(GroupUnits, 3);
            GroupUnits.Content = Trigonometry;
            gridWindow.Children.Add(GroupUnits);
        }

        private void MyTrigonometry()
        {
            bSin = CreateButton("sin", "bSin");
            bSin.TabIndex = 27;
            Grid.SetRow(bSin, 2);
            Grid.SetColumn(bSin, 3);
            Grid.SetColumnSpan(bSin, 2);
            gridWindow.Children.Add(bSin);

            bCos = CreateButton("cos", "bCos");
            bCos.TabIndex = 28;
            Grid.SetRow(bCos, 3);
            Grid.SetColumn(bCos, 3);
            Grid.SetColumnSpan(bCos, 2);
            gridWindow.Children.Add(bCos);

            bTg = CreateButton("tg", "bTg");
            bTg.TabIndex = 29;
            Grid.SetRow(bTg, 2);
            Grid.SetColumn(bTg, 5);
            Grid.SetColumnSpan(bTg, 2);
            gridWindow.Children.Add(bTg);

            bCtg = CreateButton("ctg", "bCTg");
            bCtg.TabIndex = 30;
            Grid.SetRow(bCtg, 3);
            Grid.SetColumn(bCtg, 5);
            Grid.SetColumnSpan(bCtg, 2);
            gridWindow.Children.Add(bCtg);

            bAsin = CreateButton("asin", "bAsin");
            bAsin.TabIndex = 31;
            Grid.SetRow(bAsin, 2);
            Grid.SetColumn(bAsin, 7);
            Grid.SetColumnSpan(bAsin, 2);
            gridWindow.Children.Add(bAsin);

            bAcos = CreateButton("acos", "bAcos");
            bAcos.TabIndex = 32;
            Grid.SetRow(bAcos, 3);
            Grid.SetColumn(bAcos, 7);
            Grid.SetColumnSpan(bAcos, 2);
            gridWindow.Children.Add(bAcos);

            bAtg = CreateButton("atg", "bAtg");
            bAtg.TabIndex = 33;
            Grid.SetRow(bAtg, 2);
            Grid.SetColumn(bAtg, 9);
            Grid.SetColumnSpan(bAtg, 2);
            gridWindow.Children.Add(bAtg);

            bActg = CreateButton("actg", "bActg");
            bActg.TabIndex = 34;
            Grid.SetRow(bActg, 3);
            Grid.SetColumn(bActg, 9);
            Grid.SetColumnSpan(bActg, 2);
            gridWindow.Children.Add(bActg);
        }

        private void MyNumeral()
        {
            b0 = CreateButton("_0", "b0");
            b0.FontSize = 35;
            b0.Padding = new Thickness(0, -2, 0, 0);
            b0.TabIndex = 10;
            Grid.SetRow(b0, 8);
            Grid.SetColumn(b0, 1);
            Grid.SetColumnSpan(b0, 4);
            gridWindow.Children.Add(b0);

            b1 = CreateButton("_1", "b1");
            b1.FontSize = 35;
            b1.Padding = new Thickness(0, -2, 0, 0);
            b1.TabIndex = 1;
            Grid.SetRow(b1, 5);
            Grid.SetColumn(b1, 1);
            Grid.SetColumnSpan(b1, 2);
            gridWindow.Children.Add(b1);

            b2 = CreateButton("_2", "b2");
            b2.FontSize = 35;
            b2.Padding = new Thickness(0, -2, 0, 0);
            b2.TabIndex = 2;
            Grid.SetRow(b2, 5);
            Grid.SetColumn(b2, 3);
            Grid.SetColumnSpan(b2, 2);
            gridWindow.Children.Add(b2);


            b3 = CreateButton("_3", "b3");
            b3.FontSize = 35;
            b3.Padding = new Thickness(0, -2, 0, 0);
            b3.TabIndex = 3;
            Grid.SetRow(b3, 5);
            Grid.SetColumn(b3, 5);
            Grid.SetColumnSpan(b3, 2);
            gridWindow.Children.Add(b3);

            b4 = CreateButton("_4", "b4");
            b4.FontSize = 35;
            b4.Padding = new Thickness(0, -2, 0, 0);
            b4.TabIndex = 4;
            Grid.SetRow(b4, 6);
            Grid.SetColumn(b4, 1);
            Grid.SetColumnSpan(b4, 2);
            gridWindow.Children.Add(b4);

            b5 = CreateButton("_5", "b5");
            b5.FontSize = 35;
            b5.Padding = new Thickness(0, -2, 0, 0);
            b5.TabIndex = 5;
            Grid.SetRow(b5, 6);
            Grid.SetColumn(b5, 3);
            Grid.SetColumnSpan(b5, 2);
            gridWindow.Children.Add(b5);

            b6 = CreateButton("_6", "b6");
            b6.FontSize = 35;
            b6.Padding = new Thickness(0, -2, 0, 0);
            b6.TabIndex = 6;
            Grid.SetRow(b6, 6);
            Grid.SetColumn(b6, 5);
            Grid.SetColumnSpan(b6, 2);
            gridWindow.Children.Add(b6);

            b7 = CreateButton("_7", "b7");
            b7.FontSize = 35;
            b7.Padding = new Thickness(0, -2, 0, 0);
            b7.TabIndex = 7;
            Grid.SetRow(b7, 7);
            Grid.SetColumn(b7, 1);
            Grid.SetColumnSpan(b7, 2);
            gridWindow.Children.Add(b7);


            b8 = CreateButton("_8", "b8");
            b8.FontSize = 35;
            b8.Padding = new Thickness(0, -2, 0, 0);
            b8.TabIndex = 8;
            Grid.SetRow(b8, 7);
            Grid.SetColumn(b8, 3);
            Grid.SetColumnSpan(b8, 2);
            gridWindow.Children.Add(b8);

            b9 = CreateButton("_9", "b9");
            b9.FontSize = 35;
            b9.Padding = new Thickness(0, -2, 0, 0);
            b9.TabIndex = 9;
            Grid.SetRow(b9, 7);
            Grid.SetColumn(b9, 5);
            Grid.SetColumnSpan(b9, 2);
            gridWindow.Children.Add(b9);

            bComma = CreateButton("_,", "bComma");
            bComma.FontSize = 40;
            bComma.Padding = new Thickness(0, -20, 0, 0);
            bComma.TabIndex = 11;
            Grid.SetRow(bComma, 8);
            Grid.SetColumn(bComma, 5);
            Grid.SetColumnSpan(bComma, 2);
            gridWindow.Children.Add(bComma);
        }

        private void MyDeleteButton()
        {
            bBack = new Button();
            bBack.Name = "bDelete";
            bBack.Content = "_Back";
            SolidColorBrush FontBrush = new SolidColorBrush(Colors.Black);
            FontBrush.Opacity = 0.5;
            bBack.Foreground = FontBrush;
            bBack.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/arrow.png")));
            bBack.TabIndex = 12;
            Grid.SetRow(bBack, 4);
            Grid.SetColumn(bBack, 1);
            Grid.SetColumnSpan(bBack, 2);
            bBack.Margin = new Thickness(5, 5, 5, 5);
            gridWindow.Children.Add(bBack);

            bCFullDelete = CreateButton("_C", "bTotalObnul");
            bCFullDelete.TabIndex = 13;
            Grid.SetRow(bCFullDelete, 4);
            Grid.SetColumn(bCFullDelete, 3);
            Grid.SetColumnSpan(bCFullDelete, 2);
            gridWindow.Children.Add(bCFullDelete);

            bCELastDelete = CreateButton("C_E", "bObnul");
            bCELastDelete.TabIndex = 14;
            Grid.SetRow(bCELastDelete, 4);
            Grid.SetColumn(bCELastDelete, 5);
            Grid.SetColumnSpan(bCELastDelete, 2);
            gridWindow.Children.Add(bCELastDelete);

        }

        private void MyBinarOperation()
        {
            bMultiplic = CreateButton("_*", "bUmn");
            bMultiplic.FontSize = 40;
            bMultiplic.Padding = new Thickness(0, 0, 0, 0);
            bMultiplic.TabIndex = 18;
            Grid.SetRow(bMultiplic, 5);
            Grid.SetColumn(bMultiplic, 7);
            Grid.SetColumnSpan(bMultiplic, 2);
            gridWindow.Children.Add(bMultiplic);

            bDivis = CreateButton("_/", "bDelit");
            bDivis.TabIndex = 19;
            Grid.SetRow(bDivis, 6);
            Grid.SetColumn(bDivis, 7);
            Grid.SetColumnSpan(bDivis, 2);
            gridWindow.Children.Add(bDivis);

            bPlus = CreateButton("_+", "bPlus");
            bPlus.TabIndex = 20;
            Grid.SetRow(bPlus, 7);
            Grid.SetColumn(bPlus, 7);
            Grid.SetColumnSpan(bPlus, 2);
            gridWindow.Children.Add(bPlus);

            bDifferen = CreateButton("_-", "bMinus");
            bDifferen.FontSize = 40;
            bDifferen.Padding = new Thickness(0, -15, 0, 0);
            bDifferen.TabIndex = 21;
            Grid.SetRow(bDifferen, 8);
            Grid.SetColumn(bDifferen, 7);
            Grid.SetColumnSpan(bDifferen, 2);
            gridWindow.Children.Add(bDifferen);
        }

        private void MyUnarOperation()
        {
            bSign = CreateButton("_Minus", "bPlusMinus");
            bSign.FontSize = 14;
            bSign.TabIndex = 15;
            Grid.SetRow(bSign, 4);
            Grid.SetColumn(bSign, 7);
            Grid.SetColumnSpan(bSign, 2);
            gridWindow.Children.Add(bSign);

            bSqrt = new Button();
            bSqrt.Name = "bKoren";
            bSqrt.Content = CreateButtonImage("pack://application:,,,/squareroot.png"); ;
            bSqrt.TabIndex = 16;
            Grid.SetRow(bSqrt, 4);
            Grid.SetColumn(bSqrt, 9);
            Grid.SetColumnSpan(bSqrt, 2);
            bSqrt.Margin = new Thickness(5, 5, 5, 5);
            gridWindow.Children.Add(bSqrt);

            bPercent = CreateButton("_%", "bProzent");
            bPercent.FontSize = 30;
            bPercent.TabIndex = 22;
            Grid.SetRow(bPercent, 5);
            Grid.SetColumn(bPercent, 9);
            Grid.SetColumnSpan(bPercent, 2);
            gridWindow.Children.Add(bPercent);

            bProportion = CreateButton("1/x", "bDrob");
            bProportion.TabIndex = 23;
            Grid.SetRow(bProportion, 6);
            Grid.SetColumn(bProportion, 9);
            Grid.SetColumnSpan(bProportion, 2);
            gridWindow.Children.Add(bProportion);

            bResult = CreateButton("=", "");
            bResult.IsDefault = true;
            bResult.FontSize = 40;
            bResult.TabIndex = 35;
            Grid.SetRow(bResult, 7);
            Grid.SetColumn(bResult, 9);
            Grid.SetRowSpan(bResult, 2);
            Grid.SetColumnSpan(bResult, 2);
            gridWindow.Children.Add(bResult);
        }

        //Обработчики событий
        private void CalcEventHandlers()
        {
            bCELastDelete.Click += Delete_Click;
            bCFullDelete.Click += Delete_Click;
            bBack.Click += Delete_Click;

            b0.Click += Numeral_Click;
            b1.Click += Numeral_Click;
            b2.Click += Numeral_Click;
            b3.Click += Numeral_Click;
            b4.Click += Numeral_Click;
            b5.Click += Numeral_Click;
            b6.Click += Numeral_Click;
            b7.Click += Numeral_Click;
            b8.Click += Numeral_Click;
            b9.Click += Numeral_Click;
            bComma.Click += Numeral_Click;

            bSign.Click += Unar_Click;
            bSqrt.Click += Unar_Click;
            bPercent.Click += Unar_Click;
            bProportion.Click += Unar_Click;

            bSin.Click += Trigonom_Click;
            bCos.Click += Trigonom_Click;
            bTg.Click += Trigonom_Click;
            bCtg.Click += Trigonom_Click;
            bAsin.Click += Trigonom_Click;
            bAcos.Click += Trigonom_Click;
            bAtg.Click += Trigonom_Click;
            bActg.Click += Trigonom_Click;

            bDifferen.Click += Binar_Click;
            bDivis.Click += Binar_Click;
            bMultiplic.Click += Binar_Click;
            bPlus.Click += Binar_Click;

            bResult.Click += Result_Click;

        }

        //флаг сохранения результата в Brain.Op1
        //без этого флага -  после получения результата начнется ввод в конец полученной цифры
        bool EndOperation = false;
        bool ResultOperation
        { get { return EndOperation; } set { EndOperation = value; } }

        //удаление ввода
        private void Delete_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            String str = Data.Text;
            switch (button.Name)
            {
                case "bDelete": //удаление последнего введенного элемента в поле текстбокса
                    {
                        if (str.Length < 2)
                        { Data.Text = string.Format("0"); break; }
                        else Data.Text = Data.Text.Substring(0, Data.Text.Length - 1); break;
                    }
                case "bObnul": //кнопка С
                    {
                        Data.Text = Convert.ToString("0");
                        break;
                    }
                case "bTotalObnul":  //кнопка СЕ
                    {
                        Data.Text = string.Format("0");
                        Brain.Op1 = double.NaN;
                        Brain.Op1 = double.NaN;
                        Brain.OpMark = ' ';
                        break;
                    }
            }
        }

        //ввод цифр
        private void Numeral_Click(object sender, EventArgs e)
        {
            try
            {

                //если после знака "равно" начался ввод числа, а не мат. операция
                //очищаем TextBox, снимаем флаг сохраненного значения
                //т.е. начинаем новую операцию расчетов
                if (ResultOperation && Brain.OpMark == ' ')
                {
                    ResultOperation = false; //обнуляем флаг
                    Data.Text = String.Empty;//обнуляем очищаем поле
                }

                Button button = (Button)sender;
                double x = (Data.Text == String.Empty) ? 0 : double.Parse(Data.Text);

                String y = Data.Text;

                //Обработка ситуаций некорректных значений         
                switch (y)
                {
                    //обнуление поля: если в textbox'e  - значение NaN
                    case "NaN": Data.Text = string.Format("0"); y = "0"; x = 0; break;
                    //обнуление поля: если в textbox'e  - значение "бесконечность"
                    case "бесконечность": Data.Text = string.Format("0"); y = "0"; x = 0; break;
                    //обнуление поля при попытке поставить несколько нулей подряд: если поле 0 нету запятой - замена содержимого текстбокса на 0
                    case "0": if (button.Name != "bComma") Data.Text = ""; break;
                }

                //    если ввод числа только начался - обнуляем поле, чтобы число с 0 не начиналось
                switch (button.Name) //добавление в текстбокс новой цифры - т. о. 0 по умолчанию стоящий в текстбоксе - заменяется
                {
                    case "b0": Data.Text += string.Format("0"); break;
                    case "b1": Data.Text += string.Format("1"); break;
                    case "b2": Data.Text += string.Format("2"); break;
                    case "b3": Data.Text += string.Format("3"); break;
                    case "b4": Data.Text += string.Format("4"); break;
                    case "b5": Data.Text += string.Format("5"); break;
                    case "b6": Data.Text += string.Format("6"); break;
                    case "b7": Data.Text += string.Format("7"); break;
                    case "b8": Data.Text += string.Format("8"); break;
                    case "b9": Data.Text += string.Format("9"); break;
                    case "bComma":
                        {
                            if (y.IndexOf(',') != -1) break; //если в строке уже есть запятая - повторный ввод запятой невозможен, выход из переключателя
                            if ((y.Length <= 1) && (Convert.ToDouble(Data.Text) != 0.0))//по умолчанию 0 удаляется - поэтому при вводе дроби <1  - перезаписываем строку 0,
                            { Data.Text += string.Format("0,"); break; }
                            else { Data.Text += string.Format(","); break; }
                        }
                }

            }

            catch { }

        }

        //бинарные операции
        private void Binar_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            //обработка ситуации, когда к результату вычисления применяется операция
            //т.е. нажали равно => в текстбоксе результат (в Result_Click он также сохраняется в Brain.Op1)=> 
            //этот результат  дальше в вычислениях (напр. / 2)
            if (ResultOperation && Brain.OpMark == ' ')
            {
                switch (button.Name)//сохраняем знак операции
                {

                    case "bUmn": Brain.OpMark = '*'; break;
                    case "bDelit": Brain.OpMark = '/'; break;
                    case "bPlus": Brain.OpMark = '+'; break;
                    case "bMinus": Brain.OpMark = '-'; break;
                }
                Data.Text = String.Empty;
                return;
            }

            if (ResultOperation)
            {
                //если в Op1 уже есть сохраненное значение
                Result_Click(Brain.OpMark, null);
            }
            else
            {
                //если в Op1 нету сохраненного значения
                Brain.Op1 = double.Parse(Data.Text);
                ResultOperation = true;
            }

            //сохраняем знак операции
            switch (button.Name)
            {

                case "bUmn": Brain.OpMark = '*'; break;
                case "bDelit": Brain.OpMark = '/'; break;
                case "bPlus": Brain.OpMark = '+'; break;
                case "bMinus": Brain.OpMark = '-'; break;
            }

            //Очистка текстбокса для ввода следующего значения
            Data.Text = String.Empty;
        }

        //унарные операции
        private void Unar_Click(object sender, EventArgs e)
        {
            if (FalseStart()) { return; }

            Button button = (Button)sender;
            switch (button.Name)
            {
                case "bDrob":
                    {
                        Data.Text = Brain.Fraction(Data.Text);
                        break;
                    }
                case "bProzent":
                    {
                        Data.Text = Brain.Percent(Data.Text);
                        break;
                    }
                case "bKoren":
                    {
                        Data.Text = Brain.SQRT(Data.Text);
                        break;
                    }
                case "bPlusMinus":
                    {
                        Data.Text = Brain.SignChange(Data.Text);
                        break;
                    }
            }
            ResultOperation = true;
        }

        //тригонометрические операции
        private void Trigonom_Click(object sender, EventArgs e)
        {
            if (FalseStart()) { return; } 

            try
            {
                bool ChechedRadBut = false;
                UIElementCollection Col = Trigonometry.Children;
                foreach (UIElement item in Col)
                {
                    if ((item as RadioButton).IsChecked == true)
                    { ChechedRadBut = true; }
                }
                if (ChechedRadBut == false)
                {
                    MessageBox.Show("Укажите единицу измерения!");
                    return;
                }

                Button button = (Button)sender;
                double x = double.Parse(Data.Text);
                if (rGradus.IsChecked == true)
                {
                    Data.Text = Brain.Trigonometry(x, (rGradus.Content as String), button.Name);
                }

                if (rRadian.IsChecked == true)
                {
                    Data.Text = Brain.Trigonometry(x, (rRadian.Content as String), button.Name);
                }

                if (rGrad.IsChecked == true)
                {
                    Data.Text = Brain.Trigonometry(x, (rGrad.Content as String), button.Name);
                }
                ResultOperation = true;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        //знак равно (вывод результата)
        private void Result_Click(object sender, EventArgs e)
        {
            if (FalseStart()) { return; }

            Brain.Op2 = double.Parse(Data.Text);
            Data.Text = Brain.GetResult();
            ResultOperation = true;
            
        }

        //Обработка ситуаций,когда после ввода знака сразу нажали "="
        //возврат к стартовым позициям
        private bool FalseStart()
        {
            if (Data.Text == String.Empty)
            {
                Brain.OpMark = ' ';
                ResultOperation = false;
                Data.Text = string.Format("0");
                return true;
            }
            else { return false; }
        }

    }
}
