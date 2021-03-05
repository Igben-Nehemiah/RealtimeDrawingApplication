using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public interface ISelectedObject
    {
        string ControlType { get; set; }
        string SelectedObjectTitle { get; set; }
        double SelectedObjectWidth { get; set; }
        double SelectedObjectHeight { get; set; }
        Brush SelectedObjectFill { get; set; }
        Brush SelectedObjectBorder { get; set; }
        double SelectedObjectXPos { get; set; }
        double SelectedObjectYPos { get; set; }
        Guid SelectedObjectId { get; set; }
    }

    public class StartUpWindowViewModel : BindableBase
    {
        private RightPaneViewModel _rightPaneViewModel;
        private FrameworkElement _rightPaneContentControl;
        private UserProxy _userProxy;
        private ProjectProxy _activeProject;
        private string _statusBarMessage;
        private MenuPaneViewModel _menuPaneViewModel;
        private FrameworkElement _menuContentControl;
        private DrawingCanvas _drawingCanvas;

        //A store for the shared projects
        Dictionary<string, List<string>> shared = new Dictionary<string, List<string>>();

        public DrawingCanvas DrawingCanvas
        {
            get { return _drawingCanvas; }
            set
            {
                SetProperty(ref _drawingCanvas, value);
            }
        }
        public UserProxy UserProxy
        {
            get { return _userProxy; }
            set
            {
                SetProperty(ref _userProxy, value);
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
        public FrameworkElement MenuContentControl
        { get => _menuContentControl;
            set
            {
                SetProperty(ref _menuContentControl, value);
            }
        }
        public string StartUpWindowTitle { get; set; } = "Realtime Drawing Application";

        //Commands
        public DelegateCommand AddsharedUserFromTopPaneCommand { get; set; }
        public DelegateCommand ShowMenuCommand { get; set; }
        public DelegateCommand ShowRightPaneCommand { get; set; }

        public IEventAggregator _eventAggregator;

        public StartUpWindowViewModel(IEventAggregator eventAggregator)
        {
            //Set user here
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoggedInEvent>().Subscribe(SetUser);
            _eventAggregator.GetEvent<CreateProjectEvent>().Subscribe(SetActiveProject);
            _eventAggregator.GetEvent<AddSharedUserEvent>().Subscribe(AddUserToShareProjectWith);
            _eventAggregator.GetEvent<SaveProjectEvent>().Subscribe(SaveProject);
            _eventAggregator.GetEvent<ExportProjectEvent>().Subscribe(ExportProject);
            _eventAggregator.GetEvent<ImportProjectEvent>().Subscribe(ImportProject);

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

        private void ExportProject(string exportType)
        {
            StatusBarMessage = $"Starting Export as {exportType}";

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                ExporterImporter.Export(ActiveProject, filePath, exportType);
            }

            StatusBarMessage = $"Finished Exporting as {exportType.ToUpper()}";
        }

        private void ImportProject(string importType)
        {
            StatusBarMessage = $"Starting Import as {importType}";

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;

                ActiveProject.ProjectDrawingCanvasObjects = (List<DrawingCanvasObjectProxy>)ExporterImporter.Import(filePath, importType);

                //Set up drawing canvas
                DrawingCanvas = new DrawingCanvas();

                foreach (var drawingObj in ActiveProject.ProjectDrawingCanvasObjects)
                {
                    DrawingCanvas.Children.Add(drawingObj);
                   // var element = drawingObj as FrameworkElement;
                   //Create ISelectedObject

                    DrawingCanvas.SetItemOnCanvas(drawingObj, drawingObj.XPosition, drawingObj.YPosition);
                    //DrawingCanvas.SetLeft(drawingObj, drawingObj.XPosition);
                    //DrawingCanvas.SetTop(drawingObj, drawingObj.YPosition);
                }
            }

            //Check later if import is successful
            StatusBarMessage = $"Import as {importType} complete!";

        }

        private void SaveProject()
        {
            if (ActiveProject != null)
            {
                StatusBarMessage = "Saving project";
                
                foreach (var child in DrawingCanvas.Children)
                {
                    var ch = child as ISelectedObject;

                    var item = UnpackProperties(ch);

                    ActiveProject.ProjectDrawingCanvasObjects.Add(item);
                }

                ActiveProject.ProjectId = DAL.LoadProjectFromDatabase(UserProxy, ActiveProject.ProjectName).ProjectId;

                DAL.SaveProjectDrawingCanvasObjectsToDB(ActiveProject.ProjectDrawingCanvasObjects, ActiveProject);

                //UserProxy.UserCreatedProjects.Add(ActiveProject);
                StatusBarMessage = "Project Saved";

                _eventAggregator.GetEvent<UserProjectChangedEvent>().Publish(UserProxy.UserCreatedProjects);
            }
            else
            {
                StatusBarMessage = "Create a project before saving";
            }
        }

        //Helper function to unpack a child from a canvas to a drawing canvas object 
        private DrawingCanvasObjectProxy UnpackProperties(ISelectedObject ch)
        {
            var item = new DrawingCanvasObjectProxy();
            item.XPosition = ch.SelectedObjectXPos;
            item.YPosition = ch.SelectedObjectYPos;
            item.Height = ch.SelectedObjectHeight;
            item.Width = ch.SelectedObjectWidth;
            //item.BorderFill = ch.SelectedObjectBorder.ToString();
            //item.ShapeFill = ch.SelectedObjectFill.ToString();
            item.CanvasObjectName = ch.SelectedObjectTitle;
            item.Project = ActiveProject;
            //item.CanvasObjectGuid = ch.SelectedObjectId.ToString();
            //other things
            return item;
        }

        private void AddUserToShareProjectWith(UserProxy sharedUser)
        {
            if (sharedUser == null)
            {
                _menuPaneViewModel.shareProjectWindowView.Visibility = Visibility.Collapsed;
                return;
            }
            //Should not be able to add self to shared users
            //should be able to shared if project has been created
            if (_activeProject != null)
            {
                StatusBarMessage = $"Adding {sharedUser.UserEmailAddress} to list of shared users!";
                _menuPaneViewModel.shareProjectWindowView.Visibility = Visibility.Collapsed;
                var sharedProjectFullname = ActiveProject.ProjectName + "_" + UserProxy.UserEmailAddress;
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
                _eventAggregator.GetEvent<ProjectSharedToAnotherUserEvent>().Publish(sharedUser.UserEmailAddress);
                StatusBarMessage = "Done!";
                return;
            }

            StatusBarMessage = "Create Project First";
        }

        private void SetActiveProject(ProjectProxy createdProject)
        {
            _menuPaneViewModel.createProjectWindowView.Visibility = Visibility.Collapsed;

            //Check if project name exists in user projects
            if (UserProxy.UserCreatedProjects.Count != 0)
            {
                //Setting up local variable for easy access
                var userCreatedProjects = UserProxy.UserCreatedProjects;

                foreach(var existingProject in userCreatedProjects)
                {
                    if(createdProject.ProjectName == existingProject.ProjectName)
                    {
                        //Creation Not successful
                        _menuPaneViewModel.createProjectWindowView.Visibility = Visibility.Visible;
                        MessageBox.Show("Project name already exists!");
                        return;
                    }
                }
            }
            //Set up drawing canvas
            DrawingCanvas = new DrawingCanvas();
            //Make user project creatorId
            createdProject.ProjectCreator = UserProxy;
            //Initialize drawing canvas

            createdProject.ProjectDrawingCanvasObjects = new List<DrawingCanvasObjectProxy>();
            //Project creator can edit project
            createdProject.CanEdit = true;

            //Add project to list of user created projects
            //Make project active project
            ActiveProject = createdProject;

            ActiveProject.ProjectCreationDate = DateTime.Now;

            //Add newly created project to the database
            DAL.AddProjectToDatabase(ActiveProject);

            //Set the project name on the menu pane
            _menuPaneViewModel.ProjectName = createdProject.ProjectName;
            MenuContentControl.DataContext = _menuPaneViewModel;

        }

        //Set User
        private void SetUser(UserProxy pUserProxy)
        {
            UserProxy = pUserProxy;

            //Load user projects
            pUserProxy.UserCreatedProjects = DAL.LoadUserProjectsFromDatabase(pUserProxy).ToList();

            
            if (pUserProxy.UserCreatedProjects.Count != 0)
            {
                _eventAggregator.GetEvent<UserProjectChangedEvent>().Publish(pUserProxy.UserCreatedProjects);
            }
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
