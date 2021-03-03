using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class PropertyPaneViewModel : INotifyPropertyChanged
    {
        private ISelectedObject _focusedCanvasDrawingObject;
        public ISelectedObject FocusedCanvasDrawingObject
        {
            get { return _focusedCanvasDrawingObject; }
            set
            {
                if (value == _focusedCanvasDrawingObject) return;
                _focusedCanvasDrawingObject = value;
                OnPropertyChanged(nameof(FocusedCanvasDrawingObject));
            }
        }

        private string _title;
        private double _width;
        private double _height;
        private Brush _fill;
        private Brush _border;
        private double _xPos;
        private double _yPos;

        //public Guid Height
        //{
        //    get { return _height; }
        //    set
        //    {
        //        if (value == _height) return;
        //        _height = value;
        //        OnPropertyChanged(nameof(Height));
        //    }
        //}
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public double Width
        {
            get { return _width; }
            set
            {
                if (value == _width) return;
                _yPos = value;
                OnPropertyChanged(nameof(Width));
            }
        }
        public double Height
        {
            get { return _height; }
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
        public Brush Fill
        {
            get { return _fill; }
            set
            {
                if (value == _fill) return;
                _fill = value;
                OnPropertyChanged(nameof(Fill));
            }
        }
        public Brush Border
        {
            get { return _border; }
            set
            {
                if (value == _border) return;
                _border = value;
                OnPropertyChanged(nameof(Border));
            }
        }
        public double XPos
        {
            get { return _xPos; }
            set
            {
                if (value == _xPos) return;
                _xPos = value;
                OnPropertyChanged(nameof(XPos));
            }
        }
        public double YPos
        {
            get { return _yPos; }
            set
            {
                if (value == _yPos) return;
                _yPos = value;
                OnPropertyChanged(nameof(YPos));
            }
        }

        IEventAggregator _eventAggregator;

        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Subscribe(ChangeFocusedObjectProperties);
            //_eventAggregator.GetEvent<PropertyPaneChangedEvent>().Publish()
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
                FocusedCanvasDrawingObject = item;
            }
        }

        private void SetFocusedObjProperties()
        {
            if (FocusedCanvasDrawingObject != null)
            {
                FocusedCanvasDrawingObject.SelectedObjectHeight = Height;
                FocusedCanvasDrawingObject.SelectedObjectWidth = Width;
                FocusedCanvasDrawingObject.SelectedObjectXPos = XPos;
                FocusedCanvasDrawingObject.SelectedObjectYPos = YPos;
                FocusedCanvasDrawingObject.SelectedObjectBorder = Border;
                FocusedCanvasDrawingObject.SelectedObjectFill = Fill;
                FocusedCanvasDrawingObject.SelectedObjectTitle = Title;

            }
        }

        protected void OnPropertyChanged(string propertyName = "")
        {

            SetFocusedObjProperties();
            _eventAggregator.GetEvent<PropertyPaneChangedEvent>().Publish(FocusedCanvasDrawingObject);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }
    }
}