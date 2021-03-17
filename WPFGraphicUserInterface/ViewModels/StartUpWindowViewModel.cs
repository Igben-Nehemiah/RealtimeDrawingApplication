using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;
using Prism.Ioc;

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
        public string StartUpWindowTitle { get; set; } = "Simple Drawing Application";

        //There should be an active user on start up
        private UserProxy _userProxy;
        private ProjectProxy _activeProject;

        //The main window is made up of three parts,
        //the right pane, the menu pane and the drawing canvas
        private RightPaneViewModel _rightPaneViewModel;
        private MenuPaneViewModel _menuPaneViewModel;
        private DrawingCanvas _drawingCanvas; //This is both a view and a view model because it extends the default Canvas class

        private FrameworkElement _rightPaneContentControl;
        private FrameworkElement _menuContentControl;

        private string _statusBarMessage;
        private string _importOrExport = "";
        private bool _canEdit;
        private bool _isSharedProject;

        //A store for the shared projects
        Dictionary<string, List<string>> shared = new Dictionary<string, List<string>>();

        public UserProxy UserProxy
        {
            get { return _userProxy; }
            set
            {
                SetProperty(ref _userProxy, value);
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
        public FrameworkElement RightPaneContentControl
        {
            get { return _rightPaneContentControl; }
            set 
            {
                SetProperty(ref _rightPaneContentControl, value);
            }
        }
        public FrameworkElement MenuContentControl
        { get => _menuContentControl;
            set
            {
                SetProperty(ref _menuContentControl, value);
            }
        }
        public DrawingCanvas DrawingCanvas
        {
            get { return _drawingCanvas; }
            set
            {
                SetProperty(ref _drawingCanvas, value);
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

        #region Import and export
        public string ImportOrExport
        {
            get { return _importOrExport; }
            set
            {
                SetProperty(ref _importOrExport, value);
            }
        }
        private bool _importExportPopUpIsOpen = false;
        public ObservableCollection<ImportExportOption> ImportExportPopUpOptions { get; set; }
        public class ImportExportOption
        {
            public string ImportExportOptionName { get; set; }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                SetProperty(ref _selectedIndex, value);
                SelectImportExportOption();
            }
        }

        public bool ImportExportPopUpIsOpen
        {
            get { return _importExportPopUpIsOpen; }
            set
            {
                SetProperty(ref _importExportPopUpIsOpen, value);
            }
        }
        #endregion

        //Commands
        public DelegateCommand AddsharedUserFromTopPaneCommand { get; set; }
        public DelegateCommand ShowMenuCommand { get; set; }
        public DelegateCommand ShowRightPaneCommand { get; set; }
        public DelegateCommand ExportFromTopPaneCommand { get; set; }

        private IEventAggregator _eventAggregator;

        public StartUpWindowViewModel()
        {
            InitializeClass();
        }

        private void InitializeClass()
        {
            _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();

            //Events registered to this class
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
            _eventAggregator.GetEvent<DeleteProjectBtnClickEvent>().Subscribe(DeleteActiveProject);
            _eventAggregator.GetEvent<OpenProjectBtnClickEvent>().Subscribe(OpenUserProjects);
            _eventAggregator.GetEvent<SignOutBtnClickEvent>().Subscribe(SignOut);
            _eventAggregator.GetEvent<CloseMenuBtnClickEvent>().Subscribe(CloseMenu);
            _eventAggregator.GetEvent<ShowPopupEvent>().Subscribe(ShowImportExportOptions);

            //Initialize Right menu pane
            RightPaneContentControl = new RightPaneView();
            _rightPaneViewModel = new RightPaneViewModel();
            RightPaneContentControl.DataContext = _rightPaneViewModel;
            RightPaneContentControl.Visibility = Visibility.Collapsed;

            //Initialize menu pane
            MenuContentControl = new MenuPaneView();
            _menuPaneViewModel = new MenuPaneViewModel();
            MenuContentControl.DataContext = _menuPaneViewModel;
            MenuContentControl.Visibility = Visibility.Collapsed;

            //Initialize Drawing canvas
            DrawingCanvas = new DrawingCanvas();
            DrawingCanvas.AllowDrop = false;

            AddsharedUserFromTopPaneCommand = new DelegateCommand(AddSharedUserFromTopPane, () => true);
            ShowMenuCommand = new DelegateCommand(ShowMenu, () => true);
            ShowRightPaneCommand = new DelegateCommand(ShowRightPane, () => true);
            ExportFromTopPaneCommand = new DelegateCommand(ExportFromTopPane, () => true);

            //Import and export 
            //Initialize and add import and export pop up options
            ImportExportPopUpOptions = new ObservableCollection<ImportExportOption>
            {
                new ImportExportOption() { ImportExportOptionName = "JSON" },
                new ImportExportOption() { ImportExportOptionName = "XML" }
            };

            StatusBarMessage = "Ready";
        }

        private void CloseMenu()
        {
            MenuContentControl.Visibility = Visibility.Collapsed;
        }

        private void SignOut()
        {
            var result = MessageBox.Show("Do you want to sign out?", "Sign out", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                //Save active project before signing out
                if (ActiveProject != null) SaveProject();

                _eventAggregator.GetEvent<SigningOutEvent>().Publish();
            }
        }

        private void OpenUserProjects()
        {
            _rightPaneViewModel.SelectedIndex = 2;
            ShowRightPane();
        }

        private void DeleteActiveProject()
        {
            if (ActiveProject == null)
            {
                MessageBox.Show("Select a project to delete!");
                return;
            }

            if (_isSharedProject)
            {
                MessageBox.Show("Cannot delete a shared project!");
                return;
            }

            var result = MessageBox.Show("Do you want to delete project?", "Delete Project", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Deleting Project!");

                DrawingCanvas = new DrawingCanvas();

                //if project not yet saved do something
                DataAccessLayer.DeleteProject(ActiveProject);//This is responsible for removing the project from the db and clearing related items

                //Remove project from project list
                var projectToRemove = _rightPaneViewModel.ProjectPaneViewModel.Projects.FirstOrDefault(p => p.ToLower() == ActiveProject.ProjectName);

                _rightPaneViewModel.ProjectPaneViewModel.Projects.Remove(projectToRemove);

                _rightPaneViewModel.ProjectPaneViewModel.SelectedProject = null;

                _rightPaneViewModel.ProjectPaneViewModel.Projects.Remove(projectToRemove);
                //Reset project name to 
                _menuPaneViewModel.ProjectName = "";

                ActiveProject = null;

                SetUser(UserProxy);
            }
            StatusBarMessage = "Ready!";
        }

        private void ExportFromTopPane()
        {
            _eventAggregator.GetEvent<ShowPopupEvent>().Publish("export");
        }

        bool IsProjectNameInProjects(string projectName, IEnumerable<ProjectProxy> projectProxies)
        {
            foreach (var projectproxy in projectProxies)
            {
                if (projectproxy.ProjectName.ToLower() == projectName.ToLower())
                    return true;
            }

            return false;
        }

        private void LoadProjectFromProjectPane(string projectName)
        {
            //Check if project is user created project
            var inUserCreatedProjects = IsProjectNameInProjects(projectName, UserProxy.UserCreatedProjects);


            IEnumerable<DrawingCanvasObjectProxy> drawingCanvasObjectsProxies;

            if (inUserCreatedProjects)
            {
                DrawingCanvas = new DrawingCanvas();

                ActiveProject = DataAccessLayer.LoadProjectFromDatabase(UserProxy, projectName);

                _canEdit = true;
                _isSharedProject = false;

                drawingCanvasObjectsProxies = DataAccessLayer.LoadProjectWithProjectName(projectName, UserProxy.UserId);

                //Load shared users
                var id = DataAccessLayer.GetProjectWithProjectName(ActiveProject.ProjectName, UserProxy.UserId).ProjectId;

                _eventAggregator.GetEvent<ClearSharedUsersListEvent>().Publish();

                foreach (var item in UserProxy.UserSharedProjects)
                {
                    var projectSharedUser = new ProjectUserProxy();

                    projectSharedUser.ProjectId = id;
                    projectSharedUser.UserId = item.UserId;

                    projectSharedUser.SharedProject = ActiveProject;
                    projectSharedUser.SharedUser = DataAccessLayer.GetUserWithUserId(item.UserId);
                    projectSharedUser.CanEdit = item.CanEdit;

                    _eventAggregator.GetEvent<ProjectSharedToAnotherUserEvent>().Publish(projectSharedUser);
                }
            }
            else
            {
                var result = MessageBox.Show("Is Shared project. Do you want to load project?",
                    "Load shared project", MessageBoxButton.YesNo);
                var sharedProject = new ProjectProxy();

                if (result == MessageBoxResult.No) return;

                DrawingCanvas = new DrawingCanvas();


                foreach (var projectUserProxy in UserProxy.UserSharedProjects)
                {
                    var projectProxy = DataAccessLayer.GetProjectWithId(projectUserProxy.ProjectId);

                    if(projectName.ToLower() == projectProxy.ProjectName.ToLower())
                    {
                        sharedProject = projectProxy;

                        ActiveProject = sharedProject;
                        _canEdit = projectUserProxy.CanEdit;
                        DrawingCanvas.AllowDrop = _canEdit;
                        break;
                    }
                }

                _isSharedProject = true;

                drawingCanvasObjectsProxies = DataAccessLayer.LoadProjectDrawingCanvasObjects(sharedProject);
            }



            ActiveProject.ProjectDrawingCanvasObjects = drawingCanvasObjectsProxies.ToList();

            StatusBarMessage = "Setting up drawing Canvas!";

            UnpackProjectToCanvas(drawingCanvasObjectsProxies);

            _menuPaneViewModel.ProjectName = projectName;

            StatusBarMessage = "Ready!";
        }

        private async void RemoveSharedUser(string sharedUserEmailAddress)
        {
            var id = DataAccessLayer.GetProjectWithProjectName(ActiveProject.ProjectName, UserProxy.UserId).ProjectId;
            await DataAccessLayer.RemoveSharedUserAsync(sharedUserEmailAddress, id);
        }

        private async void RefreshSharedUserDetails((string, bool) newInfo)
        {
            await DataAccessLayer.EditSharedUserDetailsAsync(newInfo, ActiveProject.ProjectId);
        }

        private void SetStatusbarMessage(string newStatusbarMessage)
        {
            StatusBarMessage = newStatusbarMessage;
        }

        private async void ExportProject(string exportType)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = $"{exportType.ToLower()} document(*.{exportType.ToLower()}|*.{exportType.ToLower()}"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                await ExporterImporter.ExportAsync(ActiveProject, filePath, exportType);
            }
        }

        private async void ImportProject(string importType)
        {
            //Create a project first
            if (ActiveProject == null)
            {
                MessageBox.Show("Create a project first!","No Active project",MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = $"{importType.ToLower()} document(*.{importType.ToLower()}|*.{importType.ToLower()}"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;

                IEnumerable<DrawingCanvasObjectProxy> drawingCanvasObjectsProxies;

                try
                {
                    drawingCanvasObjectsProxies = await ExporterImporter.ImportAsync(filePath, importType);
                    //Set up drawing canvas
                    DrawingCanvas = new DrawingCanvas();

                    StatusBarMessage = "Setting up drawing Canvas!";

                    UnpackProjectToCanvas(drawingCanvasObjectsProxies);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to import file. Make sure file type matches import type.","Import Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    StatusBarMessage = "Ready!";
                }
            }
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

        private async void SaveProject()
        {
            if (ActiveProject != null)
            {
                var canvasItems = PackProject();

                int userId;

                if (!_canEdit)
                {
                    MessageBox.Show("Cannot Save this project because it has been locked by it's creator!");
                    return;
                }

                if (_isSharedProject)
                {
                    
                    userId = DataAccessLayer.GetProjectCreatorIdWithProjectId(ActiveProject.ProjectId);
                }
                else
                {
                    userId = UserProxy.UserId;
                }
                //Save or Update project
                await DataAccessLayer.SaveProjectDrawingCanvasObjectsToDBAsync(canvasItems, 
                    DataAccessLayer.GetProjectWithProjectName(ActiveProject.ProjectName, userId));
            }
            else
            {
                MessageBox.Show("Create or select a project first!", "Saving unsuccessful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private List<DrawingCanvasObjectProxy> PackProject()
        {
            _eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Packing project!");

            var canvasItems = new List<DrawingCanvasObjectProxy>();

            foreach (var child in DrawingCanvas.Children)
            {
                var ch = child as ISelectedObject;

                var item = ConvertFrameworkElementToDrawingCanvasObjectProxy(ch);

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

        private async void AddUserToShareProjectWith((UserProxy, bool) sharedUserInfo)
        {
            //should be able to shared if project has been created
            if (_activeProject != null)
            {
                if (_isSharedProject)
                {
                    MessageBox.Show("Cannot shared shared project!");
                    return;
                }
                //Should not be able to add self to shared users
                if (sharedUserInfo.Item1.UserEmailAddress == UserProxy.UserEmailAddress)
                {
                    MessageBox.Show("Silly, cannot add one's self to shared users :)!");
                    return;
                }

                StatusBarMessage = $"Adding {sharedUserInfo.Item1.UserEmailAddress} to list of shared users!";
                _menuPaneViewModel.shareProjectWindowView.Visibility = Visibility.Collapsed;

                var projectSharedUser = new ProjectUserProxy();

                var id = DataAccessLayer.GetProjectWithProjectName(ActiveProject.ProjectName, UserProxy.UserId).ProjectId;
                projectSharedUser.ProjectId = id;
                projectSharedUser.UserId = sharedUserInfo.Item1.UserId;

                projectSharedUser.SharedProject = ActiveProject;
                projectSharedUser.SharedUser = sharedUserInfo.Item1;
                projectSharedUser.CanEdit = sharedUserInfo.Item2;

                await DataAccessLayer.AddSharedUserToDatabaseAsync(projectSharedUser);

                UserProxy.UserSharedProjects.Add(projectSharedUser);

                _eventAggregator.GetEvent<ProjectSharedToAnotherUserEvent>().Publish(projectSharedUser);
                StatusBarMessage = "Ready!";
                return;
            }

            MessageBox.Show("Create a project to share first!", "Create project", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void SetActiveProject(ProjectProxy createdProject)
        {
            _menuPaneViewModel.createProjectWindowView.Visibility = Visibility.Collapsed;

            //Check if project name exists in user projects
            if (UserProxy.UserCreatedProjects.Count != 0)
            {
                //Setting up local variable for easy access
                var userCreatedProjects = UserProxy.UserCreatedProjects;
                var userTotalProjects = new List<ProjectProxy>();

                userTotalProjects.AddRange(userCreatedProjects);

                foreach (var sharedProject in UserProxy.UserSharedProjects)
                {
                    var projectProxy = DataAccessLayer.GetProjectWithId(sharedProject.ProjectId);
                    userTotalProjects.Add(projectProxy);
                }


                foreach(var existingProject in userTotalProjects)
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
            StatusBarMessage = "Setting up drawing canvas!";

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

            _isSharedProject = false;
            _canEdit = true;
            //Refresh user
            SetUser(UserProxy);
        }

        //Set User
        private void SetUser(UserProxy pUserProxy)
        {
            UserProxy = pUserProxy;

            //Load user Created projects
            pUserProxy.UserCreatedProjects = DataAccessLayer.LoadUserProjectsFromDatabase(pUserProxy).ToList();
            pUserProxy.UserSharedProjects = DataAccessLayer.LoadUserSharedProjectsFromDatabase(pUserProxy).ToList();
            List<ProjectProxy> projectProxies = new List<ProjectProxy>();



            if (pUserProxy.UserCreatedProjects.Count != 0)
            {
                var createdProjects = pUserProxy.UserCreatedProjects;
                var sharedProjects = pUserProxy.UserSharedProjects;

                projectProxies.AddRange(createdProjects);
                
                foreach(var sharedProject in sharedProjects)
                {
                    var projectProxy = DataAccessLayer.GetProjectWithId(sharedProject.ProjectId);
                    projectProxies.Add(projectProxy);
                }
                _eventAggregator.GetEvent<UserProjectChangedEvent>().Publish(projectProxies);
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

        //Importing and exporting pop up
        private void ShowImportExportOptions(string option)
        {
            ImportOrExport = option;
            SelectedIndex = -1;
            ImportExportPopUpIsOpen = !ImportExportPopUpIsOpen;
        }

        //Export Project
        private void ExportProjectAsExportType(string exportType)
        {
            switch (exportType.ToLower())
            {
                case "json":
                    _eventAggregator.GetEvent<ExportProjectEvent>().Publish("json");
                    break;
                case "xml":
                    _eventAggregator.GetEvent<ExportProjectEvent>().Publish("xml");
                    break;
                default:
                    break;
            }
        }

        //Import Project
        private void ImportProjectAsImportType(string importType)
        {
            switch (importType.ToLower())
            {
                case "json":
                    _eventAggregator.GetEvent<ImportProjectEvent>().Publish("json");
                    break;
                case "xml":
                    _eventAggregator.GetEvent<ImportProjectEvent>().Publish("xml");
                    break;
                default:
                    break;
            }
        }

        //Pop up options selection
        private void SelectImportExportOption()
        {
            if (ImportOrExport.ToLower() == "import")
            {
                switch (_selectedIndex)
                {
                    case 0:
                        ImportProjectAsImportType("json");
                        break;
                    case 1:
                        ImportProjectAsImportType("xml");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (_selectedIndex)
                {
                    case 0:
                        ExportProjectAsExportType("json");
                        break;
                    case 1:
                        ExportProjectAsExportType("xml");
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
