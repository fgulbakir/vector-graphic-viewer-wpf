using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorGraphicViewer.ServiceFoundation;
using VectorGraphicViewer.ServiceFoundation.Enum;
using VectorGraphicViewer.UI.Core;
using DrawCircle = VectorGraphicViewer.ServiceFoundation.Circle;
using DrawLine = VectorGraphicViewer.ServiceFoundation.Line;

namespace VectorGraphicViewer.UnitTest
{
    [TestClass]
   public class VectorGraphicViewerTest
    {
        [TestMethod]
        public void IsAbstractBaseClassTest()
        {
            var t = typeof(ViewModelBase);
            Assert.IsTrue(t.IsAbstract);
        }

        [TestMethod]
        public void IsIDataErrorInfoTest()
        {

            Assert.IsTrue(typeof(IDataErrorInfo).IsAssignableFrom(typeof(ViewModelBase)));
        }


        [TestMethod]
        public void DrawLine()
        {
            DrawLine drawing = new DrawLine();

            InputFileVector drawLine= new InputFileVector();

            var brush = Brushes.Yellow;

            drawLine.a= " - 1,5; 3,4";

            drawLine.b= "2,2; 5,7";

            drawLine.color = "127; 255; 255; 255";

            drawLine.lineType = "solid"; 
            
            drawing.Color = brush;

            drawing.X1 = -1.5f;

            drawing.X2 = 3.4f;

            drawing.Y1 = 2.2f;

            drawing.Y2 = 5.7f;

            drawing.LineType = LineTypesValues.Solid;
           

        }

        [TestMethod]
        public void DrawCircle()
        {
            DrawCircle drawCircle = new DrawCircle();

            InputFileVector inputFileVector = new InputFileVector();
            
            drawCircle.FilledColor=Brushes.BlueViolet;

            drawCircle.Center = new DoubleCollection(){2.2,2.2 };

            drawCircle.RadiousX = 15.0f;

            drawCircle.LineType= LineTypesValues.Dot;


        }

        [TestMethod]
        public void DrawTriangle()
        {
            var polygon = new Polygon();

            InputFileVector inputFileVector = new InputFileVector();

            var polygonPoints = new PointCollection();

            var point1 = new System.Windows.Point(2.2, 2.8);

            var point2 = new System.Windows.Point(5.1,5.8);

            var point3 = new System.Windows.Point(1.8, 8.0);

            polygonPoints.Add(point1);

            polygonPoints.Add(point2);

            polygonPoints.Add(point3);

            polygon.Points = polygonPoints;

            polygon.Stroke = Brushes.Aqua;

            polygon.StrokeDashArray = new DoubleCollection() { 1, 1 };

        }

        private string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));

                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));

                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
