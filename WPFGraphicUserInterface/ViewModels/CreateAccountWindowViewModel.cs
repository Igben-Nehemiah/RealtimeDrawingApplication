using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;
using Prism.Ioc;

namespace WPFGraphicUserInterface.ViewModels
{
    public class CreateAccountWindowViewModel : BindableBase
    {
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _password;
        private string _verifiedpassword;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                SetProperty(ref _firstName, value);
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetProperty(ref _lastName, value);
            }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                SetProperty(ref _emailAddress, value);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
            }
        }

        public string VerifiedPassword
        {
            get { return _verifiedpassword; }
            set
            {
                SetProperty(ref _verifiedpassword, value);
            }
        }

        public UserProxy UserProxy { get; set; }

        public DelegateCommand CreateAccountCommand { get; set; }

        IEventAggregator _eventAggregator;
        public CreateAccountWindowViewModel()
        {
            CreateAccountCommand = new DelegateCommand(ExecuteCreateAccount, CanExecuteCreateAccount);

            //Listen for account creation report
            _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();
        }

        

        private bool CanExecuteCreateAccount()
        {
            return true;
        }

        private void ExecuteCreateAccount()
        {
            //If entries pass validation
            if (VerifiedPassword == Password)
            {
                UserProxy = new UserProxy();
                UserProxy.UserFirstName = FirstName;
                UserProxy.UserLastName = LastName;
                UserProxy.UserPassword = Password;
                UserProxy.UserEmailAddress = EmailAddress;

                //Perform checks here

                DAL.AddNewUserToDatabase(UserProxy);

                var startUpWindowView = new StartUpWindowView();
                var startUpWindowViewModel = new StartUpWindowViewModel(_eventAggregator);
                startUpWindowView.DataContext = startUpWindowViewModel;

                _eventAggregator.GetEvent<AccountCreationStatusEvent>().Publish(true);
                _eventAggregator.GetEvent<UserLoggedInEvent>().Publish(UserProxy);
                //if succesful 
                
                startUpWindowView.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }

    
}
