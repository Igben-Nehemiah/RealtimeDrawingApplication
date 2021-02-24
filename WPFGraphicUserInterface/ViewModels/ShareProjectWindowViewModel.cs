using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows;
using WPFGraphicUserInterface.Services;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class ShareProjectWindowViewModel : BindableBase
    {
        private string _sharedUserEmailAddress;
        public string SharedUserEmailAddress
        {
            get { return _sharedUserEmailAddress; }
            set
            {
                SetProperty(ref _sharedUserEmailAddress, value);
            }
        }

        public DelegateCommand AddSharedUserCommand { get; set; }

        IEventAggregator _eventAggregator;

        public ShareProjectWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AddSharedUserCommand = new DelegateCommand(AddSharedUser, CanAddSharedUser);
        }

        private bool CanAddSharedUser()
        {
            return true;
        }

        private void AddSharedUser()
        {
            //Perform validation
            var userDatabase = UserProxyProvider.GenerateUsers();
            foreach (var user in userDatabase)
            {
                if (user.UserEmailAddress == SharedUserEmailAddress)
                {
                    _eventAggregator.GetEvent<AddSharedUserEvent>().Publish(user);
                }
            }
            //Email Address not registered in db
            MessageBox.Show("Email Addres not registered!");

        }
    }
}
