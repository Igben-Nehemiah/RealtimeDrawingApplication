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

namespace WPFGraphicUserInterface.ViewModels
{
    public class DrawingCanvas : Canvas, INotifyPropertyChanged
    {
        private FrameworkElement _focusedDrawingElement;

        public FrameworkElement FocusedDrawingElement { get; set; }

        //public ObservableCollection<DrawingCanvasObjectProxy> DrawingCanvasObjects;

        //public FrameworkElement FocusedDrawingElement
        //{
        //    get { return _focusedDrawingElement; }
        //    set
        //    {
        //        if (value == _focusedDrawingElement) return;
        //        _focusedDrawingElement = value;
        //        OnPropertyChanged(nameof(FocusedDrawingElement));
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
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
            //_mouseButtonEventArgs = null;
            if (obj != null)
            {
                //FrameworkElement component = null;

                foreach(UIElement item in Children)
                {
                    var _item = (ISelectedObject)item;

                    if (_item.SelectedObjectId == obj.SelectedObjectId)
                    {
                        _item.SelectedObjectXPos = obj.SelectedObjectXPos;
                        _item.SelectedObjectYPos = obj.SelectedObjectYPos;
                        _item.SelectedObjectHeight = obj.SelectedObjectHeight;
                        _item.SelectedObjectFill = obj.SelectedObjectFill;
                        _item.SelectedObjectBorder = obj.SelectedObjectBorder;
                        _item.SelectedObjectTitle = obj.SelectedObjectTitle;
                        _item.SelectedObjectWidth = obj.SelectedObjectWidth;

                        FocusedDrawingElement = _item as FrameworkElement;
                        xPos = obj.SelectedObjectXPos;
                        yPos = obj.SelectedObjectYPos;
                        SetLeft(FocusedDrawingElement, xPos);
                        SetTop(FocusedDrawingElement, yPos);
                    }
                }
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //if (_mouseButtonEventArgs != null)
            //{
                //if (_mouseButtonEventArgs.XButton1 == MouseButtonState.Released)
                //{
            if (FocusedDrawingElement != null)
            {
                var position = e.GetPosition(this);
                if (position.X < ActualWidth - FocusedDrawingElement.ActualWidth &&
                    position.Y < ActualHeight - FocusedDrawingElement.ActualHeight)
                {
                    var model = FocusedDrawingElement as ISelectedObject;
                    model.SelectedObjectXPos = position.X;
                    model.SelectedObjectYPos = position.Y;
                    xPos = position.X;
                    yPos = position.Y;
                    SetLeft(FocusedDrawingElement, position.X);
                    SetTop(FocusedDrawingElement, position.Y);
                    EventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Publish(model);
                }
            }
        }

        private MouseButtonEventArgs _mouseButtonEventArgs;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //FocusedDrawingElement = null;
            //_mouseButtonEventArgs = e;
            FocusedDrawingElement = e.Source as FrameworkElement;
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
            var item = e.Data.GetData("toolboxitem") as FrameworkElement;
            var currentMousePosition = e.GetPosition(this);
            xPos = currentMousePosition.X;
            yPos = currentMousePosition.Y;
            
            if (item is ISelectedObject)
            {
                if (item is ShapeComponent sh)
                {
                    FocusedCanvasItem = sh;
                }
                else if (item is TextBoxComponent tx)
                {
                    FocusedCanvasItem = tx;
                }

                FocusedCanvasItem.SelectedObjectXPos = xPos;
                FocusedCanvasItem.SelectedObjectYPos = yPos;
                EventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Publish(FocusedCanvasItem);
                
                //The drawing canvas can accept UIElement and its derivatives
                FocusedDrawingElement = FocusedCanvasItem as FrameworkElement;

                //SetLeft(FocusedDrawingElement, xPos);
                //SetTop(FocusedDrawingElement, yPos);
                SetItemOnCanvas(FocusedDrawingElement, xPos, yPos);
                Children.Add(FocusedDrawingElement);
            }
        }

        public ISelectedObject FocusedCanvasItem;

        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetItemOnCanvas(UIElement element, double xPos, double yPos)
        {
            SetLeft(element, xPos);
            SetTop(element, yPos);
        }
    }
}
