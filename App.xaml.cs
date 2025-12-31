using MaliyeHesaplama.wins;
using System.Windows;

namespace MaliyeHesaplama
{
    public partial class App : System.Windows.Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            // Splash
            var splash = new winSplashScreen();
            splash.ShowSplash(2000);

            // Login
            var login = new winLogin();
            bool loginSuccess = login.ShowDialog() == true;

            if (!loginSuccess)
            {
                Shutdown();
                return;
            }

            // Home
            var main = new HomeScreen();
            Current.MainWindow = main;
            main.Show();
            
        }
    }

}
