using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;

namespace Lab4_Control
{
    class Blast
    {
        public Image ExplosionImage { get; set; }
        public Canvas Owner { get; set; }

        DispatcherTimer BlastTimer = new DispatcherTimer();
        public Blast()
        {
            BlastTimer.Tick += BlastTimer_Tick;
            BlastTimer.Interval = TimeSpan.FromSeconds(0.5);
            BlastTimer.Start();
        }

        private void BlastTimer_Tick(object sender, EventArgs e)
        {
            BlastTimer.IsEnabled = false;
            (this.Owner).Children.Remove(ExplosionImage);
        }
    }
}
