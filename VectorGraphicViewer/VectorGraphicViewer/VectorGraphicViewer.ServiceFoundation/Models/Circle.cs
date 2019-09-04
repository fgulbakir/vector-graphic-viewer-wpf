using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VectorGraphicViewer.ServiceFoundation
{
    public class Circle
    {
        public DoubleCollection Center { get; set; }
        public float RadiousX { get; set; }
        public Brush FilledColor { get; set; }
        public string LineType { get; set; }

    }
}
