using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;

namespace Lab4_Control
{
    class Torpedo : Unit, INotifyPropertyChanged
    {
        //для привязки координаты Х к координате Х перископа (см. метод MovePeriscope в UC)
        static int Shift;
        public static int PeriscopeCoordX
        { set {Shift = value; } get { return Shift; } }

        public event PropertyChangedEventHandler PropertyChanged;
        DispatcherTimer TorpedoTimer = new DispatcherTimer();
        int DecelerationCount = 0; //счетчик для уменьшения скорости движения (см. метод Move) и расчета ее скорости (метод Speed)

//настройка привязки события к изменениям свойств уменьшения размера и достижения границы движения

        bool HorizontBorder = false;
        public bool Horizont
        { set { HorizontBorder = value; OnPropertyChanged("Horizont"); } get { return HorizontBorder; } }

        int HeightImage = 100;
        public int ReductionInSize
        { set { HeightImage = value; OnPropertyChanged("ReductionInSize"); } get { return HeightImage; } }

        protected void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(property)); }
        }

        //настройка движения торпеды
        public Torpedo()
        {
            CoordinateX = PeriscopeCoordX;
            CoordinateY = 350;
            TorpedoTimer.Tick += TorpedoTimer_Tick;
            TorpedoTimer.Interval = TimeSpan.FromSeconds(0.1);
            TorpedoTimer.Start();
        }

        public override void Move()
        {
            CoordinateY = CoordinateY - 20 + 1 * DecelerationCount;
        }

        private void TorpedoTimer_Tick(object sender, EventArgs e)
        {
            if (CoordinateY > 200)
            {
                this.Move();
                DecelerationCount++; //для замедления движения торпеды
                ReductionInSize = ReductionInSize - 7;  //для уменьшения размера по мере отдаления
            }
            else //достиг горизонта - останавливаем движение, сообщаем о достижении границы
            {
                TorpedoTimer.Stop();
                Horizont = true;
            }
        }

        //Остановка торпеды
        public void Stop(bool command)
            {
                if (command == true) TorpedoTimer.Stop();
                else TorpedoTimer.Start();
            }

        //скорость торпеды
        public double Speed()
        {
            double range = 350 - this.CoordinateY;
            double speed = (range / DecelerationCount) * 1000; //умножение на 1000 - для реализма
            return speed;
        }

        

    }
}
