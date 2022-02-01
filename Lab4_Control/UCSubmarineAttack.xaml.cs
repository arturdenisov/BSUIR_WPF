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
using System.Windows.Threading;
using WpfAnimatedGif;
using System.ComponentModel;
using System.Windows.Media.Animation;



namespace Lab4_Control
{
    /// <summary>
    /// Логика взаимодействия для UCSubmarineAttack.xaml
    /// </summary>
    public partial class UCSubmarineAttack : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;
        DispatcherTimer Timer;

        Ship Warship;
        Periscope Eye;
        Torpedo weapon;
        Image imTorpedo;
        Blast BlastShip;

//ОСНОВНАЯ НАСТРОЙКА ИГРЫ
        //баллы за сбитые корабли
        public int ResultGame
        { set; get; }

        public UCSubmarineAttack()
        {
            InitializeComponent();
            this.PropertyChanged += Start_PropertyChanged;
            //привязка объекта корабль (изменения его координат) к картинке
            Warship = new Ship();
            ShipImage.DataContext = Warship;
            ShipImage.SetBinding(Canvas.TopProperty, new Binding("CoordinateY"));
            ShipImage.SetBinding(Canvas.LeftProperty, new Binding("CoordinateX"));
            //подписка на событие клик левой клавишей по объекту игры
            ExternalCanvas.MouseLeftButtonDown += ObjectGame_Click;

            //привязка перископа к внутреннему канвасу
            //(т.е. перемещение перископа = перемещение содержимого внутреннего канваса относительно статичной картинки перископа)
            Eye = new Periscope();
            InternalCanvas.DataContext = Eye;
            InternalCanvas.SetBinding(Canvas.LeftProperty, new Binding("Displacement"));

            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromSeconds(0.005);
            ResetGame(); 
        }

        //для перезапуска игры
        public void ResetGame()
        {
            //обнуляем
            StartMyGame = false;
            ResultGame = 0;
            MyPoints.Content = String.Empty;
            //корабль - на нач. позицию
            Warship.CoordinateX = 50;
            Warship.CoordinateY = 160;
            //перескоп - на нач. позицию
            Eye.Displacement = 50;
            DeleteTorpedos();

            ShipImage.Visibility = Visibility.Visible;
            if (StartMyGame == true)
             {  Timer.Start(); }
        }

        //таймер игры
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Warship.CoordinateX > 1000) //на каждом новом круге корабль снова становится видимым
            {
                Timer.Interval = TimeSpan.FromSeconds(0.005);
                ShipImage.Visibility = Visibility.Visible;
                Warship.CoordinateX = 50;
            }
            Warship.Move();
        }

//ПРИВЯЗКА ПЕРИСКОПА К КООРДИНАТАМ ВНУТРЕННЕГО(!) КАНВАСА
        //перемещение внутреннего канваса относительно перископа
        public void MovePeriscope(string where, bool action)
        {
            if (where == "right")
            {
                Eye.DownRight = action;
                Eye.Move(-5);
            }
            else
            {
                Eye.DownLeft = action;
                Eye.Move(5);
            }
        }

//СОЗДАНИЕ И УПРАВЛЕНИЕ ПЕРЕМЕЩЕНИЕМ ТОРПЕД
        //динамичное создание торпед
        public void CreateTorpedo()
        {
            if (StartMyGame == false) return;
            imTorpedo = new Image();
            imTorpedo.Source = new BitmapImage(new Uri(@"images\torpedo2.png", UriKind.Relative));
            imTorpedo.Height = 100;
            imTorpedo.Width = 50;
            imTorpedo.Uid = "tor";
            //+imTorpedo.MouseLeftButtonDown += ObjectGame_Click;

            //задаем координаты торпеде - в зависмости от смещения перископа
            if ((Eye.Displacement <= 0)) 
            {
                Torpedo.PeriscopeCoordX = 370 + (Eye.Displacement)*(-1); 
            }
            else
            {
                Torpedo.PeriscopeCoordX =370 - (Eye.Displacement);
            }
            weapon = new Torpedo();
            imTorpedo.DataContext = weapon;
            imTorpedo.SetBinding(Canvas.TopProperty, new Binding("CoordinateY"));
            imTorpedo.SetBinding(Canvas.LeftProperty, new Binding("CoordinateX"));
            InternalCanvas.Children.Add(imTorpedo);
            //подписка на событие изменения свойств торпеды - удаление торпеды которая "ушла за горизонт" и уменьшение ее размера
            weapon.PropertyChanged += Torpedo_PropertyChanged;
        }

         //Определение попадания и обработка изменения свойств торпеды - ее размера и достижения горизонта 
        private void Torpedo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int x, y; //координаты торпеды - используются для определения попадания и взрыва
            Image imageTorpedo = new Image();

            //дочерние элементы внутреннего канваса  - для поиска нужной картинки торпеды
            UIElementCollection Elements = InternalCanvas.Children;
            foreach (UIElement item in Elements)
            {
                if (item is Image)
                    //определяем торпеду
                    if ((item as Image).DataContext == (sender as Torpedo))
                        imageTorpedo = (item as Image);
            }

            switch (e.PropertyName)
            {
                case "ReductionInSize":
                    {
                        //уменьшаем размер торпеды
                        imageTorpedo.Height = (sender as Torpedo).ReductionInSize;
                        break;
                    }
                case "Horizont":
                    {
                        //удаляем изображение торпеды
                        InternalCanvas.Children.Remove(imageTorpedo);
                        break;
                    }
            }

            //определение попадания 
            x = (sender as Torpedo).CoordinateX;
            y = (sender as Torpedo).CoordinateY;
            HitTestResult result = VisualTreeHelper.HitTest(InternalCanvas, new Point(x, y));
            if (result != null)
            {
                var element = result.VisualHit as FrameworkElement;
                if ((element.Name == "ShipImage") && (element.Visibility == Visibility.Visible))
                { element.Visibility = Visibility.Collapsed;
                    ResultGame = ResultGame + 5;
                    MyPoints.Content =("Баллы: "+ResultGame).ToString();
                    this.CreateExplosion(x, y);
                    Timer.Interval = TimeSpan.FromSeconds(0.0001); //ускоряем завершение круга кораблем-невидимкой
                }
            }
        }

        //остановка всех торпед
        private void StopTorpedoes(bool command)
        {
            Image Torpedoes;
            UIElementCollection Elements = InternalCanvas.Children;
            foreach (UIElement item in Elements)
            {
                if (item is Image)
                    if ((item as Image).DataContext is Torpedo)
                    { Torpedoes = (item as Image); (Torpedoes.DataContext as Torpedo).Stop(command); }
            }
        }

        //удаление всех торпед при перезапуске игры
        private void DeleteTorpedos()
        {
            int count = 1;
            while (count > 0)
            {
                UIElementCollection Col = InternalCanvas.Children;
                foreach (UIElement item in Col)
                {
                    if (item is Image)
                    {
                        if ((item as Image).Uid == "tor")
                        {
                            imTorpedo = item as Image;
                            count = 1;
                        }
                        else count = 0;
                    }
                }
                InternalCanvas.Children.Remove(imTorpedo);
            }

        }

 //СОЗДАНИЕ ВЗРЫВА
        private void CreateExplosion(int x, int y)
        {
            Image Explosion = new Image();
            Explosion.Source = new BitmapImage(new Uri(@"images\Expl2.gif", UriKind.Relative));
            Explosion.Height = 200;
            Explosion.Width = 200;
            BlastShip = new Blast();
            BlastShip.ExplosionImage = Explosion;
            BlastShip.Owner = InternalCanvas;
            Explosion.DataContext = BlastShip;
            ImageBehavior.SetRepeatBehavior(Explosion, RepeatBehavior.Forever);
            ImageBehavior.SetAnimatedSource(Explosion, Explosion.Source);

            InternalCanvas.Children.Add(Explosion);
            Explosion.Margin = new Thickness((x-70), (y-100), 0, 0);
        }

//СВЯЗЬ ИНТЕРФЕЙСА ОКНА С ИГРОЙ
        //событие изменения свойств User Control'a - для привязки действия игры к нажатию клавиш
        protected void OnPropertyChanged(string UCproperty)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(UCproperty)); }
        }
        
        //для реализации старта / паузы игры
        bool StartGame = false;
        public bool StartMyGame
        {
            get { return StartGame; }
            set { StartGame = value; OnPropertyChanged("Start"); }
        }

        private void Start_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StartMyGame == false) { Timer.Stop(); StopTorpedoes(true); }
            else { Timer.Start(); StopTorpedoes(false); }
        }

        //для возврата координат корабля - СМ. MainWindow.xaml.cs метод 
        Point CoordinateShip;
        public Point Coordinates
        { set { CoordinateShip = value; OnPropertyChanged("CoordinatesShip"); } get { return CoordinateShip; } }

        //для возврата скорости торпеды
        double SpeedTorpedo;
        public double Speed
        { set { SpeedTorpedo = value; OnPropertyChanged("Speed"); } get { return SpeedTorpedo; } }

        private void ObjectGame_Click(object sender, MouseEventArgs e)
        {
            
          Point XYclick = Mouse.GetPosition(this);
          double coorY = XYclick.Y;
          double coorX = XYclick.X;
            
          if ((Eye.Displacement <= 0))
            {
               coorX = (double)(XYclick.X + (Eye.Displacement) * (-1)); //500
            }
          else
            {
                coorX = (double)(XYclick.X - Eye.Displacement);
            }
            HitTestResult result = VisualTreeHelper.HitTest(InternalCanvas, new Point(coorX, coorY));
            if (result != null)
            {
                var element = result.VisualHit as FrameworkElement;
              //  MessageBox.Show("Here " + element.Name);
                if (element.Name == "ShipImage") 
                { this.Coordinates = new Point(coorX, coorY); }
                if (element.Uid == "tor")
                { this.Speed = ((element as Image).DataContext as Torpedo).Speed(); }
            }
        }

    }
}
