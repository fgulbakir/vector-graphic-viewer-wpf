using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VectorGraphicViewer.ServiceFoundation
{
  public class Line
    {
        public float X1 { get; set; }
        public float X2 { get; set; }

        public float Y1 { get; set; }
        public float Y2 { get; set; }

        public Brush Color { get; set; }

        public string LineType { get; set; }
        

    }
}
