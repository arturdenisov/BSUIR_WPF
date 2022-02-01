using System;
using System.Threading;
using System.ComponentModel;

namespace Lab6
{
    class Calculation
    {
        //Делегаты для работы Dispatcher-Object
        public event Action<double> ProgressChanged;
        public event Action<double> ProgressResult;
        public event Action CalculationCompleted;

        //BackWorker-object
        BackgroundWorker backWorker;

        public double beginInterval;
        public double endInterval;
        public Int32 numberOfIntervals;
        private double increment;

        public Calculation(double _begin, double _end, Int32 n, BackgroundWorker _bck)
        {
            beginInterval = _begin;
            endInterval = _end;
            numberOfIntervals = n;
            increment = (endInterval-beginInterval) / numberOfIntervals;
            backWorker = _bck;
        }

        //Метод расчетов для объекта-Диспетчера
        public void TrigonometricIntegral_Dispatcher()
        {
            double result = 0;
            double x;
            for (Int32 i = 1; i <= numberOfIntervals; i ++)
            {
                x = i * increment + beginInterval;
                result = result + Math.Pow(x, 4)*increment;
                Thread.Sleep(25);
                ProgressChanged(x);
                ProgressResult(result);
            }
            CalculationCompleted();
        }

        //Метод расчетов для BckWorker
        public Double TrigonometricIntegral_BckWorker()
        {
            double result = 0;
            double x;
            for (Int32 i = 1; i <= numberOfIntervals; i++)
            {
                x = i * increment + beginInterval;
                result = result + Math.Pow(x, 4) * increment;
                Thread.Sleep(25);
                backWorker.ReportProgress(i*100/numberOfIntervals);
            }
            return result;
        }
    }
}
