using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class PropertyPaneViewModel : BindableBase
    {
        //private FrameworkElement _focusedCanvasDrawingObject = new FrameworkElement();

        //public FrameworkElement FocusedCanvasDrawingObject
        //{
        //    get { return _focusedCanvasDrawingObject; }
        //    set
        //    {
        //        SetProperty(ref _focusedCanvasDrawingObject, value);
        //    }
        //}
        
        private double _fontSize;
        private string _title;
        private double _width;
        private double _height;
        private Brush _fill;
        private Brush _border;
        private double _xPos;
        private double _yPos;

        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                SetProperty(ref _fontSize, value);
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
            }
        }
        public double Width
        {
            get { return _width; }
            set
            {
                SetProperty(ref _width, value);
            }
        }
        public double Height
        {
            get { return _height; }
            set
            {
                SetProperty(ref _height, value);
            }
        }
        public Brush Fill
        {
            get { return _fill; }
            set
            {
                SetProperty(ref _fill, value);
            }
        }
        public Brush Border
        {
            get { return _border; }
            set
            {
                SetProperty(ref _border, value);
            }
        }
        public double XPos
        {
            get { return _xPos; }
            set
            {
                SetProperty(ref _xPos, value);
            }
        }
        public double YPos
        {
            get { return _yPos; }
            set
            {
                SetProperty(ref _yPos, value);
            }
        }


        IEventAggregator _eventAggregator;

        public PropertyPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Subscribe(ChangeFocusedObjectProperties);
        }

        private void ChangeFocusedObjectProperties(ISelectedObject focusedItem)
        {
            var item = focusedItem;

            if (item != null)
            {
                Height = item.SelectedObjectHeight;
                Width = item.SelectedObjectWidth;
                XPos = item.SelectedObjectXPos;
                YPos = item.SelectedObjectYPos;
            }
        }
    }
}