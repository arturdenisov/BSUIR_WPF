using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace Lab3_1
{

    public class Calculator
    {
        private double xStart;
        private double xEnd;
        private double increment;
        private Int32 nIterations;
        private Int32 numIdexes;
        private List<String> listResults;

        public double XStart
        {
            get { return xStart; }
            set
            {
                if (value > xEnd) throw new ArgumentException("X начальное должно быть меньше X конечного.");
                if( value < 0) throw new ArgumentException("Значение должно быть не меньше 0.");
                xStart = value; ;
            }
        }

        public double XEnd
        {
            get { return xEnd; }
            set
            {
                if (value < 0) throw new ArgumentException("Значение должно быть не меньше 0.");
                if (value < xStart) throw new ArgumentException("X конечное должно быть больше X начального.");
                xEnd = value;
            }
        }
        public double Increment
        {
            get { return increment; }
            set
            {

                if (value < 0) throw new ArgumentException("Значение должно быть не меньше 0.");
                increment = value;
            }
        }
        public Int32 NIterations
        {
            get { return nIterations; }
            set
            {
                if (value < 0) throw new ArgumentException("Значение должно быть не меньше 0.");
                nIterations = value;
            }
        }

        public Calculator()
        {
            listResults = new List<string>();
        }

        public List<String> Calculate()
        {
            if (listResults.Count > 0) listResults.Clear();
            double[] xResults = Calculate_Sx();
            double[] yResults = Calculate_Sy();
            for (Int32 i = 0; i < xResults.Count(); i++)
            {
                double xActual = xStart + increment * (i);
                listResults.Add(String.Format("X={0:0}   S(X)={1:0.00}   Y(x)={2:0.00}", xActual, xResults[i], yResults[i]));
            }
            return listResults;
        }

        private double[] Calculate_Sx()
        {
            numIdexes = (Int32)Math.Truncate((Math.Abs(xEnd - xStart) / increment))+1;
            double[] results = new double[numIdexes];
            double xAct = xStart;
            for (Int32 x = 0; x < numIdexes; x++)
            {
                results[x] = Equation_Sx(xAct);
                xAct = xAct + increment;
            }
            return results;
        }

        private double[] Calculate_Sy()
        {
            numIdexes = (Int32)Math.Truncate((Math.Abs(xEnd - xStart) / increment))+1;
            double[] results = new double[numIdexes];
            double xAct = xStart;

            for (Int32 x = 0; x < numIdexes; x++)
            {
                results[x] = Equation_Yx(xAct);
                xAct = xAct + increment;
            }
            return results;
        }

        private double Equation_Sx(double xAct)
        {
            double sum = 0;
            for (Int32 k = 1; k < nIterations; k++)
            {
                //     sum = sum + (Math.Pow(Math.Log(3, Math.E),k) / Factorial(k)) * Math.Pow(xAct, k);
                sum = sum + Math.Cos(k * xAct) / k;
            }
            return sum;
        }

        private double Equation_Yx(double xAct)
        {
            // return Math.Pow(3, xAct)-1;
            return (-Math.Log(Math.Abs(2 * Math.Sin(xAct / 2))));
        }

        private Int32 Factorial(Int32 k)
        {
            if (k == 0 | k == 1)
            {
                return 1;
            }
            else
            {
                Int32 result = 1;
                for (Int32 iteration = 1; iteration <= k; iteration++)
                {
                    result = result * iteration;
                }
                return result;
            }
        }
    }
}
