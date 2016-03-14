using System.Windows;
using BenchLab.ErrorLog;
using BenchLab.Resources;
using BenchLab.Widget;
using BenchLab.Widget.Core;

namespace BenchLab.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ScreenManager WindowManager;
        public static WidgetManager WidgetManager;
        private void ApplicationDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            NLogLogger.LogError(e.Exception, ExceptionResources.ExceptionOccured);
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            Envi.Language = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
            Envi.AnimationEnabled = true;

            WindowManager = new ScreenManager();
            WidgetManager = new WidgetManager();
        }
    }
}
