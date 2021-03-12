using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class ShareProjectWindowViewModel : BindableBase
    {
        //private string _sharedUserEmailAddress = "sarahallen@gmail.com";
        //public string SharedUserEmailAddress
        //{
        //    get { return _sharedUserEmailAddress; }
        //    set
        //    {
        //        SetProperty(ref _sharedUserEmailAddress, value);
        //    }
        //}

        private bool _canEdit = false;

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                SetProperty(ref _canEdit, value);
            }
        }

        private UserProxy _sharedUser = new UserProxy();
        
        public UserProxy SharedUser
        {
            get { return _sharedUser; }
            set
            {
                SetProperty(ref _sharedUser, value);
            }
        }

        public DelegateCommand AddSharedUserCommand { get; set; }

        IEventAggregator _eventAggregator;

        public ShareProjectWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AddSharedUserCommand = new DelegateCommand(AddSharedUser, () => true);
        }

        private async void AddSharedUser()
        {
            //Perform validation
            var isApplicationUser = await DAL.CheckIfIsApplicationUserAsync(SharedUser.UserEmailAddress);

            if (isApplicationUser.Item1)
            {
                _sharedUser = isApplicationUser.Item2;

                var projectUserProxy = new ProjectUserProxy();
                _eventAggregator.GetEvent<AddSharedUserEvent>().Publish(new Tuple<UserProxy, bool>(_sharedUser, CanEdit));
                return;
            }
            //Email Address not registered in db
            _eventAggregator.GetEvent<AddSharedUserEvent>().Publish(null);
            MessageBox.Show("Email Addres not registered!");
        }
    }
}
