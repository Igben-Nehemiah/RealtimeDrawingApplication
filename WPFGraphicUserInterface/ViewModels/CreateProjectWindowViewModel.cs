using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class CreateProjectWindowViewModel : BindableBase
    {
        
        private string ProjectCreator;
        private ObservableCollection<UserProxy> ProjectSharedUsers;

        private string _projectTitle;
        public string ProjectTitle
        {
            get { return _projectTitle; }
            set
            {
                _projectTitle = value;
                RaisePropertyChanged();
            }
        }


        private ProjectProxy ProjectProxy;


        public CreateProjectWindowViewModel()
        {
            CreateProjectCommand = new DelegateCommand(ExecuteCreateProject, CanExecuteCreateProject);
        }

        private bool CanExecuteCreateProject()
        {
            //Validation Logic
            return true;
        }

        private void ExecuteCreateProject()
        {
            StartUpWindowViewModel.CurrentProject = MockProjectProxy.CreateMockProject();
            ProjectCreator = StartUpWindowViewModel.User.FirstName;
            var message = String.Format($"\nCurrent Project: {StartUpWindowViewModel.CurrentProject}\nProject Creator: {ProjectCreator}");
            MessageBox.Show(message, "TESTING RUNNING");
        }

        //On Create Project button click, check if project name already exists. All validations should be done in the project proxy
        public DelegateCommand CreateProjectCommand { get; set; }

    }

    public static class MockProjectProxy
    {
        public static ProjectProxy CreateMockProject()
        {
            return new ProjectProxy()
            {
                ProjectName = "My first project",
                SharedUsers = new List<UserProxy>()
            };
        }

    }
}
