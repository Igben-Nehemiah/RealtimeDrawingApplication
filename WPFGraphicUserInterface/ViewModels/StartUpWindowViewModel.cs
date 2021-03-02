using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public interface IDrawingCanvasItem { }
    public interface ISelectedObject
    {
        ControlEnum ControlType { get; set; }
        int SelectedObjectFontsize { get; set; }
        string SelectedObjectTitle { get; set; }
        double SelectedObjectWidth { get; set; }
        double SelectedObjectHeight { get; set; }
        Brush SelectedObjectFill { get; set; }
        Brush SelectedObjectBorder { get; set; }
        double SelectedObjectXPos { get; set; }
        double SelectedObjectYPos { get; set; }
    }

    public class StartUpWindowViewModel : BindableBase
    {
        private RightPaneViewModel _rightPaneViewModel;
        private FrameworkElement _rightPaneContentControl;
        private UserProxy _user;
        private string _userName;
        private ProjectProxy _activeProject;
        private string _statusBarMessage;
        private MenuPaneViewModel _menuPaneViewModel;
        private FrameworkElement _menuContentControl;

        //A store for the shared projects
        Dictionary<string, List<string>> shared = new Dictionary<string, List<string>>();

        public UserProxy User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
            }
        }
        public string StatusBarMessage
        {
            get { return _statusBarMessage; }
            set
            {
                SetProperty(ref _statusBarMessage, value);
            }
        }
        public ProjectProxy ActiveProject
        {
            get { return _activeProject; }
            set
            {
                SetProperty(ref _activeProject, value);
            }
        }
        public string UserName 
        { 
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
            }
        }

        public bool _isRightPaneOptionsPopUpOpen;
        public bool IsRightPaneOptionsPopUpOpen
        {
            get { return _isRightPaneOptionsPopUpOpen; }
            set
            {
                SetProperty(ref _isRightPaneOptionsPopUpOpen, value);
            }
        }

        public FrameworkElement RightPaneContentControl
        {
            get { return _rightPaneContentControl; }
            set 
            {
                SetProperty(ref _rightPaneContentControl, value);
            }
        }
        //Start up window property
        public string StartUpWindowTitle { get; set; } = "Start Up Window";
        public FrameworkElement MenuContentControl
        { get => _menuContentControl;
            set
            {
                SetProperty(ref _menuContentControl, value);
            }
        }

        //Commands
        public DelegateCommand AddsharedUserFromTopPaneCommand { get; set; }
        public DelegateCommand ShowMenuCommand { get; set; }
        public DelegateCommand ShowRightPaneCommand { get; set; }

        //Helpers
        public class RightPaneOption
        {
            public string OptionName { get; set; }
        }

        public IEventAggregator _eventAggregator;

        public StartUpWindowViewModel(IEventAggregator eventAggregator)
        {
            //Set user here
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoggedInEvent>().Subscribe(SetUser);
            _eventAggregator.GetEvent<CreateProjectEvent>().Subscribe(SetActiveProject);
            _eventAggregator.GetEvent<AddSharedUserEvent>().Subscribe(AddUserToShareProjectWith);

            //Initialize Right menu pane
            RightPaneContentControl = new RightPaneView();
            _rightPaneViewModel = new RightPaneViewModel(_eventAggregator);
            RightPaneContentControl.DataContext = _rightPaneViewModel;
            RightPaneContentControl.Visibility = Visibility.Collapsed;

            //Initialize menu pane
            MenuContentControl = new MenuPaneView();
            _menuPaneViewModel = new MenuPaneViewModel(_eventAggregator);
            MenuContentControl.DataContext = _menuPaneViewModel;
            MenuContentControl.Visibility = Visibility.Collapsed;

            AddsharedUserFromTopPaneCommand = new DelegateCommand(AddSharedUserFromTopPane, () => true);
            ShowMenuCommand = new DelegateCommand(ShowMenu, () => true);
            ShowRightPaneCommand = new DelegateCommand(ShowRightPane, () => true);

            StatusBarMessage = "Ready";
        }

        private void AddUserToShareProjectWith(UserProxy sharedUser)
        {
            //Should not be able to add self to shared users
            //should be able to shared if project has been created
            if (_activeProject != null)
            {
                StatusBarMessage = $"Adding {sharedUser.UserEmailAddress} to list of shared users!";
                _menuPaneViewModel.shareProjectWindowView.Visibility = Visibility.Collapsed;
                var sharedProjectFullname = ActiveProject.ProjectName + "_" + User.UserEmailAddress;
                if (shared.ContainsKey(sharedProjectFullname))
                {
                    shared[sharedProjectFullname].Add(sharedUser.UserEmailAddress);
                }
                else
                {
                    shared.Add(sharedProjectFullname, new List<string>());
                    shared[sharedProjectFullname].Add(sharedUser.UserEmailAddress);
                }
                //Send notification to SharedProjectWindow
                _eventAggregator.GetEvent<ProjectSharedToAnotherUser>().Publish(sharedUser.UserEmailAddress);
                StatusBarMessage = "Done!";
                return;
            }

            StatusBarMessage = "Create Project First";
        }

        private void SetActiveProject(ProjectProxy createdProject)
        {
            _menuPaneViewModel.createProjectWindowView.Visibility = Visibility.Collapsed;
            //Check if project name exists in user projects
            if (User.UserCreatedProjects != null)
            {
                var userCreatedProjects = User.UserCreatedProjects;
                foreach(var project in userCreatedProjects)
                {
                    if(createdProject.ProjectName == project.ProjectName)
                    {
                        //Exit method
                        //Creation Not successful

                        _menuPaneViewModel.createProjectWindowView.Visibility = Visibility.Visible;
                        MessageBox.Show("Project name already exists!");
                        return;
                    }
                }
            }
            else
            {
                User.UserCreatedProjects = new List<ProjectProxy>();
            }
            //Make user project creator
            createdProject.ProjectCreator = User;
            //Project creator can edit project
            createdProject.CanEdit = true;
            //Add project to list of user created projects
            //Make project active project
            ActiveProject = createdProject;
            User.UserCreatedProjects.Add(createdProject);

            //Set the project name on the menu pane
            _menuPaneViewModel.ProjectName = createdProject.ProjectName;
            MenuContentControl.DataContext = _menuPaneViewModel;
        }

        private bool CanCreateProject()
        {
            //Validation Logic
            return true;
        }

        //Set User
        private void SetUser(UserProxy user)
        {
            User = user;
            UserName = User.UserFirstName + " " + User.UserLastName;
        }

        //Menu
        private void ShowMenu()
        {
            MenuContentControl.Visibility = Visibility.Visible;
        }

        //Add Shared user
        private void AddSharedUserFromTopPane()
        {
            _menuPaneViewModel.ShowShareProjectWindow();
        }

        private void ShowRightPane()
        {
            RightPaneContentControl.Visibility = Visibility.Visible;
        }
    }
}
