using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using WPFGraphicUserInterface.Views;

namespace WPFGraphicUserInterface.ViewModels
{
    public class RightPaneViewModel : BindableBase
    {
        //This pane should have three view models
        private SharedUsersPaneViewModel _sharedUsersPaneViewModel;

        private ProjectPaneViewModel _projectPaneViewModel;

        private PropertyPaneViewModel _propertyPaneViewModel;

        private string _currentPaneName;

        private FrameworkElement _currentPane;

        private int _selectedIndex;

        public class RightPaneOption
        {
            public string OptionName { get; set; }
        }

        public bool _isRightPaneOptionsPopUpOpen;

        public ObservableCollection<RightPaneOption> RightPaneOptions { get; set; }

        public bool IsRightPaneOptionsPopUpOpen
        {
            get { return _isRightPaneOptionsPopUpOpen; }
            set
            {
                SetProperty(ref _isRightPaneOptionsPopUpOpen, value);
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                SetProperty(ref _selectedIndex, value);
                SelectPane();
            }
        }

        public FrameworkElement CurrentPane
        {
            get { return _currentPane; }
            set
            {
                SetProperty(ref _currentPane, value);
            }
        }

        public string CurrentPaneName
        {
            get { return _currentPaneName; }
            set
            {
                SetProperty(ref _currentPaneName, value);
            }
        }

        public DelegateCommand ShowRightPaneOptionsPopUpCommand { get; set; }

        IEventAggregator _eventAggregator;

        public RightPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            ShowRightPaneOptionsPopUpCommand = new DelegateCommand(ShowRightPaneOptionsPopUp, CanShowRightPaneOptionsPopUp);
            RightPaneOptions = new ObservableCollection<RightPaneOption>();
            RightPaneOptions.Add(new RightPaneOption() { OptionName = "Shared Users Pane" });
            RightPaneOptions.Add(new RightPaneOption() { OptionName = "Property Pane" });
            RightPaneOptions.Add(new RightPaneOption() { OptionName = "Project Pane" });
            CurrentPaneName = RightPaneOptions[0].OptionName;
            CurrentPane = new SharedUsersPaneView();
            SelectedIndex = 0;
        }

        private void SelectPane()
        {
            switch (_selectedIndex)
            {
                case 0:
                    //Change the marker on others
                    CurrentPane = new SharedUsersPaneView();
                    _sharedUsersPaneViewModel = new SharedUsersPaneViewModel(_eventAggregator);
                    CurrentPane.DataContext = _sharedUsersPaneViewModel;
                    CurrentPaneName = RightPaneOptions[0].OptionName;
                    break;
                case 1:
                    CurrentPane = new PropertyPaneView();
                    _propertyPaneViewModel = new PropertyPaneViewModel(_eventAggregator);
                    CurrentPane.DataContext = _propertyPaneViewModel;
                    CurrentPaneName = RightPaneOptions[1].OptionName;
                    break;
                case 2:
                    CurrentPane = new ProjectPaneView();
                    _projectPaneViewModel = new ProjectPaneViewModel(_eventAggregator);
                    CurrentPane.DataContext = _projectPaneViewModel;
                    CurrentPaneName = RightPaneOptions[2].OptionName;
                    break;
                default:
                    //CurrentPane.Visibility = Visibility.Visible;
                    break;
            }
        }

        //Show right pane options 
        private bool CanShowRightPaneOptionsPopUp()
        {
            return true;
        }

        private void ShowRightPaneOptionsPopUp()
        {
            IsRightPaneOptionsPopUpOpen = !IsRightPaneOptionsPopUpOpen;
        }
    }
}
