using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VectorGraphicViewer.UI.Extension;

namespace VectorGraphicViewer.UI.Helper
{
    public class ExceptionHandler
    {
        public static void GlobalUnhandledExceptionHandler(Exception exception, string source)
        {
            string message = exception.GetaAllMessages();
            string caption = $"Unhandled exception ({source})";

            MessageBox.Show(message, caption);
        }
    }
}
