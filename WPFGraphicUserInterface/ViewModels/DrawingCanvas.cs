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
        
        public ObservableCollection<DrawingCanvasObjectProxy> DrawingCanvasObjects;

        public FrameworkElement FocusedDrawingElement
        {
            get { return _focusedDrawingElement; }
            set
            {
                if (value == _focusedDrawingElement) return;
                _focusedDrawingElement = value;
                OnPropertyChanged(nameof(FocusedDrawingElement));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEventAggregator EventAggregator { get; set; }

        public DrawingCanvas() 
        {
            Background = Brushes.White;
            DrawingCanvasObjects = new ObservableCollection<DrawingCanvasObjectProxy>();
            AllowDrop = true;

            EventAggregator = App.ShellContainer.Resolve<IEventAggregator>();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (FocusedDrawingElement != null)
            {
                var position = e.GetPosition(this);
                if (position.X < ActualWidth - FocusedDrawingElement.ActualWidth &&
                    position.Y < ActualHeight - FocusedDrawingElement.ActualHeight)
                {
                    var model = FocusedDrawingElement as ISelectedObject;
                    model.SelectedObjectXPos = position.X;
                    model.SelectedObjectYPos = position.Y;
                    SetLeft(FocusedDrawingElement, position.X);
                    SetTop(FocusedDrawingElement, position.Y);
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
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
            var item = e.Data.GetData("toolboxitem") as FrameworkElement;
            var currentMousePosition = e.GetPosition(this);
            var xValue = currentMousePosition.X;
            var yValue = currentMousePosition.Y;

            Canvas.SetLeft(item, xValue);
            Canvas.SetTop(item, yValue);
            FocusedDrawingElement = item;
            Children.Add(item);

            if (item is ISelectedObject component)
            {
                if (item is ShapeComponent sh)
                {
                    FocusedCanvasItem = sh;//new ShapeComponent(sh.Geometry);
                }
                else if (item is TextBoxComponent tx)
                {
                    FocusedCanvasItem = tx;
                }
                
                EventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Publish(FocusedCanvasItem);
            }
        }

        public ISelectedObject FocusedCanvasItem;

        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
