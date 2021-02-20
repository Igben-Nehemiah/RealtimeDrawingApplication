using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class StartUpWindowViewModel : BindableBase
    {
        

        //Start up window property
        public string StartUpWindowTitle { get; set; } = "Main Window";
        private ShareProjectWindowView ShareProjectWindowView { get; set; }

        private FrameworkElement _sharedUsersPaneContentControl;

        public FrameworkElement SharedUsersPaneContentControl
        {
            get { return _sharedUsersPaneContentControl; }
            set 
            {
                SetProperty(ref _sharedUsersPaneContentControl, value);
            }
        }

        private FrameworkElement _propertyPaneContentControl;

        public FrameworkElement PropertyPaneContentControl
        {
            get { return _propertyPaneContentControl; }
            set
            {
                SetProperty(ref _propertyPaneContentControl, value);
            }
        }

        private FrameworkElement _projectPaneContentControl;

        public FrameworkElement ProjectPaneContentControl
        {
            get { return _projectPaneContentControl; }
            set
            {
                SetProperty(ref _projectPaneContentControl, value);
            }
        }


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
        public DelegateCommand SetSharedUsersPaneCommand { get; set; }
        public DelegateCommand SetPropertyPaneCommand { get; set; }
        public DelegateCommand SetProjectPaneCommand { get; set; }


        public StartUpWindowViewModel()
        {
            AddsharedUserCommand = new DelegateCommand(ExecuteAddSharedUser, CanExecuteAddSharedUser);
            MenuCommand = new DelegateCommand(ExecuteMenu, CanExecuteMenu).ObservesProperty(()=>MenuContentControl);
            SetSharedUsersPaneCommand = new DelegateCommand(ExecuteSetShareUsersPane, CanExecuteSetSharedUsersPane);
            SetPropertyPaneCommand = new DelegateCommand(ExecutePropetyPane, CanExecutePropertyPane);
            SetProjectPaneCommand = new DelegateCommand(ExecuteProjectPane, CanExecuteProjectPane);

        }

        //Shared Project Pane
        private bool CanExecuteProjectPane()
        {
            return true;
        }

        private void ExecuteProjectPane()
        {
            PropertyPaneContentControl = new ProjectPaneView();
        }

        //Shared Property Pane
        private bool CanExecutePropertyPane()
        {
            return true;
        }

        private void ExecutePropetyPane()
        {
            PropertyPaneContentControl = new PropertyPaneView();
        }

        //Shared User Pane
        private bool CanExecuteSetSharedUsersPane()
        {
            return true;
        }

        private void ExecuteSetShareUsersPane()
        {
            SharedUsersPaneContentControl = new SharedUsersPaneView();
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
    }
}
