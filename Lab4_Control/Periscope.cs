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
    class Periscope : DependencyObject 
    {
        public event PropertyChangedEventHandler PropertyChanged;

 //НАСТРОЙКА ДВИЖЕНИЯ
        public Periscope()
        {
            Displacement = 50; //установка начальной точки
        }

        //метод сдвигания InternalCanvas (в не кораблик и море)
        public void Move(int step)
        {
            if ((Displacement < 220)&&(Displacement > -420))
            { Displacement = Displacement + step; }
            else {
                if (Displacement == 220) { Displacement = Displacement - 10; }
                if (Displacement == -420) { Displacement = Displacement + 10; }
                 }
        }

//ДЛЯ ПРИВЯЗКИ ВНУТРЕННЕГО КАНВАСА К КООРДИНАТАМ ОБЪЕКТА КЛАССА PERISCOPE
        //свойство передвижения - задает новую координату Х для InternalCanvas (в котором - море и кораблик)
        public int Displacement
        {
            get { return (int)GetValue(DisplacementProperty); }
            set { SetValue(DisplacementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplacementProperty =
            DependencyProperty.Register("Displacement", typeof(int), typeof(Periscope), new PropertyMetadata());

 //ПРИВЯЗКА ДВИЖЕНИЯ К КНОПКАМ КЛАВИАТУРЫ И МЫШИ
        //свойство смещения влево - для связи с кнопкой интерфейсв "<<"
        bool LeftDown = false;
        public bool DownLeft
        {
            set { LeftDown = value; OnPropertyChanged("LeftDown"); }
            get { return LeftDown; }
        }
        //свойство смещения вправо - для связи с кнопкой интерфейсв ">>"
        bool RightDown = false;
        public bool DownRight
        {
            set { RightDown = value; OnPropertyChanged("RightDown"); }
            get { return RightDown; }
        }
        
        //для обработки события изменения свойства (смещение влево и смещение вправо)
        protected void OnPropertyChanged(string UCproperty)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(UCproperty)); }
        }
               
    }
}
