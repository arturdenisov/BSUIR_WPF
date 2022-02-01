using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DenisovArt_Kurs
{	
    class Bar
    {
        //отступ столбца от оси Х
        static int LeftIndent = -500;
        public static int BarLeftIndent
        { get { return LeftIndent; } set { LeftIndent = value; } }

        //отступ значения столбца от оси Х
        static int LabelLeftIndent = 100;
        public static int LeftDataIndent
        { get { return LabelLeftIndent; } set { LabelLeftIndent = value; } }

        //высота столбца
        double BarHeight = 305;
        public double MyBarHeight
        { get { return BarHeight; } set { BarHeight = value; } }

        //координаты размещения столбца
        Thickness BarCoord = new Thickness(LeftIndent, 0, 0, 20);
        public Thickness BarMargin
        { get { return BarCoord; } set { BarCoord = value; } }

        //координаты размещения подписи столбца
        Thickness TBLabelCoord = new Thickness(LeftDataIndent + 20, 350, 0, 0);
        public Thickness TBMargin
        { get { return TBLabelCoord; } set { TBLabelCoord = value; } }

        //координаты размещения значения столбца
        Thickness TBDataCoord = new Thickness(LeftDataIndent, 45, 0, 0);
        public Thickness TBDataMargin
        { get { return TBDataCoord; } set { TBDataCoord = value; } }

        public Bar(double x, double y)
        {
            //настройка высоты столбца относительно максимального значения
            MyBarHeight = (MyBarHeight * y);
            BarCoord.Top += (300 - MyBarHeight);
            //смещение координат столбца
            BarCoord.Left += x;
            //смещение координат подписи столбца
            TBLabelCoord.Left += x / 2;
            //смещение координат данных столбца
            TBDataCoord.Top += (280 - MyBarHeight);
            TBDataCoord.Left += x / 2;
        }
    }
}
