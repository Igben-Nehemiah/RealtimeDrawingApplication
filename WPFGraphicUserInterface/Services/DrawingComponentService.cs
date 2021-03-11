using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFGraphicUserInterface.ViewModels;

namespace WPFGraphicUserInterface.Services
{
    public static class DrawingComponentService
    {
        public static FrameworkElement GetDefaultComponent(ControlEnum component)
        {
            if (component == ControlEnum.TextBox)
            {
                return new TextBoxComponent();
            }
            else
            {
                return new ShapeComponent(GetDefaultShapeGeometry(component), component);
            }
        }

        public static Geometry GetDefaultShapeGeometry(ControlEnum component)
        {
            switch (component)
            {
                case ControlEnum.Ellipse:
                    return new EllipseGeometry(new Point(25, 25), 25, 25);
                case ControlEnum.Rectangle:
                    return Geometry.Parse("M0,0 L50,0 L50,50 L0,50Z");
                case ControlEnum.Triangle:
                    return Geometry.Parse("M25,0 L50,50 L0,50Z");
                case ControlEnum.Line:
                    return Geometry.Parse("M0,0 L50,50");
                default:
                    return Geometry.Parse("M25,0 L50,50 L0,50Z");
            }
        }

    }
    public enum ControlEnum
    {
        Ellipse,
        Rectangle,
        Path,
        Triangle,
        Line,
        TextBox,
    }

    public class ShapeComponent : Shape, ISelectedObject
    {
        public ShapeComponent(Geometry geometry, ControlEnum shapeType)
        {
            Geometry = geometry;
            SelectedObjectFill = Brushes.Black;
            SelectedObjectBorder = Brushes.Black;
            SelectedObjectWidth = 50;
            SelectedObjectHeight = 50;
            SelectedObjectTitle = shapeType.ToString();


            ControlType = shapeType;

            Stretch = Stretch.Fill;

            Fill = SelectedObjectFill;
            Stroke = SelectedObjectBorder;
            StrokeThickness = 3;
            Width = SelectedObjectWidth;
            Height = SelectedObjectHeight;
        }

        public string SelectedObjectTitle { get; set; }
        public double SelectedObjectWidth { get; set; }
        public double SelectedObjectHeight { get; set; }
        public Brush SelectedObjectFill { get; set; }
        public Brush SelectedObjectBorder { get; set; }
        public double SelectedObjectXPos { get; set; }
        public double SelectedObjectYPos { get; set; }
        public Geometry Geometry { get; set; }
        public Guid SelectedObjectId { get; set; } = Guid.NewGuid();
        public ControlEnum ControlType { get; set; }

        protected override Geometry DefiningGeometry => Geometry;

    }

    public class TextBoxComponent : TextBox, ISelectedObject
    {
        public TextBoxComponent(double width = 60, double height = 20, string text = "Text")
        {
            SelectedObjectWidth = width;
            SelectedObjectHeight = height;
            SelectedObjectTitle = ControlEnum.TextBox.ToString();
            Text = text;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            Background = SelectedObjectFill;
            //Border = SelectedObjectBorder;
            Width = SelectedObjectWidth;
            Height = SelectedObjectHeight;
            ControlType = ControlEnum.TextBox;
        }

        public string SelectedObjectTitle { get; set; }
        public double SelectedObjectWidth { get; set; }
        public double SelectedObjectHeight { get; set; }
        public Brush SelectedObjectFill { get; set; } = Brushes.White;
        public Brush SelectedObjectBorder { get; set; } = Brushes.Black;
        public double SelectedObjectXPos { get; set; }
        public double SelectedObjectYPos { get; set; }
        public Guid SelectedObjectId { get; set; } = Guid.NewGuid();
        public ControlEnum ControlType { get; set; }
    }
}
