using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using VectorGraphicViewer.UI.Helper;

namespace VectorGraphicViewer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GlobalExceptionHandler();
        }


        private void GlobalExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                Helper.ExceptionHandler.GlobalUnhandledExceptionHandler((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
        }
    }
}
