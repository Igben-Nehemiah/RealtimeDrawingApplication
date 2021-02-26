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
        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                SetProperty(ref _projectName, value);
            }
        }
        public ProjectProxy CurrentProject
        {
            get { return _currentProject; }
            set
            {
                SetProperty(ref _currentProject, value);
            }
        }
        public DelegateCommand CreateProjectCommand { get; set; }

        IEventAggregator _eventAggregator;

        public CreateProjectWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CreateProjectCommand = new DelegateCommand(RaiseCreateProjectEvent, () => true);
            //_eventAggregator.GetEvent<TriedToCreateProjectEvent>().Subscribe(RaiseCreateProjectEvent);
        }

        private void RaiseCreateProjectEvent()
        {
            var project = new ProjectProxy();
            project.ProjectName = ProjectName;
            project.ProjectCreationDate = DateTime.Now;
            //The creation of a project is done in the main window because the main window holds an instance of the active project
            if (project.ProjectName != null)
            {
                _eventAggregator.GetEvent<CreateProjectEvent>().Publish(project);
            }
        }
    }
}
