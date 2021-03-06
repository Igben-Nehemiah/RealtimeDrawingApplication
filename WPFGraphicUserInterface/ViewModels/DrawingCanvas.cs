﻿using Prism.Events;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Ioc;
using WPFUserInterface.Core;
using WPFGraphicUserInterface.Services;

namespace WPFGraphicUserInterface.ViewModels
{
    public class DrawingCanvas : Canvas
    {
        public FrameworkElement FocusedDrawingElement { get; set; }
        //This is the element that is published to the property pane...
        public ISelectedObject FocusedCanvasItem { get; set; }

        private double xPos;
        private double yPos;

        public IEventAggregator EventAggregator { get; set; }

        public DrawingCanvas()
        {
            Background = Brushes.LightGray;
            AllowDrop = true;
             
            EventAggregator = App.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<PropertyPaneChangedEvent>().Subscribe(ChangeFocusedObjectProperties);
        }

        private void ChangeFocusedObjectProperties(ISelectedObject obj)
        {
            //FocusedDrawingElement = null;

            if (obj != null)
            {
                FrameworkElement _component = null;

                foreach (FrameworkElement item in Children)
                {
                    var _item = (ISelectedObject)item;

                    if (_item != null && _item.SelectedObjectId == obj.SelectedObjectId)
                    {
                        _component = _item as FrameworkElement;
                        
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

                            FocusedCanvasItem = sh;
                        }

                        else if (_item is TextBoxComponent tx)
                        {
                            tx.Height = _item.SelectedObjectHeight;
                            tx.Width = _item.SelectedObjectWidth;
                            tx.SelectedObjectXPos = _item.SelectedObjectXPos;
                            tx.SelectedObjectYPos = _item.SelectedObjectYPos;

                            FocusedCanvasItem = tx;
                        }

                        FocusedDrawingElement = FocusedCanvasItem as FrameworkElement;
                        FocusedDrawingElement.Height = _item.SelectedObjectHeight;
                        FocusedDrawingElement.Width = _item.SelectedObjectWidth;
                        
                        SetLeft(FocusedDrawingElement, xPos);
                        SetTop(FocusedDrawingElement, yPos);

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
            EventAggregator.GetEvent<CloseMenuBtnClickEvent>().Publish();

            FocusedDrawingElement = null;
           
            var item = e.OriginalSource;

            if (item is ShapeComponent sh)
            {
                FocusedCanvasItem = sh;
            }
            else if (item is TextBoxComponent tx)
            {
                FocusedCanvasItem = tx;
            }

            FocusedDrawingElement = FocusedCanvasItem as FrameworkElement;

            if (FocusedDrawingElement == null)
            {
                return;
            }

            EventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Publish(FocusedCanvasItem);
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            FocusedDrawingElement = null;
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            FocusedDrawingElement = null;

            var droppedItem = e.Data.GetData("toolboxitem") as FrameworkElement;

            var dropPosition = e.GetPosition(this);
            
            SetItemOnCanvas(droppedItem, dropPosition.X, dropPosition.Y);

            FocusedDrawingElement = droppedItem;

            //This part is responsible for publishing a model that is used by the property pane window
            if (droppedItem is ISelectedObject selectedObject)
            {
                if (selectedObject is ShapeComponent shapeObj)
                {
                    FocusedCanvasItem = shapeObj;
                }
                else if (selectedObject is TextBoxComponent textboxObj)
                {
                    FocusedCanvasItem = textboxObj;
                }

                FocusedCanvasItem.SelectedObjectXPos = dropPosition.X;
                FocusedCanvasItem.SelectedObjectYPos = dropPosition.Y;

                var newChild = FocusedCanvasItem as FrameworkElement;
                Children.Add(newChild);

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
