using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WPFGraphicUserInterface.ModelProxies;
using WPFUserInterface.Core;
using Prism.Ioc;

namespace WPFGraphicUserInterface.ViewModels
{
    public class SharedUsersPaneViewModel : BindableBase
    {
        private SharedUser _selectedProjectUser;

        private ObservableCollection<SharedUser> _projectUsers = new ObservableCollection<SharedUser>();

        public SharedUser SelectedProjectUser
        {
            get { return _selectedProjectUser; }
            set
            {
                SetProperty(ref _selectedProjectUser, value);
            }
        }

        public ObservableCollection<SharedUser> ProjectUsers
        {
            get { return _projectUsers; }
            set
            {
                SetProperty(ref _projectUsers, value);
            }
        }

        public class SharedUser : BindableBase
        {
            IEventAggregator _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();

            private bool _canEdit;
            public string SharedUserEmailAddress { get; set; }
            public bool CanEdit
            {
                get { return _canEdit; }
                set
                {
                    SetProperty(ref _canEdit, value);
                    if (!string.IsNullOrEmpty(SharedUserEmailAddress))
                    {
                        _eventAggregator.GetEvent<SharedUserInfoChangedEvent>().Publish(new Tuple<string, bool>(SharedUserEmailAddress, _canEdit));
                    }
                }
            }

        }


        public DelegateCommand AddSharedUserCommand { get; set; }

        public IEventAggregator _eventAggregator;

        public SharedUsersPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ProjectSharedToAnotherUserEvent>().Subscribe(RefreshSharedProjectPane);
            AddSharedUserCommand = new DelegateCommand(ExecuteAddSharedUser, CanExecuteAddSharedUser);
        }

        private void RefreshSharedProjectPane(ProjectUserProxy projectUserProxy)
        {
            var newSharedUser = new SharedUser();

            newSharedUser.CanEdit = projectUserProxy.CanEdit;
            newSharedUser.SharedUserEmailAddress = projectUserProxy.SharedUser.UserEmailAddress;

            ProjectUsers.Add(newSharedUser);
        }

        //Add Shared User
        private bool CanExecuteAddSharedUser()
        {
            return true;
        }

        private void ExecuteAddSharedUser()
        {
            _eventAggregator.GetEvent<ShowAddSharedUserEvent>().Publish();
        }
    }
}
