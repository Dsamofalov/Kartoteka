using System.Windows;
using GalaSoft.MvvmLight.Threading;
using NLog;

namespace NewKartoteka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            _logger.Error($"Unhandled Exception { e.Exception}");
            e.Handled = true;
        }
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
