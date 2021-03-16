using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WPFGraphicUserInterface.ModelProxies;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class ProjectPaneViewModel : BindableBase
    {
        private string _selectedProject;

        private ObservableCollection<string> _projects = new ObservableCollection<string>();

        public string SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                SetProperty(ref _selectedProject, value);

                if (_selectedProject != null) 
                    _eventAggregator.GetEvent<SelectedProjectChangedEvent>().Publish(_selectedProject);
            }
        }

        public ObservableCollection<string> Projects
        {
            get { return _projects; }
            set
            {
                SetProperty(ref _projects, value);
            }
        }

        private IEventAggregator _eventAggregator;
        
        public ProjectPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserProjectChangedEvent>().Subscribe(ChangeProjectList);
            //_eventAggregator.GetEvent<RemoveProjectFromProjectPaneEvent>().Subscribe(RemoveProjectFromList);
            Projects = new ObservableCollection<string>();
        }

        //private void RemoveProjectFromList(string projectName)
        //{

        //    SelectedProject = null;
        //    _projects.Remove(projectToRemove);
        //}

        private void ChangeProjectList(ICollection<ProjectProxy> projects)
        {
            Projects.Clear();

            foreach (var project in projects)
            {
                Projects.Add(project.ProjectName);
            }
        }
    }
}
