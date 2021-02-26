using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class SharedUsersPaneViewModel : BindableBase
    {
        private ObservableCollection<string> _sharedUsers = new ObservableCollection<string>();

        public ObservableCollection<string> SharedUsers
        {
            get { return _sharedUsers; }
            set
            {
                SetProperty(ref _sharedUsers, value);
            }
        }

        public DelegateCommand AddSharedUserCommand { get; set; }

        IEventAggregator _eventAggregator;

        public SharedUsersPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ProjectSharedToAnotherUser>().Subscribe(RefreshSharedProjectPane);
            AddSharedUserCommand = new DelegateCommand(ExecuteAddSharedUser, CanExecuteAddSharedUser);
        }

        private void RefreshSharedProjectPane(string sharedUserEmailAddress)
        {
            SharedUsers.Add(sharedUserEmailAddress);
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
