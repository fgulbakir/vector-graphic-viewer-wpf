using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using VectorGraphicViewer.ServiceFoundation;
using VectorGraphicViewer.ServiceFoundation.Enum;
using VectorGraphicViewer.UI.Core;
using VectorGraphicViewer.UI.Helper;
using DrawCircle = VectorGraphicViewer.ServiceFoundation.Circle;
using DrawLine = VectorGraphicViewer.ServiceFoundation.Line;


namespace VectorGraphicViewer.UI.ViewModel
{
    public class VectorViewerViewModel : ViewModelBase
    {

        #region Fields

        private ICommand _openFileCommand;

        private string _filePath;

        private ObservableCollection<DrawLine> _lines { get; set; }

        private ObservableCollection<DrawCircle> _circles;

        private ObservableCollection<Polygon> _triangles;

        #endregion

        #region Ctor
        public VectorViewerViewModel()
        {

            _lines = new ObservableCollection<DrawLine>();

            _circles = new ObservableCollection<DrawCircle>();

            _triangles = new ObservableCollection<Polygon>();
        }
        #endregion

        #region Properties
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }

        public ObservableCollection<DrawLine> Lines
        {
            get { return _lines; }
            set
            {
                _lines = value;
                NotifyPropertyChanged("Lines");
            }
        }

        public ObservableCollection<DrawCircle> Circles
        {
            get { return _circles; }
            set
            {
                _circles = value;
                NotifyPropertyChanged("Circles");
            }
        }

        public ObservableCollection<Polygon> Triangles
        {
            get { return _triangles; }
            set
            {
                _triangles = value;
                NotifyPropertyChanged("Triangles");
            }
        }

        #endregion

        #region Commands
        public ICommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand =
                           new HandlerCommand(OpenFileCommandExecute, () => CanOpenFileCommandExecute));
            }
        }

        #endregion

        #region Methods

        public bool CanOpenFileCommandExecute
        {
            get { return true; }
        }

        public bool CanSavePdfCommandExecute
        {
            get { return !string.IsNullOrEmpty(FilePath); }
        }

        public void OpenFileCommandExecute()
        {
            try
            {
                ClearDraw();

                var openFileDlg = new Microsoft.Win32.OpenFileDialog();

                var result = openFileDlg.ShowDialog();

                if (result == true)
                {
                    FilePath = openFileDlg.FileName;

                }

                XmlDocument doc = new XmlDocument();

                FileInfo fi = new FileInfo(FilePath);

                var inputFile = File.OpenText(FilePath);

                JsonSerializer jseri = new JsonSerializer();

                List<InputFileVector> parsedData = null;

                if (fi.Extension == FileFormatValues.XmlFormat)
                {

                    XmlTextReader readXml = new XmlTextReader(inputFile);

                    doc.Load(readXml);

                    doc.RemoveChild(doc.FirstChild);

                    var jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);

                    jsonText = jsonText.Remove(0, 11);

                    jsonText = jsonText.Remove(jsonText.Length - 1);

                    JsonTextReader readerXmlJson = new JsonTextReader(new StringReader(jsonText));

                    parsedData = jseri.Deserialize<List<InputFileVector>>(readerXmlJson);

                }
                if (fi.Extension == FileFormatValues.JsonFormat)
                {

                    JsonTextReader reader = new JsonTextReader(inputFile);

                    parsedData = jseri.Deserialize<List<InputFileVector>>(reader);

                }

                if (fi.Extension != FileFormatValues.JsonFormat && fi.Extension != FileFormatValues.XmlFormat)
                {
                    MessageBox.Show("The file format that is selected is not support!Only support .xml or .json");

                    return;

                }

                foreach (var item in parsedData)
                {
                    switch (Enum.Parse(typeof(VectorTypeEnum), item.type))
                    {
                        case VectorTypeEnum.line:
                            DrawLine(item);
                            break;
                        case VectorTypeEnum.circle:
                            DrawCircle(item);
                            break;
                        case VectorTypeEnum.triangle:
                            DrawTriangle(item);
                            break;

                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public Visual CreateVisual()
        {
            const double Inch = 96;
            DrawingVisual visual = new DrawingVisual();
            DrawingContext dc = visual.RenderOpen();
            Pen bluePen = new Pen(Brushes.Blue, 1);
            dc.DrawRectangle(Brushes.Yellow, bluePen, new Rect(Inch / 2, Inch / 2, Inch * 1.5, Inch * 1.5));
            Brush pinkBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 255));
            Pen blackPen = new Pen(Brushes.Black, 1);
            dc.DrawEllipse(pinkBrush, blackPen, new Point(Inch * 2.25, Inch * 2), Inch * 1.25, Inch);
            dc.Close();
            return visual;
        }

        public void DrawLine(InputFileVector drawLine)
        {

            DrawLine drawing = new DrawLine();

            var brush = FindBrushByARGB(drawLine.color);

            var xCoordinates = drawLine.a.Split(';');

            var yCoordinates = drawLine.b.Split(';');

            drawing.Color = brush;

            drawing.X1 = float.Parse(xCoordinates[0]);

            drawing.X2 = float.Parse(xCoordinates[1]);

            drawing.Y1 = float.Parse(yCoordinates[0]);

            drawing.Y2 = float.Parse(yCoordinates[1]);

            switch (Enum.Parse(typeof(System.Drawing.Drawing2D.DashStyle), FirstCharToUpper(drawLine.lineType)))
            {
                case System.Drawing.Drawing2D.DashStyle.Dash:
                    drawing.LineType = LineTypesValues.Dash;
                    break;
                case System.Drawing.Drawing2D.DashStyle.Dot:
                    drawing.LineType = LineTypesValues.Dot;
                    break;
                case System.Drawing.Drawing2D.DashStyle.Solid:
                    drawing.LineType = LineTypesValues.Solid;
                    break;
                case System.Drawing.Drawing2D.DashStyle.DashDot:
                    drawing.LineType = LineTypesValues.DashDot;
                    break;
            }

            Lines.Add(drawing);


        }
        public void DrawCircle(InputFileVector drawCircle)
        {


            DrawCircle drawing = new DrawCircle();

            var brush = FindBrushByARGB(drawCircle.color);

            drawing.FilledColor = brush;

            drawing.RadiousX = drawCircle.radius;

            var centers = drawCircle.center.Split(';');

            drawing.Center = new DoubleCollection() { double.Parse(centers[0]), double.Parse(centers[1]) };

            switch (Enum.Parse(typeof(System.Drawing.Drawing2D.DashStyle), FirstCharToUpper(drawCircle.lineType)))
            {
                case System.Drawing.Drawing2D.DashStyle.Dash:
                    drawing.LineType = LineTypesValues.Dash;
                    break;
                case System.Drawing.Drawing2D.DashStyle.Dot:
                    drawing.LineType = LineTypesValues.Dot;
                    break;
                case System.Drawing.Drawing2D.DashStyle.Solid:
                    drawing.LineType = LineTypesValues.Solid;
                    break;
                case System.Drawing.Drawing2D.DashStyle.DashDot:
                    drawing.LineType = LineTypesValues.DashDot;
                    break;
            }

            Circles.Add(drawing);

        }
        public void DrawTriangle(InputFileVector drawTriangle)
        {

            var polygon = new Polygon();

            var polygonPoints = new PointCollection();

            var brush = FindBrushByARGB(drawTriangle.color);

            var xCoordinates = drawTriangle.a.Split(';');

            var yCoordinates = drawTriangle.b.Split(';');

            var zCoordinates = drawTriangle.c.Split(';');

            var point1 = new System.Windows.Point(double.Parse(xCoordinates[0]), double.Parse(xCoordinates[1]));

            var point2 = new System.Windows.Point(double.Parse(yCoordinates[0]), double.Parse(yCoordinates[1]));

            var point3 = new System.Windows.Point(double.Parse(zCoordinates[0]), double.Parse(zCoordinates[1]));

            polygonPoints.Add(point1);

            polygonPoints.Add(point2);

            polygonPoints.Add(point3);

            polygon.Points = polygonPoints;

            polygon.Stroke = brush;

            switch (Enum.Parse(typeof(System.Drawing.Drawing2D.DashStyle), FirstCharToUpper(drawTriangle.lineType)))
            {
                case System.Drawing.Drawing2D.DashStyle.Dash:
                    polygon.StrokeDashArray = new DoubleCollection() { 3, 1 };
                    break;
                case System.Drawing.Drawing2D.DashStyle.Dot:
                    polygon.StrokeDashArray = new DoubleCollection() { 1, 1 };
                    break;
                case System.Drawing.Drawing2D.DashStyle.Solid:
                    polygon.StrokeDashArray = new DoubleCollection() { 3, 0 };
                    break;
                case System.Drawing.Drawing2D.DashStyle.DashDot:
                    polygon.StrokeDashArray = new DoubleCollection() { 30, 10, 10, 10 };
                    break;
            }

            Triangles.Add(polygon);

        }
        public string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));

                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));

                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
        private Brush FindBrushByARGB(string argb)
        {
            try
            {
                var colors = argb.Split(';');

                Brush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Convert.ToByte(colors[0]),
                    Convert.ToByte(colors[1]), Convert.ToByte(colors[2]), Convert.ToByte(colors[3])));

                return brush;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ClearDraw()
        {
            Lines = new ObservableCollection<DrawLine>();
            Circles = new ObservableCollection<Circle>();
            Triangles = new ObservableCollection<Polygon>();
        }
        protected override string OnValidate(string propertyName)
        {
            return base.OnValidate(propertyName);
        }

        #endregion

    }
}
