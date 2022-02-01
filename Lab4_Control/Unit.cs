using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab4_Control
{
    abstract class Unit : DependencyObject
    {

        public int CoordinateX
        {
            get { return (int)GetValue(CoordXProperty); }
            set { SetValue(CoordXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoordXProperty =
            DependencyProperty.Register("CoordinateX", typeof(int), typeof(Unit), new PropertyMetadata());

        public int CoordinateY
        {
            get { return (int)GetValue(CoordYProperty); }
            set { SetValue(CoordYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoordYProperty = DependencyProperty.Register("CoordinateY", typeof(int), typeof(Unit), new PropertyMetadata(0));

        public abstract void Move();
    }
}

