using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

        public DelegateCommand AddSharedUser { get; set; }

        public SharedUsersPaneViewModel()
        {
            AddSharedUser = new DelegateCommand(ExecuteAddSharedUser, CanExecuteAddSharedUser);
        }

        //Add Shared User
        private bool CanExecuteAddSharedUser()
        {
            return true;
        }

        private void ExecuteAddSharedUser()
        {
            throw new NotImplementedException();
        }
    }
}
