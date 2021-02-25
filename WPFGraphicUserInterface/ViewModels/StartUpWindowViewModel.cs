using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
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
        List<Tuple<string, string>> shared = new List<Tuple<string, string>>();

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
        public ObservableCollection<RightPaneOption> RightPaneOptions { get; set; }

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
        public DelegateCommand AddsharedUserCommand { get; set; }
        public DelegateCommand ShowMenuCommand { get; set; }
        public DelegateCommand ShowRightPaneCommand { get; set; }

        //Helpers
        public class RightPaneOption
        {
            public string OptionName { get; set; }
        }

        IEventAggregator _eventAggregator;

        public StartUpWindowViewModel(IEventAggregator eventAggregator)
        {
            //Set user here
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoggedInEvent>().Subscribe(SetUser);
            _eventAggregator.GetEvent<CreateProjectEvent>().Subscribe(SetCurrentProject);
            _eventAggregator.GetEvent<AddSharedUserEvent>().Subscribe(AddSharedUser);

            AddsharedUserCommand = new DelegateCommand(AddSharedUser, () => true);
            ShowMenuCommand = new DelegateCommand(ShowMenu, ()=>true);
            ShowRightPaneCommand = new DelegateCommand(ShowRightPane, ()=>true);

            StatusBarMessage = "Ready";
        }

        private void AddSharedUser(UserProxy sharedUser)
        {
            shared.Add(new Tuple<string, string>(User.UserEmailAddress, sharedUser.UserEmailAddress));
        }

        private void SetCurrentProject(ProjectProxy activeProject)
        {
            //Check if project name exists in user projects
            if (User.UserCreatedProjects != null)
            {
                var userCreatedProjects = User.UserCreatedProjects;
                foreach(var project in userCreatedProjects)
                {
                    if(!string.IsNullOrEmpty(activeProject.ProjectName) && activeProject.ProjectName == project.ProjectName)
                    {
                        MessageBox.Show("Project name already exists!");
                        //Exit method
                        return;
                    }
                }
            }
            else
            {
                User.UserCreatedProjects = new List<ProjectProxy>();
            }
            //Add project of list of user projects
            User.UserCreatedProjects.Add(activeProject);

            //Raises an event to close project creation window||Should come up before adding
            _eventAggregator.GetEvent<ProjectCreationStatusEvent>().Publish(CanCreateProject());

            //activeProject.ProjectCreatorEmailAddress = User.UserEmailAddress;
            //Set the project name on the menu pane
            _eventAggregator.GetEvent<ProjectCreationSuccessfulEvent>().Publish(activeProject.ProjectName);
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
            MenuContentControl = new MenuPaneView();
            _menuPaneViewModel = new MenuPaneViewModel(_eventAggregator);
            MenuContentControl.DataContext = _menuPaneViewModel;
            MenuContentControl.Visibility = Visibility.Visible;
        }

        //Add Shared user
        private void AddSharedUser()
        {
            _menuPaneViewModel.ShowShareProjectWindow();
        }

        private void ShowRightPane()
        {
            RightPaneContentControl = new RightPaneView();
            _rightPaneViewModel = new RightPaneViewModel(_eventAggregator);
            RightPaneContentControl.DataContext = _rightPaneViewModel;
            RightPaneContentControl.Visibility = Visibility.Visible;
        }
    }
}
