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
using Lab4_Control;
using System.ComponentModel;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LeftButton.Content = CreateArrow("pack://application:,,,/arrow.png");
            RightButton.Content = CreateArrow("pack://application:,,,/arrow2.png");
            UCSea.PropertyChanged += InfoObjectGame_PropertyChanged;

        }

        private Image CreateArrow(string uri)
        {
            BitmapImage btimgArrow = new BitmapImage();
            btimgArrow.BeginInit();
            Uri uArrow = new Uri(uri);
            btimgArrow.UriSource = uArrow;
            btimgArrow.EndInit();
            Image img = new Image();
            img.Source = btimgArrow;
            return img;
        }

        //ОБРАБОТКА КНОПКИ ОГОНЬ
        private void Fire_Click(object sender, RoutedEventArgs e)
        {
            UCSea.CreateTorpedo();
        }


        //ОБРАБОТКА ДВИЖЕНИЯ ПЕРИСКОПА ВПРАВО
        //ОБРАБОТЧИК ДЛЯ НАЖАТИЯ КНОПОК A и D и W
        private void Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            { UCSea.MovePeriscope("right", true); }
            if (e.Key == Key.A)
            { UCSea.MovePeriscope("left", true); }
            if (e.Key == Key.W)
            { UCSea.CreateTorpedo(); }
        }
                   
        //ПЕРЕГРУЗКА ДВИЖЕНИЯ ВПРАВО ДЛЯ НАЖАТИЯ МЫШЬЮ
        private void Right_Down(object sender, EventArgs e)
        {
            UCSea.MovePeriscope("right", true);
        }

        //ОБРАБОТКА ДВИЖЕНИЯ ВЛЕВО ДЛЯ НАЖАТИЯ МЫШЬЮ
        private void Left_Down(object sender, EventArgs e)
        {
            UCSea.MovePeriscope("left", true);
        }
        
        //ОБРАБОТКА КНОПКИ СТАРТ \ ПАУЗА
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (UCSea.StartMyGame == false) UCSea.StartMyGame = true;
            else UCSea.StartMyGame = false;
        }
        
        //ОБРАБОТКА КНОПКИ СБРОС
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            UCSea.ResetGame();
            SpeedTorpedo.Text = Coordinates.Text = String.Empty;
        }

        //ДЛЯ ВЫВОДА ИНФОРМАЦИИ О КООРДИНАТАХ КОРАБЛЯ И СКОРОСТИ ТОРПЕДЫ - ПОДПИСКА ПРИ ИНИЦИАЛИЗАЦИИ USER CONTROL
        private void InfoObjectGame_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            switch (e.PropertyName)
            {
                case "CoordinatesShip": Coordinates.Text = Convert.ToString(UCSea.Coordinates); break;
                case "Speed": SpeedTorpedo.Text = Convert.ToString(UCSea.Speed); break;
            }
        }
    }
}   
