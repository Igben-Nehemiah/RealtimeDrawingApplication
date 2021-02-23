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
        IEventAggregator _eventAggregator;

        //Create Project view
        private string _importOrExport;
        public string ImportOrExport
        {
            get { return _importOrExport; }
            set
            {
                SetProperty(ref _importOrExport, value);
            }
        }

        private CreateProjectWindowView _createProjectWindowView;
        private ShareProjectWindowView _shareProjectWindowView;
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
        public DelegateCommand CreateProjectWindowCommand { get; set; }
        public DelegateCommand ShareProjectWindowCommand { get; set; }
        public DelegateCommand OpenProjectCommand { get; set; }
        public DelegateCommand SaveProjectCommand { get; set; }
        public DelegateCommand DeleteProjectCommand { get; set; }
        public DelegateCommand ImportProjectCommand { get; set; }
        public DelegateCommand ExportProjectCommand { get; set; }
        public DelegateCommand<string> ShowImportExportOptionsCommand { get; set; }


        public MenuPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CreateProjectWindowCommand = new DelegateCommand(ExecuteCreateProjectView, CanExecuteCreateProjectView);
            ShareProjectWindowCommand = new DelegateCommand(ExecuteShareProjectView, CanExecuteShareProjectView);
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
        private bool CanExecuteShareProjectView()
        {
            return true;
        }

        private void ExecuteShareProjectView()
        {
            _shareProjectWindowView = new ShareProjectWindowView();
            _shareProjectWindowView.Show();
        }

        //Share Project
        private void ExecuteCreateProjectView()
        {
            _createProjectWindowView = new CreateProjectWindowView();
            _createProjectWindowView.Show();
        }

        private bool CanExecuteCreateProjectView()
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
