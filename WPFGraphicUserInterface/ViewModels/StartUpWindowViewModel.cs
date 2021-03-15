using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public interface ISelectedObject
    {
        ControlEnum ControlType { get; set; }
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
        public string StartUpWindowTitle { get; set; } = "Drawing Application";

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
            _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Subscribe(SetStatusbarMessage);
            _eventAggregator.GetEvent<SharedUserInfoChangedEvent>().Subscribe(RefreshSharedUserDetails);
            _eventAggregator.GetEvent<RemoveSharedUserBtnClickEvent>().Subscribe(RemoveSharedUser);
            _eventAggregator.GetEvent<SelectedProjectChangedEvent>().Subscribe(LoadProjectFromProjectPane);

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

        private void LoadProjectFromProjectPane(string projectName)
        {
            DrawingCanvas = new DrawingCanvas();

            ActiveProject = DataAccessLayer.LoadProjectFromDatabase(UserProxy, projectName);

            var drawingCanvasObjectsProxies = DataAccessLayer.LoadProjectWithProjectName(projectName);

            //Set project name to projectName

            _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Setting up drawing Canvas!");

            UnpackProjectToCanvas(drawingCanvasObjectsProxies);

            _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");

            _menuPaneViewModel.ProjectName = projectName;
        }

        private async void RemoveSharedUser(string sharedUserEmailAddress)
        {
            await DataAccessLayer.RemoveSharedUserAsync(sharedUserEmailAddress, ActiveProject.ProjectId);
        }

        private async void RefreshSharedUserDetails(Tuple<string, bool> newInfo)
        {
            await DataAccessLayer.EditSharedUserDetailsAsync(newInfo, ActiveProject.ProjectId);
        }

        private void SetStatusbarMessage(string newStatusbarMessage)
        {
            StatusBarMessage = newStatusbarMessage;
        }

        private async void ExportProject(string exportType)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                await ExporterImporter.ExportAsync(ActiveProject, filePath, exportType);
            }
        }

        private async void ImportProject(string importType)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;

                var drawingCanvasObjectsProxies = await ExporterImporter.ImportAsync(filePath, importType);

                //Set up drawing canvas
                DrawingCanvas = new DrawingCanvas();

                _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Setting up drawing Canvas!");

                UnpackProjectToCanvas(drawingCanvasObjectsProxies);
            }
            //Check later if import is successful
            _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");

        }

        void UnpackProjectToCanvas(IEnumerable<DrawingCanvasObjectProxy> drawingCanvasObjectProxies)
        {
            foreach (var drawingCanvasObjectProxy in drawingCanvasObjectProxies)
            {
                drawingCanvasObjectProxy.Project = ActiveProject;

                var item = DrawingObjectProxyFrameworkElement.ConvertToFrameworkElement(drawingCanvasObjectProxy);

                DrawingCanvas.SetItemOnCanvas(item, drawingCanvasObjectProxy.XPosition, drawingCanvasObjectProxy.YPosition);

                DrawingCanvas.Children.Add(item);
            }
        }

        private void SaveProject()
        {
            if (ActiveProject != null)
            {
                var canvasItems = PackProject();

                if (ActiveProject.ProjectId == 0)//Project not in db yet
                {
                    ActiveProject.ProjectId = DataAccessLayer.LoadProjectFromDatabase(UserProxy, ActiveProject.ProjectName).ProjectId;

                    _eventAggregator.GetEvent<UserProjectChangedEvent>().Publish(UserProxy.UserCreatedProjects);

                }

                DataAccessLayer.SaveProjectDrawingCanvasObjectsToDBAsync(canvasItems, ActiveProject);
            }
            else
            {
                _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Create a project first!");
            }
        }

        List<DrawingCanvasObjectProxy> PackProject()
        {
            _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Packing project!");

            var canvasItems = new List<DrawingCanvasObjectProxy>();

            foreach (var child in DrawingCanvas.Children)
            {
                var ch = child as ISelectedObject;

                var item = ConvertFrameworkElementToDrawingCanvasObjectProxy(ch);

                //ActiveProject.ProjectDrawingCanvasObjects.Add(item);
                canvasItems.Add(item);
            }
            _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Packing Complete!");
            return canvasItems;
        }

        //Helper function to unpack a child from a canvas to a drawing canvas object 
        private DrawingCanvasObjectProxy ConvertFrameworkElementToDrawingCanvasObjectProxy(ISelectedObject obj)
        {
            var item = new DrawingCanvasObjectProxy
            {
                XPosition = obj.SelectedObjectXPos,
                YPosition = obj.SelectedObjectYPos,
                Height = obj.SelectedObjectHeight,
                Width = obj.SelectedObjectWidth,
                BorderFill = BrushConverterHelper.ConvertToString(obj.SelectedObjectBorder),
                ShapeFill = BrushConverterHelper.ConvertToString(obj.SelectedObjectFill),
                CanvasObjectName = obj.SelectedObjectTitle,
                Project = ActiveProject,
                CanvasObjectGuid = obj.SelectedObjectId.ToString()
            };
            
            return item;
        }

        private async void AddUserToShareProjectWith(Tuple<UserProxy, bool> sharedUserInfo)
        {
            if (sharedUserInfo == null)
            {
                _menuPaneViewModel.shareProjectWindowView.Visibility = Visibility.Collapsed;
                return;
            }
            //Should not be able to add self to shared users
            if (sharedUserInfo.Item1.UserEmailAddress == UserProxy.UserEmailAddress)
            {
                MessageBox.Show("Cannot add self to shared users!");
                return;
            }
            //should be able to shared if project has been created
            if (_activeProject != null)
            {
                StatusBarMessage = $"Adding {sharedUserInfo.Item1.UserEmailAddress} to list of shared users!";
                _menuPaneViewModel.shareProjectWindowView.Visibility = Visibility.Collapsed;

                var projectSharedUser = new ProjectUserProxy();

                projectSharedUser.ProjectId = ActiveProject.ProjectId;
                projectSharedUser.UserId = sharedUserInfo.Item1.UserId;

                projectSharedUser.SharedProject = ActiveProject;
                projectSharedUser.SharedUser = sharedUserInfo.Item1;
                projectSharedUser.CanEdit = sharedUserInfo.Item2;

                await DataAccessLayer.AddSharedUserToDatabaseAsync(projectSharedUser);

                var sharedProjectFullname = ActiveProject.ProjectName + "_" + UserProxy.UserEmailAddress;
                if (shared.ContainsKey(sharedProjectFullname))
                {
                    shared[sharedProjectFullname].Add(sharedUserInfo.Item1.UserEmailAddress);
                }
                else
                {
                    shared.Add(sharedProjectFullname, new List<string>());
                    shared[sharedProjectFullname].Add(sharedUserInfo.Item1.UserEmailAddress);
                }
                //Send notification to SharedProjectWindow
                _eventAggregator.GetEvent<ProjectSharedToAnotherUserEvent>().Publish(projectSharedUser);
                StatusBarMessage = "Done!";
                return;
            }

            StatusBarMessage = "Create Project First";
        }

        private async void SetActiveProject(ProjectProxy createdProject)
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
            _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Setting up drawing canvas!");

            DrawingCanvas = new DrawingCanvas();
            //Make user project creatorId
            createdProject.ProjectCreator = UserProxy;
            //Initialize drawing canvas

            createdProject.ProjectDrawingCanvasObjects = new List<DrawingCanvasObjectProxy>();
            //Project creator can edit project
            //createdProject.CanEdit = true;

            //Add project to list of user created projects
            //Make project active project
            ActiveProject = createdProject;

            ActiveProject.ProjectCreationDate = DateTime.Now;

            //Add newly created project to the database
            await DataAccessLayer.AddProjectToDatabaseAsync(ActiveProject);

            //Set the project name on the menu pane
            _menuPaneViewModel.ProjectName = createdProject.ProjectName;
            MenuContentControl.DataContext = _menuPaneViewModel;

            //Refresh user
            SetUser(UserProxy);
        }

        //Set User
        private void SetUser(UserProxy pUserProxy)
        {
            UserProxy = pUserProxy;

            //Load user projects
            pUserProxy.UserCreatedProjects = DataAccessLayer.LoadUserProjectsFromDatabase(pUserProxy).ToList();

            
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
