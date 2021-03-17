using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using WPFGraphicUserInterface.Views;
using Prism.Ioc;

namespace WPFGraphicUserInterface.ViewModels
{
    public class RightPaneViewModel : BindableBase
    {
        public SharedUsersPaneView SharedUsersPaneView = new SharedUsersPaneView();
        public ProjectPaneView ProjectPaneView = new ProjectPaneView();
        public PropertyPaneView PropertyPaneView = new PropertyPaneView();
        public SharedUsersPaneViewModel SharedUsersPaneViewModel;
        public ProjectPaneViewModel ProjectPaneViewModel;
        public PropertyPaneViewModel PropertyPaneViewModel;
        private string _currentPaneName;
        private FrameworkElement _currentPane;
        private int _selectedIndex;

        //Helper class
        public class RightPaneOption
        {
            public string OptionName { get; set; }
        }

        private bool _isRightPaneOptionsPopUpOpen;
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

        public RightPaneViewModel()
        {
            _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();

            ShowRightPaneOptionsPopUpCommand = new DelegateCommand(ShowRightPaneOptionsPopUp, ()=>true);

            SharedUsersPaneView = new SharedUsersPaneView();
            SharedUsersPaneViewModel = new SharedUsersPaneViewModel(_eventAggregator);
            SharedUsersPaneView.DataContext = SharedUsersPaneViewModel;

            PropertyPaneView = new PropertyPaneView();
            PropertyPaneViewModel = new PropertyPaneViewModel(_eventAggregator);
            PropertyPaneView.DataContext = PropertyPaneViewModel;

            ProjectPaneView = new ProjectPaneView();
            ProjectPaneViewModel = new ProjectPaneViewModel(_eventAggregator);
            ProjectPaneView.DataContext = ProjectPaneViewModel;

            RightPaneOptions = new ObservableCollection<RightPaneOption>();
            RightPaneOptions.Add(new RightPaneOption() { OptionName = "Shared Users Pane" });
            RightPaneOptions.Add(new RightPaneOption() { OptionName = "Property Pane" });
            RightPaneOptions.Add(new RightPaneOption() { OptionName = "Project Pane" });
            SelectedIndex = 1;
        }

        private void SelectPane()
        {
            switch (_selectedIndex)
            {
                case 0:
                    //Change the marker on others
                    CurrentPane = SharedUsersPaneView;
                    //_sharedUsersPaneViewModel = new SharedUsersPaneViewModel(_eventAggregator);
                    //CurrentPane.DataContext = _sharedUsersPaneViewModel;
                    CurrentPaneName = RightPaneOptions[0].OptionName;
                    break;
                case 1:
                    CurrentPane = PropertyPaneView;
                    //_propertyPaneViewModel = new PropertyPaneViewModel(_eventAggregator);
                    //CurrentPane.DataContext = _propertyPaneViewModel;
                    CurrentPaneName = RightPaneOptions[1].OptionName;
                    break;
                case 2:
                    CurrentPane = ProjectPaneView;
                    //_projectPaneViewModel = new ProjectPaneViewModel(_eventAggregator);
                    //CurrentPane.DataContext = _projectPaneViewModel;
                    CurrentPaneName = RightPaneOptions[2].OptionName;
                    break;
                default:
                    CurrentPane.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ShowRightPaneOptionsPopUp()
        {
            IsRightPaneOptionsPopUpOpen = !IsRightPaneOptionsPopUpOpen;
        }
    }
}
