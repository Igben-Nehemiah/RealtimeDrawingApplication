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
        private UserProxy _user;
        public UserProxy User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
            }
        }

        private ProjectProxy _activeProject;
        public ProjectProxy ActiveProject
        {
            get { return _activeProject; }
            set
            {
                SetProperty(ref _activeProject, value);
            }
        }

        private string _userName;
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


        private FrameworkElement _rightPaneContentControl = new RightPaneView();

        public FrameworkElement RightPaneContentControl
        {
            get { return _rightPaneContentControl; }
            set 
            {
                SetProperty(ref _rightPaneContentControl, value);
            }
        }


        //Start up window property
        public string StartUpWindowTitle { get; set; } = "Main Window";
        private ShareProjectWindowView ShareProjectWindowView { get; set; }

        private FrameworkElement menuContentControl;

        public FrameworkElement MenuContentControl
        { get => menuContentControl;
            set
            {
                SetProperty(ref menuContentControl, value);
            }
        }


        //Commands
        public DelegateCommand AddsharedUserCommand { get; set; }
        public DelegateCommand MenuCommand { get; set; }
        public DelegateCommand ShowRightPaneCommand { get; set; }

        public class RightPaneOption
        {
            public string OptionName { get; set; }
        }

        IEventAggregator _eventAggregator;
        public StartUpWindowViewModel(IEventAggregator eventAggregator)
        {
            //Set user here
            eventAggregator.GetEvent<UserLoggedInEvent>().Subscribe(SetUser);
            eventAggregator.GetEvent<CreateProjectEvent>().Subscribe(SetCurrentProject);
            _eventAggregator = eventAggregator;
            AddsharedUserCommand = new DelegateCommand(ExecuteAddSharedUser, CanExecuteAddSharedUser);
            MenuCommand = new DelegateCommand(ExecuteMenu, CanExecuteMenu).ObservesProperty(()=>MenuContentControl);
            ShowRightPaneCommand = new DelegateCommand(ShowRightPane, CanShowRightPane);

            //Set visibility of Right pane
            RightPaneContentControl.Visibility = Visibility.Collapsed;
            
        }

        private void SetCurrentProject(ProjectProxy activeProject)
        {
            //Check if project name exists in user projects
            if (User.UserCreatedProjects != null)
            {
                var userCreatedProjects = User.UserCreatedProjects;
                foreach(var project in userCreatedProjects)
                {
                    if(activeProject.ProjectName == project.ProjectName)
                    {
                        MessageBox.Show("Project name already exists!");
                        return;
                    }
                }
            }
            else
            {
                User.UserCreatedProjects = new List<ProjectProxy>();
            }
            User.UserCreatedProjects.Add(activeProject);
            _eventAggregator.GetEvent<ProjectCreationSuccessfulEvent>().Publish(activeProject.ProjectName);
        }

        //Set User
        private void SetUser(UserProxy user)
        {
            User = user;
            UserName = User.UserFirstName + " " + User.UserLastName;
        }

        //Menu
        private bool CanExecuteMenu()
        {
            return true;
        }

        private void ExecuteMenu()
        {
            MenuContentControl = new MenuPaneView();
            MenuContentControl.Visibility = Visibility.Visible;
        }

        //Add Shared user
        private bool CanExecuteAddSharedUser()
        {
            return true;
        }

        private void ExecuteAddSharedUser()
        {
            if (ShareProjectWindowView != null) { ShareProjectWindowView = null; }
            ShareProjectWindowView = new ShareProjectWindowView();
            ShareProjectWindowView.Show();
        }

        //Show Right Pane
        private bool CanShowRightPane()
        {
            return true;
        }

        private void ShowRightPane()
        {
            RightPaneContentControl.Visibility = Visibility.Visible;
        }
    }
}
