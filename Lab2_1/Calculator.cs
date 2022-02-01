using System;
using System.Windows;


namespace Lab2_1
{
    static class Calculator
    {
        private static Double? minValue, maxValue, currentValue;
        private static string branch;
        private delegate Double FunDelegate(Double x);

        //делает расчеты согласно заданию
        internal static double DoCalculation(Double[] parameters, String functionType)
        {
            System.Windows.MessageBox.Show((Math.PI / 2).ToString());
            //определение функции, которую выбрал пользователь, и инициализация делегата
            FunDelegate fun = null;
            if (functionType == "SIN") fun = Math.Sin;
            switch (functionType)
            {
                case "SIN": { fun = Math.Sin; break; }
                case "COS": { fun = Math.Cos; break; }
                case "TNG": { fun = Math.Tan; break; }
            }

            //расчет по исходным данным в соответствии с заданием
            double x = parameters[0];
            double y = parameters[1];
            if (x * y > 0) { currentValue = Math.Pow((fun(x) + y), 2) - Math.Pow((fun(x) * y), 0.5); branch = "Branch 1 (xy>0) ";  }
            if (x * y < 0) { currentValue = Math.Pow((fun(x) + y), 2) - Math.Pow(Math.Abs(fun(x) * y), 0.5); branch = "Branch 2 (xy<0)"; }
            if (x * y == 0) { currentValue = Math.Pow((fun(x) + y), 2) + 1; branch = "Branch 3 (xy=0)"; }
                        
            //сохранить экстремальный результат 
            SaveMinMaxResults(currentValue);

            //вернуть результат
            return (double)currentValue;
        }

        //сохраняет экстремальные результаты
        private static void SaveMinMaxResults(double? currentValue)
        {
            if (minValue == null) minValue = currentValue;
            else if (currentValue < minValue) minValue = currentValue;
            if (maxValue == null) maxValue = currentValue;
            else if (currentValue > maxValue) maxValue = currentValue;
        }

        public static double MinValue
        {
            get { return (double)minValue; }
            set { minValue = value; }
        }

        public static double MaxValue
        {
            get { return (double)maxValue; }
            set { maxValue = value; }
        }

        public static string Branch
        { get { return branch; } }
    }
}
