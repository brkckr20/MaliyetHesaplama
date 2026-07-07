using MaliyeHesaplama.wins;
using System.Windows;
using System.Windows.Threading;

namespace MaliyeHesaplama
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.OnStartup(e);
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var splash = new winSplashScreen();
            splash.ShowSplash(2000);

            var login = new winLogin();
            bool loginSuccess = login.ShowDialog() == true;

            if (!loginSuccess)
            {
                Shutdown();
                return;
            }

            var main = new HomeScreen();
            Current.MainWindow = main;
            main.Show();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(
                $"Beklenmeyen Hata (UI):\n{e.Exception.Message}\n\nStack Trace:\n{e.Exception.StackTrace}",
                "Hata",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            System.Windows.MessageBox.Show(
                $"Beklenmeyen Hata:\n{ex?.Message}\n\nStack Trace:\n{ex?.StackTrace}",
                "Kritik Hata",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(
                $"Beklenmeyen Görev Hatası:\n{e.Exception?.Message}\n\nStack Trace:\n{e.Exception?.StackTrace}",
                "Hata",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            e.SetObserved();
        }
    }
}