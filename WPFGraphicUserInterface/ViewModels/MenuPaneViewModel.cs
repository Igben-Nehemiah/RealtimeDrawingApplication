using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class MenuPaneViewModel : BindableBase
    {
        private CreateProjectWindowViewModel _createProjectWindowViewModel;
        private ShareProjectWindowViewModel _shareProjectWindowViewModel;

        //Create Project view
        private string _projectName = "Project Name";
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                SetProperty(ref _projectName, value);
            }
        }

        private string _importOrExport = "JSON";
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

        //Commands
        public DelegateCommand ShowCreateProjectWindowCommand { get; set; }
        public DelegateCommand ShowShareProjectWindowCommand { get; set; }
        public DelegateCommand OpenProjectCommand { get; set; }
        public DelegateCommand SaveProjectCommand { get; set; }
        public DelegateCommand DeleteProjectCommand { get; set; }
        public DelegateCommand ImportProjectCommand { get; set; }
        public DelegateCommand ExportProjectCommand { get; set; }
        public DelegateCommand<string> ShowImportExportOptionsCommand { get; set; }

        IEventAggregator _eventAggregator;

        public MenuPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ProjectCreationSuccessfulEvent>().Subscribe(SetProjectName);
            _eventAggregator.GetEvent<ShowAddSharedUserEvent>().Subscribe(ShowShareProjectWindow);
            ShowCreateProjectWindowCommand = new DelegateCommand(ShowCreateProjectWindow, ()=>true);
            ShowShareProjectWindowCommand = new DelegateCommand(ShowShareProjectWindow, ()=>true);
            OpenProjectCommand = new DelegateCommand(ExecuteOpenProject, CanExecuteOpenProject);
            SaveProjectCommand = new DelegateCommand(ExecuteSaveProject, CanExecuteSaveProject);
            DeleteProjectCommand = new DelegateCommand(ExecuteDeleteProject, CanExecuteDeleteProject);
            ShowImportExportOptionsCommand = new DelegateCommand<string>(ShowImportExportOptions, CanShowImportExportOptions);


            //Initialize and add import and export pop up options
            ImportExportPopUpOptions = new ObservableCollection<ImportExportOption>
            {
                new ImportExportOption() { ImportExportOptionName = "JSON" },
                new ImportExportOption() { ImportExportOptionName = "XML" }
            };
        }

        private void SetProjectName(string projectName)
        {
            ProjectName = projectName;
        }

        //Resposible for showing pop-up
        private bool CanShowImportExportOptions(string option)
        {
            return true;
        }

        private void ShowImportExportOptions(string option)
        {
            ImportOrExport = option;
            ImportExportPopUpIsOpen = !ImportExportPopUpIsOpen;
        }

        //Delete Project
        private bool CanExecuteDeleteProject()
        {
            return true;
        }

        private void ExecuteDeleteProject()
        {
            MessageBox.Show("Deleting Project");
        }

        //Export Project
        private void ExportProject(string exportType)
        {
            switch (exportType.ToLower())
            {
                case "json":
                    MessageBox.Show("Exporting Project  as JSON");
                    break;
                case "xml":
                    MessageBox.Show("Exporting Project  as XML");
                    break;
                default:
                    break;
            }
        }

        //Import Project
        private void ImportProject(string importType)
        {
            switch (importType.ToLower())
            {
                case "json":
                    MessageBox.Show("Importing Project  as JSON");
                    break;
                case "xml":
                    MessageBox.Show("Importing Project  as XML");
                    break;
                default:
                    break;
            }
        }

        //Save Project
        private bool CanExecuteSaveProject()
        {
            return true;
        }

        private void ExecuteSaveProject()
        {
            MessageBox.Show("Saving Project");
        }

        //Open Project
        private bool CanExecuteOpenProject()
        {
            return true;
        }

        private void ExecuteOpenProject()
        {
            MessageBox.Show("Opening Project");
        }

        //Create Project
        private bool CanShowShareProjectWindow()
        {
            return true;
        }

        public void ShowShareProjectWindow()
        {
             var shareProjectWindowView = new ShareProjectWindowView();
            _shareProjectWindowViewModel = new ShareProjectWindowViewModel(_eventAggregator);
            shareProjectWindowView.DataContext = _shareProjectWindowViewModel;
            shareProjectWindowView.ShowDialog();
        }

        //Share Project
        private void ShowCreateProjectWindow()
        {
            var createProjectWindowView = new CreateProjectWindowView();
            _createProjectWindowViewModel = new CreateProjectWindowViewModel(_eventAggregator);
            createProjectWindowView.DataContext = _createProjectWindowViewModel;
            createProjectWindowView.ShowDialog();
            
        }

        private bool CanShowCreateProjectView()
        {
            return true;
        }

        //Pop up options selection
        private void SelectImportExportOption()
        {
            if (ImportOrExport.ToLower() == "import")
            {
                switch (_selectedIndex)
                {
                    case 0:
                        ImportProject("json");
                        break;
                    case 1:
                        ImportProject("xml");
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
                        ExportProject("json");
                        break;
                    case 1:
                        ExportProject("xml");
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}
