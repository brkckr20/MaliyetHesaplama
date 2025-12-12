using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaliyeHesaplama.wins
{
    /// <summary>
    /// Interaction logic for winSplashScreen.xaml
    /// </summary>
    public partial class winSplashScreen : Window
    {
        public winSplashScreen()
        {
            InitializeComponent();
        }
        public void ShowSplash(int delay)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(delay);

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                this.Close();
            };

            timer.Start();  // 🚀 Bunu unutmayacağız

            this.ShowDialog();  // Splash kapanana kadar bloklar
        }
    }
}
