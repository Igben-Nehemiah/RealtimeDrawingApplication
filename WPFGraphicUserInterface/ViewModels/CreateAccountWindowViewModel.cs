using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class CreateAccountWindowViewModel : BindableBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public UserProxy User { get; set; }

        public DelegateCommand CreateAccountCommand { get; set; }

        public CreateAccountWindowViewModel()
        {
            CreateAccountCommand = new DelegateCommand(ExecuteCreateAccount, CanExecuteCreateAccount);
        }

        private bool CanExecuteCreateAccount()
        {
            return true;
        }

        private void ExecuteCreateAccount()
        {
            //Creation logic is done here
            //Set user to account
            //StartUpWindowViewModel.User = new UserProxy();
            StartUpWindowViewModel.User = MockUserProxy.CreateMockUser();

        }
    }

    
}
