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
        public CreateProjectWindowView createProjectWindowView;
        public CreateProjectWindowViewModel _createProjectWindowViewModel;
        public ShareProjectWindowView shareProjectWindowView;
        public ShareProjectWindowViewModel _shareProjectWindowViewModel;

        //Create Project view
        private string _projectName = "No Name";
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                SetProperty(ref _projectName, value);
            }
        }

        private string _importOrExport = "";
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
        public DelegateCommand SignOutCommand { get; set; }
        public DelegateCommand CloseMenuCommand { get; set; }


        IEventAggregator _eventAggregator;

        public MenuPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowAddSharedUserEvent>().Subscribe(ShowShareProjectWindow);
            ShowCreateProjectWindowCommand = new DelegateCommand(ShowCreateProjectWindow, ()=>true);
            ShowShareProjectWindowCommand = new DelegateCommand(ShowShareProjectWindow, ()=>true);
            OpenProjectCommand = new DelegateCommand(ExecuteOpenProject, CanExecuteOpenProject);
            SaveProjectCommand = new DelegateCommand(ExecuteSaveProject, CanExecuteSaveProject);
            DeleteProjectCommand = new DelegateCommand(ExecuteDeleteProject, CanExecuteDeleteProject);
            ShowImportExportOptionsCommand = new DelegateCommand<string>(ShowImportExportOptions, CanShowImportExportOptions);
            SignOutCommand = new DelegateCommand(SignOut, () => true);
            CloseMenuCommand = new DelegateCommand(CloseMenu, () => true);

            //Initialize and add import and export pop up options
            ImportExportPopUpOptions = new ObservableCollection<ImportExportOption>
            {
                new ImportExportOption() { ImportExportOptionName = "JSON" },
                new ImportExportOption() { ImportExportOptionName = "XML" }
            };
        }

        private void CloseMenu()
        {
            _eventAggregator.GetEvent<CloseMenuBtnClickEvent>().Publish();
        }

        private void SignOut()
        {
            _eventAggregator.GetEvent<SignOutBtnClickEvent>().Publish();
        }

        //Resposible for showing pop-up
        private bool CanShowImportExportOptions(string option)
        {
            return true;
        }

        private void ShowImportExportOptions(string option)
        {
            ImportOrExport = option;
            SelectedIndex = -1;
            ImportExportPopUpIsOpen = !ImportExportPopUpIsOpen;
        }

        //Delete Project
        private bool CanExecuteDeleteProject()
        {
            return true;
        }

        private void ExecuteDeleteProject()
        {
            _eventAggregator.GetEvent<DeleteProjectBtnClickEvent>().Publish();
        }

        //Export Project
        private void ExportProject(string exportType)
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
        private void ImportProject(string importType)
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

        //Save Project
        private bool CanExecuteSaveProject()
        {
            return true;
        }

        private void ExecuteSaveProject()
        {
            _eventAggregator.GetEvent<SaveProjectEvent>().Publish();
        }

        //Open Project
        private bool CanExecuteOpenProject()
        {
            return true;
        }

        private void ExecuteOpenProject()
        {
            _eventAggregator.GetEvent<OpenProjectBtnClickEvent>().Publish();
        }

        public void ShowShareProjectWindow()
        {
            shareProjectWindowView = new ShareProjectWindowView();
            _shareProjectWindowViewModel = new ShareProjectWindowViewModel(_eventAggregator);
            //shareProjectWindowView.DataContext = _shareProjectWindowViewModel;
            shareProjectWindowView.ShowDialog();
        }

        //Share Project
        private void ShowCreateProjectWindow()
        {

            createProjectWindowView = new CreateProjectWindowView();
            _createProjectWindowViewModel = new CreateProjectWindowViewModel(_eventAggregator);
           
            createProjectWindowView.DataContext = _createProjectWindowViewModel;
            createProjectWindowView.ShowDialog();
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
