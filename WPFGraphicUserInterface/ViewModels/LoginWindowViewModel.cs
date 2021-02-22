using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFGraphicUserInterface.ViewModels
{
    public class LoginWindowViewModel
    {
        public UserProxy User;
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand SignUpCommand { get; set; }

        public LoginWindowViewModel()
        {
            LoginCommand = new DelegateCommand(ExecuteLogin, CanExecuteLogin).ObservesProperty(()=>UserEmail)
                                                                             .ObservesProperty(()=>UserPassword);
            SignUpCommand = new DelegateCommand(ExecuteSignUp, CanExecuteSignUp);
        }

        //Sign up
        private bool CanExecuteSignUp()
        {
            throw new NotImplementedException();
        }

        private void ExecuteSignUp()
        {
            //Take user to Create account page
            throw new NotImplementedException();
        }

        //Login
        private bool CanExecuteLogin()
        {
            bool isAccountValid = false;
            //After passing validation logic
            //Check db to see if any model matches the proxy
            //if true, set accountValid to true
            //The Check db could be in the service class...
            throw new NotImplementedException();
        }

        private void ExecuteLogin()
        {
            //If CanExecuteLogin is true then set up start up window and set user to startupwindow.user
            throw new NotImplementedException();
        }
    }
}
