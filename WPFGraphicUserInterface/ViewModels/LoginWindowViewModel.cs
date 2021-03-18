using Prism.Commands;
using Prism.Events;using Prism.Ioc;
using Prism.Unity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;
using WPFUserInterface.Core;
using WPFGraphicUserInterface.Views;
using System.Threading.Tasks;
using System.Windows;

namespace WPFGraphicUserInterface.ViewModels
{
    public class LoginWindowViewModel : BindableBase
    {
        //private StartUpWindowViewModel _startUpWindowViewModel;
        private UserProxy _user;
        private bool isValidUser;
       
        private string _userEmail = "igbennehemiah@gmail.com";
        private string _userPassword ="Bart Allen";


        public UserProxy User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
            }
        }
        public string UserEmail
        {
            get { return _userEmail; }
            set
            {
                SetProperty(ref _userEmail, value);
            }
        }
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                SetProperty(ref _userPassword, value);
            }
        }
        //public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand SignUpCommand { get; set; }

        IEventAggregator _eventAggregator;


        public LoginWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<TriedToSignInEvent>().Subscribe(Login);
            //LoginCommand = new DelegateCommand(Login, CanLogin);
            SignUpCommand = new DelegateCommand(SignUp, CanSignUp);
        }

        //Sign up
        private bool CanSignUp()
        {
            return true;
        }
        private void SignUp()
        {
            //Take user to Create account page
            throw new NotImplementedException();
        }
        //Login
        private async void Login()
        {
            var detail = await DataAccessLayer.CheckIfUserDetailIsValidAsync(UserEmail, UserPassword);
            _user = detail.Item2;
            isValidUser = detail.Item1;
            //Throw LoggedInEvent to StartUpWindowViewModel
            if (isValidUser) 
            {
                var startUpWindowView = new StartUpWindowView();

                startUpWindowView.DataContext = new StartUpWindowViewModel();

                _eventAggregator.GetEvent<UserLoggedInEvent>().Publish(User);

                _eventAggregator.GetEvent<SignInStatusEvent>().Publish(isValidUser);

                startUpWindowView.ShowDialog();

            }
            else
            {
                MessageBox.Show("Wrong user details!");
            }
        }
    }
}
