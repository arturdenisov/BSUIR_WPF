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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;

namespace DenisovArt_Kurs.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ChartsUserControl.xaml
    /// </summary>
    public partial class ChartsUserControl : UserControl
    {
        BarChart BarDiagr;
        PieChart PieDiagr;

        //подписи элементов диаграммы
        List<string> ListPositions;
        List<double> ListData;
        List<string> SpecialityTitleCol;

        //тип диаграммы
        bool? TypeDiagramIsBar = null;

        //для построения круговой диаграммы 
        Rectangle PieBackground;

        //для построения столбчатой диаграммы
        TextBlock TBMax;
        TextBlock TBMedian;
        Image ImBarDiagramFon;

        //количество столбцов в ряду
        int BarCount = 0;
        public int BarInRowCount
        { set { BarCount = value; } get { return BarCount; } }

        //список столбцов одного ряда, дизайн которых требуется изменить
        List<UIElement> ListBarsChanged;

        public ChartsUserControl(bool IsBarCharts)
        {
            InitializeComponent();
            object sender = null;
            RoutedEventArgs e = new RoutedEventArgs();
            ClearDiagram_Click(sender, e);
            TypeDiagramIsBar = IsBarCharts;
            CreateBackgroundDiagram();
            CreateListBoxServicesProfile();
            //в случае круговой диаграммы - отключаем возможность указывать период и врачей
            //диаграмма строится по данным текущего года для всех профилей врачей
            if (TypeDiagramIsBar == false)
            { spProf.IsEnabled = false; spPeriod.IsEnabled = false; }
        }

        //обработчик кнопки Очистить
        private void ClearDiagram_Click(object sender, RoutedEventArgs e)
        {
            if ((TypeDiagramIsBar == true) && (BarDiagr != null))
            {
                BarDiagr.StartingStateFabrik();
                TBMax.Text = "";
                TBMedian.Text = "";
                UCbarDiagGrid.Children.Remove(ImBarDiagramFon);
                UCbarDiagGrid.Children.Remove(TBMax);
                UCbarDiagGrid.Children.Remove(TBMedian);
                BarCount = 0;
                CreateBackgroundDiagram();
            }
            if ((TypeDiagramIsBar == false) && (PieDiagr != null))
            {
                PieDiagr.StartingStateFabrik();
                CreateBackgroundDiagram();
            }
        }

        //обработчик кнопки Изменить
        private void ChangeDesign_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem itemThickness = (ComboBoxItem)Cmb_Thickness.SelectedItem;
            Int32 lineThickness = Convert.ToInt32(itemThickness.Content.ToString());

            ComboBoxItem itemFill = (ComboBoxItem)Cmb_Fill.SelectedItem;
            SolidColorBrush _fillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(itemFill.Content.ToString()));

            ComboBoxItem itemStroke = (ComboBoxItem)Cmb_Stroke.SelectedItem;
            SolidColorBrush _strokeColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(itemStroke.Content.ToString()));

            if ((TypeDiagramIsBar == true) && (BarDiagr.RectChanged != null))
            {
                foreach (UIElement item in ListBarsChanged)
                {
                    (item as Rectangle).Fill = _fillColor;
                    (item as Rectangle).Stroke = _strokeColor;
                    (item as Rectangle).StrokeThickness = lineThickness;
                }
                //очищаем список столбцов изменяемого ряда
                ListBarsChanged.Clear();
                BarDiagr.RectChanged = null;

            }
            else
            {
                if ((TypeDiagramIsBar == false) && (PieDiagr.PathPieChanged != null))
                {
                    Path p = PieDiagr.PathPieChanged;
                    p.Fill = _fillColor;
                    p.Stroke = _strokeColor;
                    p.StrokeThickness = lineThickness;

                    //находим в коллекции грида стекпанель, в которой содержится легенда
                    StackPanel temp = new StackPanel();
                    foreach (UIElement item in UCbarDiagGrid.Children)
                    {
                        if ((item.Uid == "легенда") && (item is StackPanel))
                        {
                            temp = (item as StackPanel);
                        }
                    }

                    //получаем список прямоугольников 
                    List<Rectangle> list = new List<Rectangle>();
                    IEnumerable<Rectangle> z;
                    foreach (UIElement item in temp.Children)
                    {
                        if (item is StackPanel)
                        {
                            z = (item as StackPanel).Children.OfType<Rectangle>();
                            // MessageBox.Show(z.Count<Rectangle>().ToString());
                            foreach (Rectangle r in z)
                            { list.Add(r); }
                        }
                    }

                    //ищем нужный нам прямоугольник и перерисовываем его цвет
                    foreach (Rectangle rect in list)
                    {
                        if (rect.Uid == (p.DataContext as string))
                        {
                            rect.Fill = _fillColor;
                        }
                    }

                }

                else
                { MessageBox.Show("Не выделен элемент диаграммы, который следует изменить!"); }
            }
        }

        //обработчик кнопки Построить - вызов одной из 2 функций: BuildBarDiagram() и BuildPieDiagram()
        private void BuildDiagram_Click(object sender, RoutedEventArgs e)
        {
            //реакция приложения - если пользователь несколько раз нажал на кнопку Построить
            if (BarDiagr != null) { BarDiagr.StartingStateFabrik(); }
            if (PieDiagr != null) { PieDiagr.StartingStateFabrik(); }

            if (TypeDiagramIsBar == null)
            { MessageBox.Show("Не указан тип диаграммы! Нажмите соответствующую кнопку!"); }
            else
            {
                if (TypeDiagramIsBar == true)
                { BuildBarDiagram(); }
                else
                { BuildPieDiagram(); }
            }

        }

        //используется в обработчиках кнопкок Стобчатая диаграмма и Круговая диаграмма
        private void CreateBackgroundDiagram()
        {
            if (TypeDiagramIsBar == true)
            {
                ImBarDiagramFon = new Image();
                ImBarDiagramFon.Source = new BitmapImage(new Uri("/Images/Fon.jpg", UriKind.Relative));
                Grid.SetColumn(ImBarDiagramFon, 0);
                Grid.SetRow(ImBarDiagramFon, 0);
                Grid.SetColumnSpan(ImBarDiagramFon, 4);
                ImBarDiagramFon.Stretch = Stretch.Fill;
                UCbarDiagGrid.Children.Add(ImBarDiagramFon);


                StackPanel SPCaption = new StackPanel();
                SPCaption.Orientation = Orientation.Vertical;
                Grid.SetColumn(SPCaption, 0);
                UCbarDiagGrid.Children.Add(SPCaption);
                SPCaption.Margin = new Thickness(-150, 30, 0, 0);

                TBMax = new TextBlock();
                TBMax.Text = "";
                TBMax.FontSize = 20;
                TBMax.FontWeight = FontWeights.Bold;
                TBMax.TextAlignment = TextAlignment.Right;
                TBMax.Height = 30;
                TBMax.Width = 70;
                SPCaption.Children.Add(TBMax);

                TBMedian = new TextBlock();
                TBMedian.Text = "";
                TBMedian.FontSize = 20;
                TBMedian.FontWeight = FontWeights.Bold;
                TBMedian.TextAlignment = TextAlignment.Right;
                TBMedian.Height = 30;
                TBMedian.Width = 70;
                SPCaption.Children.Add(TBMedian);
                TBMedian.Margin = new Thickness(0, 120, 0, 0);

            }
            else
            {
                PieBackground = new Rectangle();
                PieBackground.Fill = Brushes.AliceBlue;
                Grid.SetColumn(PieBackground, 0);
                Grid.SetRow(PieBackground, 0);
                Grid.SetColumnSpan(PieBackground, 4);
                UCbarDiagGrid.Children.Add(PieBackground);
                PieBackground.Margin = new Thickness(0, -5, 0, 0);
                PieBackground.Stretch = Stretch.Fill;

            }
        }

        //заполняем листбокс из базы список профилей услуг
        private void CreateListBoxServicesProfile()
        {
            ProjectEntitiesContext context = new ProjectEntitiesContext();

            IQueryable<DocSpeciality> osDB = from os in context.DocSpecialities select os;
            List<DocSpeciality> osList = new List<DocSpeciality>();

            foreach (DocSpeciality o in osDB)
            {
                osList.Add(o);
            }

            var resultList = from o in osList select o;

            foreach (var item in resultList)
            {
                //MessageBox.Show(item.SpecialityTitle);
                ListBoxItem lbItem = new ListBoxItem();
                lbItem.Content = item.SpecialityTitle;
                lbItem.FontSize = 20;
                lbItem.FontFamily = new FontFamily("Times New Roman");
                lbItem.Selected += ListBoxItem_Selected;
                lbServisesProfile.Items.Add(lbItem);
            }
            if (TypeDiagramIsBar == true) lbServisesProfile.ToolTip = "При построении столбчатой диаграммы можно выбирать не более 4 профилей врачей!";
        }

        //формируем перечень параметров выбранных пользователем в 2 листбокса, используется в DateForBarCharts()
        private bool SelectByUser(ref List<int> Qwartal, ref List<string> SpecialityTitleCol)
        {
            int count = 0;
            //отбираем те параметры, что указал пользователь
            foreach (ListBoxItem x in lbServisesProfile.Items)
            {
                if (x.IsSelected == true)
                {
                    SpecialityTitleCol.Add(x.Content.ToString());
                    count++;
                }
            }

            foreach (UIElement y in spPeriod.Children)
            {
                if (y is CheckBox) //&&(y as CheckBox).IsChecked == true)
                {
                    if ((y as CheckBox).IsChecked == true)
                    {
                        if ((y as CheckBox).Uid == "12")
                        {
                            int[] qwartAll = { 1, 2, 3, 4 };
                            Qwartal.AddRange(new List<int>(qwartAll));
                        }
                        else
                        {
                            Qwartal.Add(Convert.ToInt32((y as CheckBox).Uid));
                        }
                    }
                }
            }


            if ((Qwartal.Count() == 0) && (TypeDiagramIsBar == true)) { MessageBox.Show("Не выбран период наблюдений"); return false; }

            if ((SpecialityTitleCol.Count() == 0) && (TypeDiagramIsBar == true)) { MessageBox.Show("Не выбраны профили врачей"); return false; }

            return true;

        }

        //формирование массива данных для диаграммы - используется в BuildBarDiagram()
        private int DateForBarCharts()
        {
            //Списки, используемые для передачи данных для построения диаграммы (объявлены как поля класса ChartsUserControl)
            ListData = new List<double>();
            ListPositions = new List<string>();
            SpecialityTitleCol = new List<string>();

            //Вспомогательныq списоск, в которых хранятся отобранные пользователем периоды и профили врачей
            List<int> Qwartal = new List<int>();

            //запрашиваем данные для построения диаграммы, если данных нет - завершаем построение диаграммы 
            if (SelectByUser(ref Qwartal, ref SpecialityTitleCol) == false) return -1;

            //построение столбчатой диаграммы
            int countZero = 0;
            foreach (string doc in SpecialityTitleCol)
            {
                double sum = 0;
                foreach (int qw in Qwartal)
                {
                    sum = QueryForBar(qw, doc);
                    if (sum == 0) { countZero++; }
                    ListData.Add(sum);
                    ListPositions.Add("Квартал №" + qw.ToString());
                }
            }
            //если в базе данных нет значений ни для одного столбца ни одного ряда - сообщение пользователю
            if (countZero == ListData.Count())
            {
                MessageBox.Show("В базе данных отсутствует информация по выбранным параметрам!");
                ListData.Clear();
                ListPositions.Clear();
                SpecialityTitleCol.Clear();
                return -1;
            }
            else
            {
                BarInRowCount = Qwartal.Count; //инициализируем свойство - количество столбцов в одном ряду
                return SpecialityTitleCol.Count; //возвращаем количество рядов диаграммы
            }
        }

        //вспомогательный метод для формирования массива данных
        //используется в DateForBarCharts для получения суммы продаж услуг за один квартал по конкретному профилю услуг
        private double QueryForBar(int qw, string Speciality)
        {
            ProjectEntitiesContext context = new ProjectEntitiesContext();
            IQueryable<OrderedService> osDB = from os in context.OrderedServices select os;
            List<OrderedService> osList = new List<OrderedService>();
            foreach (OrderedService o in osDB)
            {
                osList.Add(o);
            }
            double MySum = 0;
            Boolean canBuiltChart = false;

            for (int rMonth = qw * 3 - 2; rMonth <= qw * 3; rMonth++)
            {
                foreach (OrderedService v in osList)
                {
                    if (v.VisitList.VisitDate.Month == rMonth && v.Speciality == Speciality)
                    {
                        canBuiltChart = true;
                        break;
                    }
                }

                if (canBuiltChart != false)
                {
                    var resultList = from o in osList
                                     where (
                                     o.VisitList.VisitDate.Month == rMonth &&
                                     o.Speciality == Speciality)
                                     group o by o.Speciality into n
                                     select new
                                     {
                                         Name = n.Key,
                                         SUM = n.Sum<OrderedService>(q => q.Price)
                                     };


                    foreach (var item in resultList)
                    {
                        MySum += (double)item.SUM;
                    }

                }
                canBuiltChart = false;
            }
            return MySum;
        }

        //формирование массива данных для диаграммы - используется в BuildPieDiagram()
        private int DateForPieCharts()
        {
            //Списки, используемые для передачи данных для построения диаграммы (объявлены как поля класса ChartsUserControl)
            ListData = new List<double>();
            ListPositions = new List<string>();

            ProjectEntitiesContext context = new ProjectEntitiesContext();
            IQueryable<OrderedService> osDB = from os in context.OrderedServices select os;
            List<OrderedService> osList = new List<OrderedService>();
            foreach (OrderedService o in osDB)
            { osList.Add(o); }

            DateTime currentTime = DateTime.Now;

            var resultList = from o in osList
                             where o.VisitList.VisitDate.Year == currentTime.Year
                             group o by o.Speciality into n
                             select new { Name = n.Key, SUM = n.Sum<OrderedService>(q => q.Price) };

            foreach (var item in resultList)
            {

                ListData.Add((double)item.SUM);
                ListPositions.Add(item.Name.ToString());
            }
            //возвращаем 1 - так как все данные собираются за один период (текущий год) 
            return 1;

        }

        //используется в обработчике кнопки Построить 
        private void BuildBarDiagram()
        {
            int n = DateForBarCharts();
            if (n == -1) { MessageBox.Show("Не удалось построить диаграмму!"); return; }
            //Определяем максимальное значение из полученного числового диапазона всех рядов
            //необходимо для масштаба по оси Y
            double MaxVal = 0;
            for (int i = 0; i < BarInRowCount * n; i++)
            {
                if (ListData[i] > MaxVal) MaxVal = ListData[i];
            }

            //создаем фабрику элементов диаграммы и запускаем ее 
            BarDiagr = new BarChart(UCbarDiagGrid.Children, n, MaxVal);

            List<double> ListDataRow = new List<double>();

            int index = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < BarInRowCount; j++)
                {
                    ListDataRow.Add(ListData[index]);
                    index++;
                }
                BarDiagr.CreateBarDiagram(ListDataRow, ListPositions, SpecialityTitleCol[i], BarInRowCount, i);
                ListDataRow.Clear();
            }
            //подписи значений по оси Y
            TBMax.Text = (MaxVal).ToString("#.##");
            TBMedian.Text = (MaxVal / 2).ToString("#.##");
            BarDiagr.PropertyChanged += RectIsChanged_PropertyChanged;
        }

        //используется в обработчике кнопки Построить 
        private void BuildPieDiagram()
        {
            //получаем данные для построения - заполняем списки ListData и ListPositions
            DateForPieCharts();

            //построение круговой диаграммы
            PieDiagr = new PieChart(ListData, UCbarDiagGrid.Children);

            //строим круговую диаграмму
            PieDiagr.CreateDiagram(ListPositions);
            PieDiagr.PropertyChanged += PieIsChanged_PropertyChanged;

            //Очищаем списки так как они используются и BarDiagram
            ListData.Clear();
            ListPositions.Clear();


        }

        //обработчики событий изменения свойств объектов класса BarChart PiesPabrik
        //подписка - в методах BuildBarDiagram() и BuildPieDiagram() соотвественно
        private void RectIsChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Rectangle R = BarDiagr.RectChanged;
            int RectIndex = UCbarDiagGrid.Children.IndexOf(R);
            ListBarsChanged = new List<UIElement>();

            List<UIElement> ListUI = new List<UIElement>();
            foreach (UIElement elem in UCbarDiagGrid.Children)
            {
                if (elem.Uid == "столбец")
                {
                    ListUI.Add(elem);
                }
            }

            int count = ListUI.Count;
            int IndexRect = ListUI.IndexOf(R);
            int z = IndexRect / BarCount;
            int start = z * BarCount;
            int finish = start + BarCount;

            for (int i = start; i < finish; i++)
            {
                ListBarsChanged.Add(ListUI[i]);
            }

        }
        private void PieIsChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Path PathChanged = PieDiagr.PathPieChanged;
        }
        //обработчик текстбоксов "Периоды"
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            switch ((sender as CheckBox).Name)
            {
                case "cbYear":
                    {
                        if ((sender as CheckBox).IsChecked == true)
                        {
                            q1.IsEnabled = false; q1.IsChecked = false;
                            q2.IsEnabled = false; q2.IsChecked = false;
                            q3.IsEnabled = false; q3.IsChecked = false;
                            q4.IsEnabled = false; q4.IsChecked = false;
                        }
                        else
                        {
                            q1.IsEnabled = true;
                            q2.IsEnabled = true;
                            q3.IsEnabled = true;
                            q4.IsEnabled = true;
                        }
                        break;
                    }
                case "q1":
                case "q2":
                case "q3":
                case "q4":
                    {
                        if ((q1.IsChecked == true) || (q2.IsChecked == true) || (q3.IsChecked == true) || (q4.IsChecked == true))
                        { if (cbYear.IsEnabled == true) { cbYear.IsChecked = false; cbYear.IsEnabled = false; }; }
                        else
                        { cbYear.IsEnabled = true; }
                        break;
                    }
            }

        }

        //используется для контроля при построении столбчатой диаграммы - чтобы количество рядов было не больше 4
        //вызывается в методе CreateListBoxServicesProfile()
        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (TypeDiagramIsBar == true)
            {
                int count = 0;
                foreach (ListBoxItem x in lbServisesProfile.Items)
                {
                    if (x.IsSelected == true) { count++; }
                }

                if (count > 4)
                {
                    MessageBox.Show("Укажите не более 4 профилей врачей!");
                    int z = lbServisesProfile.SelectedIndex;
                    lbServisesProfile.SelectedItems.RemoveAt(z);
                }
            }
        }




    }

}

