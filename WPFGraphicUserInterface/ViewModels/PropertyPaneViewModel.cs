using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFGraphicUserInterface.Services;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class PropertyPaneViewModel :  INotifyPropertyChanged
    {
        private ObservableCollection<ComboBoxColour> _comboBoxColours;
        public ObservableCollection<ComboBoxColour> ComboBoxColours
        {
            get { return _comboBoxColours; }
            set { _comboBoxColours = value; }
        }

        public class ComboBoxColour
        {
            public string ColourName { get; set; }
            public Brush ColourBrush { get; set; }

        }

        private ComboBoxColour _selectedFill = new ComboBoxColour();
        public ComboBoxColour SelectedFill
        {
            get { return _selectedFill; }
            set
            {
                if (value == _selectedFill) return;
                _selectedFill = value;
                OnPropertyChanged(nameof(SelectedFill));
            }
        }

        private ComboBoxColour _selectedBorder = new ComboBoxColour();
        public ComboBoxColour SelectedBorder
        {
            get { return _selectedBorder; }
            set
            {
                if (value == _selectedBorder) return;
                _selectedBorder = value;
                OnPropertyChanged(nameof(SelectedBorder));
            }
        }

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

        private double _width;
        private string _title;
        private double _height;
        private Brush _fill;
        private Brush _border;
        private double _xPos;
        private double _yPos;

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
        public double Width
        {
            get { return _width; }
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }


        IEventAggregator _eventAggregator;

        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FocusedDrawingCanvasObjectChangedEvent>().Subscribe(ChangeFocusedObjectProperties);

            ComboBoxColours = new ObservableCollection<ComboBoxColour>();

            var brushNames = BrushesProvider.GetBrushNames();
            var brushes = BrushesProvider.GetBrushes();

            for (int i = 0; i < brushNames.Count ; i++)
            {
                var Colour = new ComboBoxColour { ColourName = brushNames[i], ColourBrush = brushes[i] };

                ComboBoxColours.Add(Colour);
            }
        }

        private void ChangeFocusedObjectProperties(ISelectedObject focusedItem)
        {
            var item = focusedItem;

            if (item != null)
            {
                Height = item.SelectedObjectHeight;
                XPos = item.SelectedObjectXPos;
                YPos = item.SelectedObjectYPos;
                Fill = item.SelectedObjectFill;
                Border = item.SelectedObjectBorder;
                Title = item.SelectedObjectTitle;
                Width = item.SelectedObjectWidth;

                SelectedFill.ColourBrush = Fill;
                SelectedBorder.ColourBrush = Border;
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
                FocusedCanvasDrawingObject.SelectedObjectBorder = SelectedBorder.ColourBrush;
                FocusedCanvasDrawingObject.SelectedObjectFill = SelectedFill.ColourBrush;
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