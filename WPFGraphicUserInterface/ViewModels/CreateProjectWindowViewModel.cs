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
        private ProjectProxy _currentProject;
        public ProjectProxy CurrentProject
        {
            get { return _currentProject; }
            set
            {
                SetProperty(ref _currentProject, value);
            }
        }
        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                SetProperty(ref _projectName, value);
            }
        }

        public DelegateCommand CreateProjectCommand { get; set; }

        IEventAggregator _eventAggregator;

        public CreateProjectWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CreateProjectCommand = new DelegateCommand(CreateProject, CanCreateProject);
        }

        private bool CanCreateProject()
        {
            //Validation Logic
            return true;
        }

        private void CreateProject()
        {
            _currentProject = new ProjectProxy();
            _currentProject.ProjectName = ProjectName;
            _eventAggregator.GetEvent<CreateProjectEvent>().Publish(_currentProject);
        }
    }
}
