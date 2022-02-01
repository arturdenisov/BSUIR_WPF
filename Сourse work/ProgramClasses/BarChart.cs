using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.ComponentModel;

namespace DenisovArt_Kurs
{
    //создает и добавляет в коллекцию грида столбцы, подписи и данные
    class BarChart
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(prop));
            }
        }

        Rectangle BarChanged;
        public Rectangle RectChanged
        {
            get
            {
                return BarChanged;
            }
            set
            {
                BarChanged = value;
                OnPropertyChanged("RectChanged");
            }
        }

        //для создания столбца с подписями
        Bar MyBar;
        Rectangle Rect;
        TextBlock TB;
        //для изменения интервала между столбцами и подписями
        int LeftIntendantion = 200;

        //для изменения ширины столбца
        int RectBarWidth = 50;

        //для изменения наклона подписей столбцов
        int Orientation = 0;

        //для работы с многорядной диаграммой
        bool MultiColumnDiagram = false;
        public int BarsCount
        { get; set; }

        //для ссылки на коллекцию элементов грида UC
        public UIElementCollection Col
        { get; set; }

        //максимальное значение из всех диапазонов всех рядов
        double MaxBar = 0;

        //цвета столбцов по умолчанию
        string[] BarsColors = { "Red", "Blue", "Green", "Yellow", "Violet", "Orange", "AliceBlue" };

        public BarChart(UIElementCollection A, int count, double Max)
        {
            Col = A;
            BarsCount = count;
            if (BarsCount > 1) Bar.LeftDataIndent -= 20 * BarsCount;
            MaxBar = Max;
        }

        //создание и отображение диаграммы
        public void CreateBarDiagram(List<double> ListData, List<string> StrMas, string SpecialityTitleCol, int n, int NumberColor)
        {
            //настройка отображения столбцов по осям диаграммы (масштабирование высоты и расстояния столбцов и подписей)
            AlinementAxisX(ListData, n);

            //создание элементов диаграммы: 
            //создание и связь объекта класса Bar (хранит координаты одного столбца и 2 его подписей)
            // с текстбоксами и прямоугольником
            CreateItemDiagram(ListData, StrMas, SpecialityTitleCol, n, NumberColor);

            //отображаем созданные фабрикой элементы по оси X
            VisualisationOfItemDiagram();
        }

        //перевод фабрики в стартовое состояние
        public void StartingStateFabrik()
        {
            Bar.BarLeftIndent = -500;
            Bar.LeftDataIndent = 100;
            LeftIntendantion = 200;
            RectBarWidth = 50;
            Orientation = 0;
            MultiColumnDiagram = false;
            BarsCount = 1;
            MaxBar = 0;
            UIElement item;
            for (int i = 0; i < Col.Count; i++)
            {
                item = Col[i];
                if ((item.Uid == "столбец") || (item.Uid == "подпись") || (item.Uid == "данные"))
                { Col.Remove(item); i--; }
            }
        }

        //настройка осей диаграммы - 
        //определяются интервалы между столбцами и группами столбцов, их подписями
        private void AlinementAxisX(List<double> ListData, int n)
        {
            //Настраиваем работу с осью X
            //MultiColumnDiagram переводится в true после построения первого ряда диаграммы
            //данная проверка смещает каждый последующий ряд диаграммы  вправо
            if (MultiColumnDiagram == true)
            {
                Bar.BarLeftIndent += 50;
                if (BarsCount < 4) Bar.LeftDataIndent += 10 * BarsCount;
                else { Bar.LeftDataIndent += 5 * (6 - BarsCount) + 15; }
            }

            //определяем каким интервалом между объектами, шириной столбца и наклоном подписи надо пользоваться
            //вызывается однократно - т.к. для последующих столбцов будут использоваться показатели, заданные 1ым столбцом
            if (MultiColumnDiagram == false)
            {
                CreateIntervalAndOrientation(n);
            }
        }

        //создание объектов диаграммы
        private void CreateItemDiagram(List<double> ListData, List<string> StrMas, string SpecialityTitleCol, int n, int NumberColor)
        {
            //создаем и добавляем объекты в коллекцию грида
            double y = 0; //для задания пропорции между столбцом и максимальным по размеру столбцом
            //создание объектов диаграммы
            for (int i = 0; i < n; i++)
            {
                y = (ListData[i] / MaxBar);
                MyBar = new Bar(LeftIntendantion * i, y);
                //создаем столбец диаграммы
                Rect = CreateRect(SpecialityTitleCol, NumberColor);
                Rect.DataContext = MyBar;
                Rect.Margin = MyBar.BarMargin;
                if (y == 0) { MyBar.MyBarHeight = 2; }
                Rect.Height = MyBar.MyBarHeight;
                Col.Add(Rect);


                //создаем подпись столбца диаграммы однократно -  если у нас несколько рядов!
                if (MultiColumnDiagram == false)
                {

                    TB = CreateTBLabel(StrMas[i]);
                    Col.Add(TB);
                }

                //создаем подпись данных столбца
                TB = CreateTBData((y == 0) ? "0" : ListData[i].ToString("#.##"));
                Col.Add(TB);
            }

            //устанавливаем свойство, чтобы следующие ряды диаграммы
            //не меняли заданных в ChartsAxis начальных интервалов и отступов между столбцами и подписями
            //а также не выводились повторно подписи столбцов
            if (BarsCount > 1) { MultiColumnDiagram = true; }
        }

        //отображение созданных объектов диаграммы
        private void VisualisationOfItemDiagram()
        {
            foreach (UIElement item in Col)
            {
                switch (item.Uid)
                {
                    case "столбец":
                        {
                            MyBar = ((item as Rectangle).DataContext as Bar);
                            (item as Rectangle).Margin = MyBar.BarMargin;
                            (item as Rectangle).PreviewMouseLeftButtonDown += Rectangle_PreviewMouseLeftButtonDown;
                            break;
                        }
                    case "подпись":
                        {
                            (item as TextBlock).Margin = MyBar.TBMargin;
                            break;
                        }
                    case "данные":
                        {
                            (item as TextBlock).Margin = MyBar.TBDataMargin;
                            Grid.SetZIndex((item as TextBlock), 1); //подписи данных - поверх всех элементов
                            break;
                        }
                }
            }
        }

        //обработчик клика мышкой по столбцу
        private void Rectangle_PreviewMouseLeftButtonDown(object sender, EventArgs e)
        {
            RectChanged = (Rectangle)sender;
        }

        //вспомогательные методы        
        //используется в ChartsAxis
        private void CreateIntervalAndOrientation(int n)
        {
            //поправки к пропорциям при многостолбчатой диаграмме
            if (BarsCount != 1)
            {
                if (n < 7)
                {
                    RectBarWidth -= 25 + 10 * (6 - n);
                    Bar.BarLeftIndent -= 20 * BarsCount;
                    LeftIntendantion -= (n >= 5) ? (3 * (7 - n) - 10 * BarsCount) : (10 * BarsCount);
                    int x = (BarsCount == 2) ? 3 : BarsCount;
                    Bar.LeftDataIndent += 16 * (x) - 5 * n;

                    if ((BarsCount > 3))
                    {
                        Bar.BarLeftIndent += 10;
                        LeftIntendantion -= 10;
                    }
                }

                else
                {
                    RectBarWidth -= (n > 10) ? 20 : 25;
                    Bar.BarLeftIndent -= 30 * BarsCount;
                    LeftIntendantion += 6 * BarsCount;
                    Bar.LeftDataIndent += 10 * BarsCount;
                }

            }

            //расчет интервалов между столбцами одного ряда
            if (n < 6)
            {
                LeftIntendantion += 50 * (6 - n);
                RectBarWidth += 10 * (6 - n);
                Bar.LeftDataIndent -= 3 * (6 - n); //введено для однорядной диаграммы
            }
            else //при однорядной диаграмме при 7 и более - нижние подписи смещаются вправо
            {
                if (n < 10)
                {
                    LeftIntendantion -= 20 * (n - 6);
                    Bar.LeftDataIndent += 2 * (6 - n);//введено для однорядной диаграммы

                }
                else
                {
                    Orientation = 340; //наклон подписей
                    LeftIntendantion -= 15 * (n - 5); //уменьшаем отступы
                    RectBarWidth -= 5 * (n - 10); //уменьшаем ширину столбца
                                                  //поправки первоначальных отступов для того, чтобы поместились все столбцы
                    Bar.BarLeftIndent -= 30;
                    Bar.LeftDataIndent -= 25;
                }
            }
        }

        //используется в CreateItemDiagram
        private Rectangle CreateRect(string SpecialityTitleCol, int NumberColor)
        {

            Rectangle TempRect = new Rectangle();
            TempRect.Uid = "столбец";
            TempRect.Width = RectBarWidth;
            TempRect.ToolTip = SpecialityTitleCol;
            TempRect.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(BarsColors[NumberColor]));
            Grid.SetRow(TempRect, 0);
            Grid.SetColumn(TempRect, 0);
            Grid.SetColumnSpan(TempRect, 4);
            return TempRect;
        }
        //используется в CreateItemDiagram
        private TextBlock CreateTBLabel(string text)
        {
            TextBlock TempTB = new TextBlock();
            TempTB.Text = text;
            TempTB.Uid = "подпись";
            TempTB.FontSize = 15;
            TempTB.FontWeight = FontWeights.Bold;
            TempTB.TextAlignment = TextAlignment.Left;
            TempTB.Width = 100;
            TempTB.Height = 400;
            TempTB.LayoutTransform = new RotateTransform(Orientation);
            TempTB.Foreground = Brushes.Black;
            TempTB.Padding = new System.Windows.Thickness(0, 6, 0, 0);
            TempTB.TextWrapping = System.Windows.TextWrapping.Wrap;
            TempTB.TextAlignment = System.Windows.TextAlignment.Center;
            TempTB.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            TempTB.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Grid.SetColumn(TempTB, 0);
            Grid.SetColumnSpan(TempTB, 4);
            return TempTB;
        }
        //используется в CreateItemDiagram
        private TextBlock CreateTBData(string text)
        {
            TextBlock TempTB = new TextBlock();
            TempTB.Text = text;
            TempTB.Uid = "данные";
            TempTB.FontSize = 15;
            TempTB.FontWeight = FontWeights.Bold;
            TempTB.Width = 100;
            TempTB.Height = 50;
            TempTB.Foreground = Brushes.Black;
            TempTB.Padding = new System.Windows.Thickness(0, 6, 0, 0);
            TempTB.TextWrapping = System.Windows.TextWrapping.Wrap;
            TempTB.TextAlignment = System.Windows.TextAlignment.Center;
            TempTB.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            TempTB.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Grid.SetColumn(TempTB, 0);
            Grid.SetColumnSpan(TempTB, 4);
            return TempTB;
        }



    }
}
