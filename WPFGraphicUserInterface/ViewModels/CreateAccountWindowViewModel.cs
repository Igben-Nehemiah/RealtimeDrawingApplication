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
        private string _verifiedpassword;

        private UserProxy _userProxy = new UserProxy();

        public UserProxy UserProxy
        {
            get { return _userProxy; }
            set
            {
                SetProperty(ref _userProxy, value);
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

        private async void ExecuteCreateAccount()
        {
            //If entries pass validation
            if (VerifiedPassword == UserProxy.UserPassword)
            {
                //Perform check here

                DataAccessLayer.AddUserToDatabase(UserProxy);

                var startUpWindowView = new StartUpWindowView();
                var startUpWindowViewModel = new StartUpWindowViewModel(_eventAggregator);

                startUpWindowView.DataContext = startUpWindowViewModel;
                startUpWindowView.Visibility = System.Windows.Visibility.Visible;

                var user = await DataAccessLayer.CheckIfIsApplicationUserAsync(UserProxy.UserEmailAddress);
                _userProxy = user.Item2;

                _eventAggregator.GetEvent<UserLoggedInEvent>().Publish(UserProxy);
                _eventAggregator.GetEvent<AccountCreationStatusEvent>().Publish(true);
            }
            //check password
        }
    }
}
