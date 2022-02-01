using System;
using System.Windows;
using System.Threading;
using System.ComponentModel;

namespace Lab6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Calculation integralCalcTask;
        Thread dispatcherThread;
        internal BackgroundWorker backgroundWorker;

        public MainWindow()
        {
            InitializeComponent();
            backgroundWorker = (BackgroundWorker)this.Resources["worker"];
        }

        //Вызов диалогового окна и ввод данных для расчета интеграла
        private void InputDataWindow_Click(object sender, RoutedEventArgs e)
        {
            integralCalcTask = null;
            Window_DataInput inputWindow = new Window_DataInput();
            inputWindow.Owner = this;
            inputWindow.ShowDialog();

            //Эта часть кода исполняется после выхода из диалогового окна.
            //Если пользователь в диалоговом окне корректно указал данные для расчетов - активировать кнопки.
            if (integralCalcTask != null)
            {
                Btn_Calculate.IsEnabled = true;
                Btn_CalculateBckWrk.IsEnabled = true;
            }
        }

        //=========================DISPATCHER=============================//
        //Создание нового потока и начало расчетов
        private void StartCalulation_Click(object sender, RoutedEventArgs e)
        {
            PrgBr.Value = 0;
            Btn_InputData.IsEnabled = false;
            Btn_Calculate.IsEnabled = false;
            Btn_CalculateBckWrk.IsEnabled = false;
            integralCalcTask.ProgressChanged += Dispatcher_ProgressIndicator;
            integralCalcTask.ProgressResult += Dispatcher_ProgressResult;
            integralCalcTask.CalculationCompleted += Dispatcher_CalculationCompleted;

            dispatcherThread = new Thread(integralCalcTask.TrigonometricIntegral_Dispatcher);
            dispatcherThread.IsBackground = true;
            dispatcherThread.Priority = ThreadPriority.Normal;
            dispatcherThread.Start();
        }

        //Обновление статус-бара
        private void Dispatcher_ProgressIndicator(double progress)
        {
            Action act = () =>
            {
                PrgBr.Value = progress;
                PrgBr.Minimum = integralCalcTask.beginInterval;
                PrgBr.Maximum = integralCalcTask.endInterval;
            };
            PrgBr.Dispatcher.BeginInvoke(act);
        }

        //Обновление промежуточного результата расчетов
        private void Dispatcher_ProgressResult(double result)
        {
            Action act = () =>
            {
                Txb_Result.Text = String.Format("{0:0.0000}", result);

            };
            Txb_Result.Dispatcher.BeginInvoke(act);
        }

        //Обновление интерфейса после завершения расчетов и завершение потока
        private void Dispatcher_CalculationCompleted()
        {
            Action act = () =>
            {
                Btn_InputData.IsEnabled = true;
                PrgBr.Value = 0;
            };
            Btn_InputData.Dispatcher.BeginInvoke(act);
            dispatcherThread.Abort();
        }

        //=========================BCK_WORKER=============================//
        //Инициализация асинхронных расчетов
        private void BckWorkerStartCalulation_Click(object sender, RoutedEventArgs e)
        {
            Btn_InputData.IsEnabled = false;
            Btn_CalculateBckWrk.IsEnabled = false;
            Btn_Calculate.IsEnabled = false;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerAsync(integralCalcTask);
        }

        //Проведение асинхронных расчетов в соответствии с шаблоном работы BCK-WORKER (вызов события DoWork)
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Calculation calc = (Calculation)e.Argument;
            e.Result = calc.TrigonometricIntegral_BckWorker();
        }

        //Обновление статус-бара
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PrgBr.Value = e.ProgressPercentage;
        }


        //Обновление интерфейса после завершения расчетов
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Txb_Result.Text = String.Format("{0:0.0000}", e.Result);
            Btn_InputData.IsEnabled = true;
            PrgBr.Value = 0;
        }
    }
}
