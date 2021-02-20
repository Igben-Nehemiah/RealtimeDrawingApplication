using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class MenuPaneViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        
        //Create Project view
        private CreateProjectWindowView _createProjectWindowView;
        private ShareProjectWindowView _shareProjectWindowView;


        //Commands
        public DelegateCommand CreateProjectWindowCommand { get; private set; }
        public DelegateCommand ShareProjectWindowCommand { get; private set; }
        public DelegateCommand OpenProjectCommand { get; set; }
        public DelegateCommand SaveProjectCommand { get; set; }
        public DelegateCommand DeleteProjectCommand { get; set; }
        public DelegateCommand ImportProjectCommand { get; set; }
        public DelegateCommand ExportProjectCommand { get; set; }


        public MenuPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CreateProjectWindowCommand = new DelegateCommand(ExecuteCreateProjectView, CanExecuteCreateProjectView);
            ShareProjectWindowCommand = new DelegateCommand(ExecuteShareProjectView, CanExecuteShareProjectView);
            OpenProjectCommand = new DelegateCommand(ExecuteOpenProject, CanExecuteOpenProject);
            SaveProjectCommand = new DelegateCommand(ExecuteSaveProject, CanExecuteSaveProject);
            ImportProjectCommand = new DelegateCommand(ExecuteImportProject, CanExecuteImportProject);
            ExportProjectCommand = new DelegateCommand(ExecuteExportProject, CanExecuteExportProject);
            DeleteProjectCommand = new DelegateCommand(ExecuteDeleteProject, CanExecuteDeleteProject);
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
        private bool CanExecuteExportProject()
        {
            return true;
        }

        private void ExecuteExportProject()
        {
            MessageBox.Show("Exporting Project");
        }

        //Import Project
        private bool CanExecuteImportProject()
        {
            return true;
        }

        private void ExecuteImportProject()
        {
            MessageBox.Show("Importing Project");
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
    }
}
