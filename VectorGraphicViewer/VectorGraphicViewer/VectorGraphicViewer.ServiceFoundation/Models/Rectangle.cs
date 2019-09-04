using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace VectorGraphicViewer.ServiceFoundation
{
    public class Rectangle
    {
        public Point Point1 { get; set; }

        public Point Point2 { get; set; }

        public Point Point3 { get; set; }
       
        public Brush FilledColor { get; set; }

        public string LineType { get; set; }

        public string Filled { get; set; }

    }
}
