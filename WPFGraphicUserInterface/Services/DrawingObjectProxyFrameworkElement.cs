using System;
using System.Windows;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.ViewModels;

namespace WPFGraphicUserInterface.Services
{
    public static class DrawingObjectProxyFrameworkElement
    {
        public static FrameworkElement ConvertToFrameworkElement(DrawingCanvasObjectProxy drawingCanvasObjectProxy)
        {
            FrameworkElement obj;

            if (drawingCanvasObjectProxy.CanvasObjectName == ControlEnum.Rectangle.ToString())
            {
                obj = DrawingComponentService.GetDefaultComponent(ControlEnum.Rectangle);
            }
            else if (drawingCanvasObjectProxy.CanvasObjectName == ControlEnum.Ellipse.ToString())
            {
                obj = DrawingComponentService.GetDefaultComponent(ControlEnum.Ellipse);
            }
            else if (drawingCanvasObjectProxy.CanvasObjectName == ControlEnum.Line.ToString())
            {
                obj = DrawingComponentService.GetDefaultComponent(ControlEnum.Line);
            }
            else if (drawingCanvasObjectProxy.CanvasObjectName == ControlEnum.Triangle.ToString())
            {
                obj = DrawingComponentService.GetDefaultComponent(ControlEnum.Triangle);
            }
            else
            {
                obj = DrawingComponentService.GetDefaultComponent(ControlEnum.TextBox);
            }

            var item = obj as ISelectedObject;

            if (item is ShapeComponent sh)
            {
                sh.SelectedObjectBorder = BrushConverterHelper.ConvertToBrush(drawingCanvasObjectProxy.BorderFill);
                sh.SelectedObjectFill = BrushConverterHelper.ConvertToBrush(drawingCanvasObjectProxy.ShapeFill);
                sh.SelectedObjectHeight = drawingCanvasObjectProxy.Height;
                sh.SelectedObjectWidth = drawingCanvasObjectProxy.Width;
                sh.SelectedObjectXPos = drawingCanvasObjectProxy.XPosition;
                sh.SelectedObjectYPos = drawingCanvasObjectProxy.YPosition;
                sh.SelectedObjectId = Guid.Parse(drawingCanvasObjectProxy.CanvasObjectGuid);

                sh.Height = item.SelectedObjectHeight;
                sh.Width = item.SelectedObjectWidth;
                sh.Fill = item.SelectedObjectFill;
                sh.StrokeThickness = 3;

                return sh;
            }

            else if (item is TextBoxComponent tx)
            {
                tx.SelectedObjectBorder = BrushConverterHelper.ConvertToBrush(drawingCanvasObjectProxy.BorderFill);
                tx.SelectedObjectFill = BrushConverterHelper.ConvertToBrush(drawingCanvasObjectProxy.ShapeFill);
                tx.SelectedObjectHeight = drawingCanvasObjectProxy.Height;
                tx.SelectedObjectWidth = drawingCanvasObjectProxy.Width;
                tx.SelectedObjectXPos = drawingCanvasObjectProxy.XPosition;
                tx.SelectedObjectYPos = drawingCanvasObjectProxy.YPosition;
                tx.SelectedObjectId = Guid.Parse(drawingCanvasObjectProxy.CanvasObjectGuid);

                tx.Height = item.SelectedObjectHeight;
                tx.Width = item.SelectedObjectWidth;
                //tx.Fill = item.SelectedObjectFill;

                return tx;
            }

            return null;
        }
    }
}
