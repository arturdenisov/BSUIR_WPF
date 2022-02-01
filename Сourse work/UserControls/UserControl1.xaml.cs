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

namespace WpfControlLibrary_BarDiagram
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        BarsFabrik BarsDiagramFabrik; 
        PiesFabrik PieDiagramFabrik;		

        //подписи элементов диаграммы
        List<string> ListPositions;
        List<double> ListData;

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

        public UserControl1()
        {
            InitializeComponent();          
        }

        //обработчик кнопки Стобчатая диаграмма - определяет TypeDiagramIsBar, использует метод CreateBackgroundDiagram
        private void BarDiagramCreate_Click(object sender, RoutedEventArgs e)
        {
            ClearDiagram_Click(sender, e);
            TypeDiagramIsBar = true;
            CreateBackgroundDiagram();
        }
        //обработчик кнопки Круговая диаграмма - определяет TypeDiagramIsBar, использует метод CreateBackgroundDiagram
        private void PieDiagramCreate_Click(object sender, RoutedEventArgs e)
        {
            ClearDiagram_Click(sender, e);
            TypeDiagramIsBar = false;
            CreateBackgroundDiagram();
        }
        //обработчик кнопки Очистить
        private void ClearDiagram_Click(object sender, RoutedEventArgs e)
        {  
            if ((TypeDiagramIsBar == true)&&(BarsDiagramFabrik !=null))
            {
                BarsDiagramFabrik.StartingStateFabrik();
                TBMax.Text = "";
                TBMedian.Text = "";
                UCbarDiagGrid.Children.Remove(ImBarDiagramFon);
                UCbarDiagGrid.Children.Remove(TBMax);
                UCbarDiagGrid.Children.Remove(TBMedian);
                BarCount = 0;
            }
            if ((TypeDiagramIsBar == false)&&(PieDiagramFabrik != null))
            {
                PieDiagramFabrik.StartingStateFabrik();
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

            if ((TypeDiagramIsBar == true) && (BarsDiagramFabrik.RectChanged != null))
            {
                foreach (UIElement item in ListBarsChanged)
                {
                    (item as Rectangle).Fill = _fillColor;
                    (item as Rectangle).Stroke = _strokeColor;
                    (item as Rectangle).StrokeThickness = lineThickness;
                }
                //очищаем список столбцов изменяемого ряда
                ListBarsChanged.Clear();
                BarsDiagramFabrik.RectChanged = null;

            }
            else
            {
                if ((TypeDiagramIsBar == false) && (PieDiagramFabrik.PathPieChanged != null))
                {
                    Path p = PieDiagramFabrik.PathPieChanged;
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
                            {  list.Add(r); }
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
            if (BarsDiagramFabrik != null) { BarsDiagramFabrik.StartingStateFabrik(); }
            if (PieDiagramFabrik != null) { PieDiagramFabrik.StartingStateFabrik(); }

            if (TypeDiagramIsBar == null)
            { MessageBox.Show("Не указан тип диаграммы! Нажмите соответствующую кнопку!"); }
            else
            {
                if (TypeDiagramIsBar == true)
                {  BuildBarDiagram(); }
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
                ImBarDiagramFon.Source = new BitmapImage(new Uri("FonBars.png", UriKind.Relative));
                Grid.SetColumn(ImBarDiagramFon, 0);
                Grid.SetRow(ImBarDiagramFon, 0);
                Grid.SetColumnSpan(ImBarDiagramFon, 3);
                ImBarDiagramFon.Stretch = Stretch.Fill;
                UCbarDiagGrid.Children.Add(ImBarDiagramFon);


                StackPanel SPCaption = new StackPanel();
                SPCaption.Orientation = Orientation.Vertical;
                Grid.SetColumn(SPCaption, 0);
                UCbarDiagGrid.Children.Add(SPCaption);
                SPCaption.Margin = new Thickness(-40, 50, 560, 70); 

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
                TBMedian.Margin = new Thickness(0, 110, 0, 0);

            }
            else
            {
                PieBackground = new Rectangle();
                PieBackground.Fill = Brushes.AliceBlue;
                Grid.SetColumn(PieBackground, 0);
                Grid.SetRow(PieBackground, 0);
                Grid.SetColumnSpan(PieBackground, 3);
                UCbarDiagGrid.Children.Add(PieBackground);
                PieBackground.Stretch = Stretch.Fill;

            }
        }

        //формирование массива данных для диаграммы - используется в BuildBarDiagram() и BuildPieDiagram()
        private int DateForCharts()
        {
            ListData = new List<double>();
            ListPositions = new List<string>();

            if (TypeDiagramIsBar == true)
            {

                int n = 2;

                //Заглушка
                double[] Mas = { 5.11111, 10.3, 20.9, 15.1, 7.9, 12.4, 34.8, 24.3, 29.1, 13.8, 8.9, 9.1, 10.12, 24.14, 17.456, 19.12, 13.21, 2.09, 9.6, 20.9,
                4.8, 10.3, 12.9, 14.1, 6.9, 10.4, 34.8, 20.3, 29.1, 18.8, 8.9, 9.1, 16.12, 22.14, 16.456, 14.12, 19.21, 20.09, 11.6, 14.9};
                string[] StrMas = {"январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август",
                "сентябрь", "октябрь", "ноябрь", "декабрь", "Новый год", "Рождество", "Пасха", "Троица", "Ханука", "Хэллоуин", "Первомай", "Святки" };
                BarInRowCount = 6;
                int count = StrMas.Length;

                //проверки на возможность отображения
                if ((BarInRowCount > 20) || (n > 5)) { MessageBox.Show("Рядов должно быть не более 5, а наблюдений - не более 20!"); return -1; }
                if ((n > 3) && (n > 6)) { MessageBox.Show("Если рядов более 3, наблюдений должно быть не более 6!"); return -1; }
                if ((n < 3) && (n > 10)) { MessageBox.Show("Если рядов менее 3, наблюдений должно быть не более 10!"); return -1; }

                for (int i = 0; i < count; i++)
                { ListPositions.Add(StrMas[i]); }

                for (int i = 0; i < BarInRowCount * n; i++)
                { ListData.Add(Mas[i]); }

                return n;
            }

            if (TypeDiagramIsBar == false)
            {
                //заглушка
                double x1 = 120;
                ListData.Add(x1);
                double x2 = 60;
                ListData.Add(x2);
                double x3 = 200;
                ListData.Add(x3);
                double x4 = 30;
                ListData.Add(x4);

                ListPositions.Add("научная");
                ListPositions.Add("учебная");
                ListPositions.Add("художественная");
                ListPositions.Add("детская");

                return 1;
            }

            return -1;

        }

        //используется в обработчике кнопки Построить 
        private void BuildBarDiagram()
        {
            int n = DateForCharts();
            //Определяем максимальное значение из полученного числового диапазона всех рядов
            //необходимо для масштаба по оси Y
            double MaxVal = 0;
            for (int i = 0; i < BarInRowCount*n; i++)
            {
                if (ListData[i] > MaxVal) MaxVal = ListData[i];
            }

            //создаем фабрику элементов диаграммы и запускаем ее 
            BarsDiagramFabrik = new BarsFabrik(UCbarDiagGrid.Children, n, MaxVal);

            List<double> ListDataRow = new List<double>();

            int index = 0;
            for (int i = 0; i < n; i++)
            {
               // ListDataRow = new List<double>();
                for (int j = 0; j< BarInRowCount; j++ )
                {                  
                    ListDataRow.Add(ListData[index]);
                    index++;                   
                }
                BarsDiagramFabrik.CreateBarDiagram(ListDataRow, ListPositions, BarInRowCount, i);
                ListDataRow.Clear();
            }
            //подписи значений по оси Y
            TBMax.Text = (MaxVal).ToString("#.##");
            TBMedian.Text = (MaxVal / 2).ToString("#.##");
            BarsDiagramFabrik.PropertyChanged += RectIsChanged_PropertyChanged;   
        }

        //используется в обработчике кнопки Построить 
        private void BuildPieDiagram()
        {
            //получаем данные для построения - заполняем списки ListData и ListPositions
            DateForCharts();

            //построение круговой диаграммы
            PieDiagramFabrik = new PiesFabrik(ListData, UCbarDiagGrid.Children);

            //строим круговую диаграмму
            string NameDiagram = "Продажи книг по жанрам";
            PieDiagramFabrik.CreateDiagram(ListPositions, NameDiagram);
            PieDiagramFabrik.PropertyChanged += PieIsChanged_PropertyChanged;

            //Очищаем списки так как они используются и BarDiagram
            ListData.Clear();
            ListPositions.Clear();

        }

        //обработчики событий изменения свойств объектов класса Barsfabrik PiesPabrik
        //подписка - в методах BuildBarDiagram() и BuildPieDiagram() соотвественно
        private void RectIsChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Rectangle R = BarsDiagramFabrik.RectChanged;
            int RectIndex = UCbarDiagGrid.Children.IndexOf(R);
            ListBarsChanged = new List<UIElement>();

            List<UIElement> ListUI = new List<UIElement>();
            foreach (UIElement elem in UCbarDiagGrid.Children)
            {
                if(elem.Uid == "столбец")
                {
                    ListUI.Add(elem);  
                }
            }

            int count = ListUI.Count;
            int IndexRect = ListUI.IndexOf(R);
            int z = IndexRect / BarCount;
            int start = z * BarCount;
            int finish = start + BarCount;

            for (int i=start; i<finish; i++)
            {
                ListBarsChanged.Add(ListUI[i]);
            }

        }
        private void PieIsChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Path PathChanged = PieDiagramFabrik.PathPieChanged;
        }
    } 

}
    

