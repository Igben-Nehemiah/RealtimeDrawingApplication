using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFGraphicUserInterface.ViewModels;

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for StartUpWindowView.xaml
    /// </summary>
    /// 
    public enum ControlEnum
    {
        Ellipse,
        Rectangle,
        Circle,
        Path,
        Triangle,
        Line,
        TextBox,
    }
    public partial class StartUpWindowView : Window
    {
        public LoginWindowView loginViewWindow { get; set; }
        public StartUpWindowView()
        {
            InitializeComponent();
        }

        private void DrawingObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement component = null;

            if (sender is Rectangle rectangle)
            {
                component = DrawingComponentService.GetDefaultComponent(ControlEnum.Rectangle);
            }
            else if (sender is Ellipse ellipse)
            {
                component = DrawingComponentService.GetDefaultComponent(ControlEnum.Ellipse);
            }
            else if (sender is Path path)
            {
                component = DrawingComponentService.GetDefaultComponent(ControlEnum.Path);
            }
            else if (sender is TextBlock textBlock)
            {
                component = DrawingComponentService.GetDefaultComponent(ControlEnum.TextBox);
            }

            if (component != null)
            {
                DataObject dataObject = new DataObject("toolboxitem", component);
                DragDrop.DoDragDrop(component as FrameworkElement, dataObject, DragDropEffects.Copy);
            }
        }
    }

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
                return new ShapeComponent(GetDefaultShapeGeometry(component));
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

    public class ShapeComponent : Shape, ISelectedObject
    {
        public ShapeComponent(Geometry geometry)
        {
            Geometry = geometry;
            Fill = Brushes.Black;
            Width = 50;
            Height = 50;
        }

        public ControlEnum ControlType { get; set; }
        public int SelectedObjectFontsize { get; set; }
        public string SelectedObjectTitle { get; set; }
        public double SelectedObjectWidth { get; set; } = 50;
        public double SelectedObjectHeight { get; set; } = 50;
        public Brush SelectedObjectFill { get; set; } = Brushes.Black;
        public Brush SelectedObjectBorder { get; set; }
        public double SelectedObjectXPos { get; set; }
        public double SelectedObjectYPos { get; set; }
        public Geometry Geometry { get; set; }
        public Guid SelectedObjectId { get; set; } = Guid.NewGuid();

        protected override Geometry DefiningGeometry => Geometry;
    }

    public class TextBoxComponent : TextBox, ISelectedObject
    {
        public TextBoxComponent(double width = 60, double height = 20, string text = "Text")
        {
            Width = width;
            Height = height;
            Text = text;
        }

        public ControlEnum ControlType { get; set; }
        public int SelectedObjectFontsize { get; set; }
        public string SelectedObjectTitle { get; set; }
        public double SelectedObjectWidth { get; set; }
        public double SelectedObjectHeight { get; set; }
        public Brush SelectedObjectFill { get; set; }
        public Brush SelectedObjectBorder { get; set; }
        public double SelectedObjectXPos { get; set; }
        public double SelectedObjectYPos { get; set; }
        public Guid SelectedObjectId { get; set; } = Guid.NewGuid();
    }
}
