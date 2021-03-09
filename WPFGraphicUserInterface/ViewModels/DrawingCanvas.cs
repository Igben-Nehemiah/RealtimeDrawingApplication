using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFGraphicUserInterface.ModelProxies;
using Prism.Ioc;
using WPFUserInterface.Core;
using System.Windows.Shapes;
using WPFGraphicUserInterface.Views;
using System;
using WPFGraphicUserInterface.Services;

namespace WPFGraphicUserInterface.ViewModels
{
    public class DrawingCanvas : Canvas//, INotifyPropertyChanged
    {
        //private FrameworkElement _focusedDrawingElement;
        //This is the element having the focus on the canvas
        public FrameworkElement FocusedDrawingElement { get; set; }

        //This is the element that is published to the property pane...
        public ISelectedObject FocusedCanvasItem { get; set; }

        //public event PropertyChangedEventHandler PropertyChanged;
        private double xPos;
        private double yPos;

        public IEventAggregator EventAggregator { get; set; }

        public DrawingCanvas()// : base()
        {
            Background = Brushes.White;
            AllowDrop = true;
             
            EventAggregator = App.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<PropertyPaneChangedEvent>().Subscribe(ChangeFocusedObjectProperties);
        }

        private void ChangeFocusedObjectProperties(ISelectedObject obj)
        {
            //FocusedDrawingElement = null;

            if (obj != null)
            {
                foreach (FrameworkElement item in Children)
                {
                    var _item = (ISelectedObject)item;

                    if (_item != null && _item.SelectedObjectId == obj.SelectedObjectId)
                    {
                        FocusedDrawingElement = _item as FrameworkElement;
                        FocusedDrawingElement.HorizontalAlignment = HorizontalAlignment.Stretch;
                        FocusedDrawingElement.VerticalAlignment = VerticalAlignment.Stretch;
                        
                        xPos = obj.SelectedObjectXPos;
                        yPos = obj.SelectedObjectYPos;

                        if (_item is ShapeComponent sh)
                        {
                            sh.Fill = _item.SelectedObjectFill;
                            sh.Height = _item.SelectedObjectHeight;
                            sh.Width = _item.SelectedObjectWidth;
                            sh.Stroke = _item.SelectedObjectBorder;
                            sh.StrokeThickness = 3;
                            sh.SelectedObjectXPos = _item.SelectedObjectXPos;
                            sh.SelectedObjectYPos = _item.SelectedObjectYPos;

                            //sh.Stretch = Stretch.Fill;

                            FocusedCanvasItem = sh;
                        }

                        else if (_item is TextBoxComponent tx)
                        {
                            //tx.Fill = _item.SelectedObjectFill;
                            tx.Height = _item.SelectedObjectHeight;
                            tx.Width = _item.SelectedObjectWidth;
                            //tx.Stroke = _item.SelectedObjectBorder;
                            //tx.StrokeThickness = 3;
                            tx.SelectedObjectXPos = _item.SelectedObjectXPos;
                            tx.SelectedObjectYPos = _item.SelectedObjectYPos;

                            //tx.Stretch = Stretch.Fill;

                            FocusedCanvasItem = tx;
                        }

                        FocusedDrawingElement = FocusedCanvasItem as FrameworkElement;
                        FocusedDrawingElement.Height = _item.SelectedObjectHeight;
                        FocusedDrawingElement.Width = _item.SelectedObjectWidth;
                        FocusedDrawingElement.HorizontalAlignment = HorizontalAlignment.Stretch;
                        FocusedDrawingElement.VerticalAlignment = VerticalAlignment.Stretch;

                        return;

                        //var index = Children.IndexOf(item);
                        //Children[index] = FocusedDrawingElement;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (FocusedDrawingElement != null)
                {
                    var position = e.GetPosition(this);

                    if (position.X < ActualWidth - FocusedDrawingElement.ActualWidth &&
                        position.Y < ActualHeight - FocusedDrawingElement.ActualHeight)
                    {
                        var model = FocusedDrawingElement as ISelectedObject;
                        if (model != null)
                        {
                            model.SelectedObjectXPos = position.X;
                            model.SelectedObjectYPos = position.Y;
                            xPos = position.X;
                            yPos = position.Y;

                            FocusedDrawingElement = model as FrameworkElement;
                            SetLeft(FocusedDrawingElement, position.X);
                            SetTop(FocusedDrawingElement, position.Y);
                            EventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Publish(model);
                        }
                    }
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            FocusedDrawingElement = null;
            //FocusedCanvasItem = null;
            //FocusedDrawingElement = e.Source as FrameworkElement;
            var item = e.Source as FrameworkElement;

            FocusedDrawingElement = item;

            var _item = item as ISelectedObject;

            if (item is ISelectedObject)
            {
                if (FocusedDrawingElement is ShapeComponent sh)
                {
                    sh.Fill = _item.SelectedObjectFill;
                    sh.Height = _item.SelectedObjectHeight;
                    sh.Width = _item.SelectedObjectWidth;
                    sh.Stroke = _item.SelectedObjectBorder;
                    sh.StrokeThickness = 3;
                    sh.SelectedObjectXPos = _item.SelectedObjectXPos;
                    sh.SelectedObjectYPos = _item.SelectedObjectYPos;
                    sh.SelectedObjectTitle = _item.SelectedObjectTitle;
                    FocusedCanvasItem = sh;
                }
                else if (item is TextBoxComponent tx)
                {
                    FocusedCanvasItem = tx;
                }
            }
            EventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Publish(FocusedCanvasItem);

            base.OnMouseLeftButtonDown(e);
        }

        private void SelectedComponent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FocusedCanvasItem = null;
            FocusedDrawingElement = null;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            FocusedDrawingElement = null;
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            FocusedDrawingElement = null;
            FocusedDrawingElement = e.Data.GetData("toolboxitem") as FrameworkElement;
            var dropPosition = e.GetPosition(this);
            xPos = dropPosition.X;
            yPos = dropPosition.Y;

            SetItemOnCanvas(FocusedDrawingElement, xPos, yPos);
            Children.Add(FocusedDrawingElement);

            //This part is responsible for publishing a model that is used by the property pane window
            if (FocusedDrawingElement is ISelectedObject selectedObject)
            {
                if (selectedObject is ShapeComponent shapeObj)
                {
                    FocusedCanvasItem = shapeObj;
                }
                else if (selectedObject is TextBoxComponent textboxObj)
                {
                    FocusedCanvasItem = textboxObj;
                }

                //Set the item x and y coordinate on canvas since these options are not set by default
                FocusedCanvasItem.SelectedObjectXPos = xPos;
                FocusedCanvasItem.SelectedObjectYPos = yPos;
                EventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Publish(FocusedCanvasItem);
            }
        }

        public void SetItemOnCanvas(UIElement element, double xPos, double yPos)
        {
            if (element != null)
            {
                SetLeft(element, xPos);
                SetTop(element, yPos);
            }
        }
    }
}
