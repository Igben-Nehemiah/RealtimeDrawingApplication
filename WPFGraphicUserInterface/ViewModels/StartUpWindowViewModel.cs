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
using WPFGraphicUserInterface.Views;

namespace WPFGraphicUserInterface.ViewModels
{
    public static class MockUserProxy
    {
        public static UserProxy CreateMockUser()
        {
            return new UserProxy()
            {
                EmailAddress = "igbennehemy@gmail.com",
                FirstName = "Obomaese",
                LastName = "Igben"
            };
        }

    }
    public class StartUpWindowViewModel : BindableBase
    {
        public static UserProxy User;
        public static ProjectProxy CurrentProject;
        public string UserName { get; set; }
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

        public StartUpWindowViewModel()
        {
            User = MockUserProxy.CreateMockUser();
            UserName = User.FirstName + " " + User.LastName;
            AddsharedUserCommand = new DelegateCommand(ExecuteAddSharedUser, CanExecuteAddSharedUser);
            MenuCommand = new DelegateCommand(ExecuteMenu, CanExecuteMenu).ObservesProperty(()=>MenuContentControl);
            ShowRightPaneCommand = new DelegateCommand(ShowRightPane, CanShowRightPane);

            //Set visibility of Right pane
            RightPaneContentControl.Visibility = Visibility.Collapsed;
            
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
