using System;

namespace Lab1
{
    internal class Calculator
    {
        private double x;
        private double y;
        private double z;

        public Calculator(double X, double Y, double Z)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }

        public string DoCalculation()
        {

            double step1 = 2.0 * Math.Cos(x - Math.PI / 6.0);
            double step2 = 0.5 + Math.Pow(Math.Sin(y), 2);

            double stepControl = 3.0 - z * z / 5.0;
            if (stepControl == 0) { return "Division by zero!"; }

            double step3 = 1.0 + z * z / stepControl;
            double result = step1 / step2 * step3;
            return Convert.ToString(result);
        }
    }
}

