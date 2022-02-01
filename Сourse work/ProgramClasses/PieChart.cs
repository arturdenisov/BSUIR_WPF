using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;

namespace DenisovArt_Kurs
{	
    class PieChart
    {
        //список подписей для секторов круга
        List<double> ListSegments;
        //центр круга
        Point CenterPie;
        //для хранения ссылки на коллекцию грида
        UIElementCollection UC_ElementCollection;
        //определяет размеры круга
        Ellipse PieControl;

        //цвета сегментов
        string[] SegmentColors = { "Red", "Blue", "Green", "Yellow", "Violet",
            "Orange", "AliceBlue", "DarkCyan", "DarkMagenta", "BurlyWood" };

        //общая сумма всех переменных круговой диаграммы
        double Sum = 0;

        //изменение свойства PathPieChanged - сектор выделен
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(prop));
            }
        }

        //при выделении сектора - можно менять его цвет и границу
        Path PathPieDiagramIsChanged;

        public Path PathPieChanged
        {
            get
            { return PathPieDiagramIsChanged; }
            set
            { PathPieDiagramIsChanged = value; OnPropertyChanged("PieChanged"); }
        }

        public PieChart(List<double> DataForPie, UIElementCollection UCItem)
        {
            ListSegments = new List<double>();
            UC_ElementCollection = UCItem;
            DataProcessing(DataForPie);
            //создаем шаблон круга
            PieControl = new Ellipse();
            PieControl.Width = 300;
            PieControl.Height = 300;
            CenterPie = new Point(PieControl.Width / 2, PieControl.Height / 2);
        }

        //перевод объекта данного класса в стартовое состояние
        public void StartingStateFabrik()
        {
            Sum = 0;
            ListSegments.Clear();
            UIElement item;
            for (int i = 0; i < UC_ElementCollection.Count; i++)
            {
                item = UC_ElementCollection[i];
                if ((item.Uid == "сектор") || (item.Uid == "легенда"))
                { UC_ElementCollection.Remove(item); i--; }
            }
        }

        //создание круговой диаграммы
        public void CreateDiagram(List<string> ListPositions)
        {
            //по умолчанию радианы осчитываются от оси Х
            //для диаграммы делаем поправку на 90 градусов
            //(сегменты откладываются от 12 часов)
            double angle = -Math.PI / 2;
            List<Path> ListSector = new List<Path>();

            foreach (double item in ListSegments)
            {
                //создаем объект Geometry, который будет содержать piediagram
                Geometry geometry;

                //Path - рисует последовательность соединенных линий и кривых.
                //в свойство Data объекта данного класса будет передан объект класса Geometry, определяющий рисуемую фигуру.
                Path path = new Path();

                if (item == 360)
                {
                    //EllipseGeometry - представляет геометрию окружности или эллипса.
                    //в конструкторе: 1)центр, 2-3) 2 радиуса (у нас круг - поэтому одни одинаковы)
                    geometry = new EllipseGeometry(new Point(PieControl.Width / 2, PieControl.Height / 2), PieControl.Width / 2, PieControl.Height / 2);
                }
                else
                {
                    //PathGeometry - представляет сложную фигуру, которая может состоять из дуг, кривых, эллипсов, линий и прямоугольников.
                    //в ее коллекицю мы будем добавлять созданные сектора, соответствующие доле признака
                    geometry = new PathGeometry();

                    //находим координаты для 90градусов на окружности 
                    double x = CalculatingCoordinate("x", angle);
                    double y = CalculatingCoordinate("y", angle);

                    //создаем линию от центра окружности к (x,y)
                    //начальная точка линии является конечной точкой предыдущего сегмента
                    //или является StartPoint если других сегментов не существует.
                    LineSegment lingeSeg = new LineSegment(new Point(x, y), true); //параметры: конечная точка, отображать линию

                    //рассчитываем координаты сегмента для нового угла,
                    //который откладываем от 90 градусов и выше
                    angle += Math.PI * item / 180.0;
                    x = CalculatingCoordinate("x", angle);
                    y = CalculatingCoordinate("y", angle);

                    //создаем дугу
                    //параметры ArcSegment (класс реализует эллиптическую дугу между двумя точками): 
                    //Point - конечная точка дуги; начальная точка дуги определяется как текущая точка PathFigure, к которому добавляется ArcSegment.
                    //Size - радиусы арки X и Y, Sharex1InGradus - поворот (угол, в соотв. с которым строится дуга)
                    //Sharex1InGradus > 180 - указывает, должна ли дуга быть больше 180 градусов.
                    //SweepDirection - определяем, в какую сторону необходимо рисовать дугу:
                    //Clockwise - в направлении положительного угла, Counterclockwise - в направлении отрицательного угла, следует задать значение.
                    //isStroked - true, чтобы вычертить дугу, если тип Pen используется для отрисовки сегмента; иначе — значение false.
                    ArcSegment arcSeg = new ArcSegment(new Point(x, y), new Size(PieControl.Width / 2, PieControl.Height / 2), item, item > 180, SweepDirection.Clockwise, true); //false

                    //создаем вторую линию - от конца дуги (arcSeg) к центру окружности
                    LineSegment lingeSeg2 = new LineSegment(CenterPie, true);

                    //PathFigure - представляет подраздел геометрии, одну соединенную последовательность двумерных геометрических сегментов.
                    //в конструкторе - 1) стартовая точка 2)совокупность сегментов 3)IsClosed - определяет для данной фигуры соеденены ли первые и последние сегменты.
                    //PathSegment - представляет сегмент объекта PathFigure.
                    PathFigure fig = new PathFigure(CenterPie, new PathSegment[] { lingeSeg, arcSeg, lingeSeg2 }, false);

                    //добавляем сформированный сектор (fig) в коллекцию фигур нашей piediagram
                    ((PathGeometry)geometry).Figures.Add(fig);
                }

                //делаем заливку сектора
                int index = ListSegments.IndexOf(item);
                path.Fill = SegmentFill(index);
                path.Uid = "сектор";
                path.DataContext = ListPositions[index];
                path.PreviewMouseLeftButtonDown += Path_PreviewMouseLeftButtonDown;

                //Для отображения созданной фигуры - передаем свойству Data объект geometry 
                path.Data = geometry;
                //добавляем все в коллекцию грида и настраиваем отображение
                UC_ElementCollection.Add(path);
                Grid.SetColumn(path, 0);
                Grid.SetColumnSpan(path, 4);

                path.Margin = new Thickness(120, 50, 0, 0);
                ListSector.Add(path);

            }

            //создаем легенду диаграммы
            CreateChartLegend(ListSector);
        }

        //создание легенды диаграммы
        private void CreateChartLegend(List<Path> ListSector)
        {
            Label DiagramTitle = new Label();
            DiagramTitle.Uid = "легенда";
            DiagramTitle.Width = 650;
            DiagramTitle.Height = 100;
            DiagramTitle.Content = "Структура реализованных услуг по профилю врачей";
            DiagramTitle.FontFamily = new FontFamily("Times New Roman");
            DiagramTitle.FontSize = 25;
            DiagramTitle.FontWeight = FontWeights.Bold;
            Grid.SetColumn(DiagramTitle, 0);
            Grid.SetRow(DiagramTitle, 0);
            Grid.SetColumnSpan(DiagramTitle, 4);
            UC_ElementCollection.Add(DiagramTitle);
            DiagramTitle.Margin = new Thickness(20, 0, 0, 350);

            StackPanel LegendaDiagram = new StackPanel();
            LegendaDiagram.Orientation = Orientation.Vertical;
            LegendaDiagram.HorizontalAlignment = HorizontalAlignment.Left;
            LegendaDiagram.Uid = "легенда";
            Label LegendaTitle = new Label();
            LegendaTitle.Content = "Профили врачей";
            LegendaTitle.FontFamily = new FontFamily("Times New Roman");
            LegendaTitle.FontSize = 20;
            LegendaTitle.FontWeight = FontWeights.Bold;
            LegendaDiagram.Children.Add(LegendaTitle);

            //создаем элементы стека диаграммы          
            for (int j = 0; j < ListSector.Count; j++)
            {
                StackPanel StackItem = new StackPanel();
                StackItem.Orientation = Orientation.Horizontal;
                StackItem.HorizontalAlignment = HorizontalAlignment.Left;
                StackItem.Uid = "ЭлементЛегенды";
                StackItem.Name = "MyElement";
                Rectangle rect = new Rectangle();
                rect.Width = rect.Height = 20;
                rect.Fill = ListSector[j].Fill;
                rect.Uid = (ListSector[j].DataContext as string);

                Label position = new Label();
                double temp = ListSegments[j] / 360 * Sum;
                position.Content = "--" + (ListSector[j].DataContext as string) + " (" + temp.ToString("#.##") + ");";
                position.FontFamily = new FontFamily("Times New Roman");
                position.FontSize = 20;
                position.FontWeight = FontWeights.Bold;

                StackItem.Children.Add(rect);
                StackItem.Children.Add(position);

                LegendaDiagram.Children.Add(StackItem);
            }

            Grid.SetColumn(LegendaDiagram, 0);
            Grid.SetRow(LegendaDiagram, 0);
            Grid.SetColumnSpan(LegendaDiagram, 4);
            UC_ElementCollection.Add(LegendaDiagram);
            LegendaDiagram.Margin = new Thickness(500, 50, 0, 0);

        }

        //вспомогательные методы
        //первод данных в углы диаграммы (в градусах) - используется в конструкторе
        public void DataProcessing(List<double> DataForPie)
        {

            ListSegments = new List<double>();
            int countItem = DataForPie.Count;
            for (int i = 0; i < countItem; i++)
            { Sum += DataForPie[i]; }
            for (int i = 0; i < countItem; i++)
            {
                ListSegments.Add((DataForPie[i] / Sum) * 360); //определяем долю угла каждой переменной
            }

        }

        //окраска сектора диаграммы, используется в CreateDiagram
        private SolidColorBrush SegmentFill(int x)
        {
            int count = SegmentColors.Length;
            if (x < count)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(SegmentColors[x]));
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("Gray"));
            }
        }

        //расчет координат X и У для сектора, используется в CreateDiagram
        private double CalculatingCoordinate(string typeCoord, double angle)
        {
            double resultCoord = 0;
            switch (typeCoord)
            {
                case "x": { resultCoord = Math.Cos(angle) * PieControl.Width / 2 + PieControl.Width / 2; break; }
                case "y": { resultCoord = Math.Sin(angle) * PieControl.Height / 2 + PieControl.Height / 2; break; }
            }
            return resultCoord;
        }

        //обработка события - клик по сектору; подписка - в CreateDiagram
        private void Path_PreviewMouseLeftButtonDown(object sender, EventArgs e)
        {
            PathPieChanged = (sender as Path);
        }








    }
}

