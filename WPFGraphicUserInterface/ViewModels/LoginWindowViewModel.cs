﻿using Prism.Commands;
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

namespace WPFGraphicUserInterface.ViewModels
{
    public class LoginWindowViewModel : BindableBase
    {
        //private StartUpWindowViewModel _startUpWindowViewModel;
        private UserProxy _user;
       
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
        //private bool CanLogin()
        //{
        //    return true;
        //}
        private void Login()
        {
            //Throw LoggedInEvent to StartUpWindowViewModel
            if (IsValidUser()) 
            {
                var startUpWindowView = new StartUpWindowView();

                _eventAggregator.GetEvent<UserLoggedInEvent>().Publish(User);

                startUpWindowView.Show();
            }
            _eventAggregator.GetEvent<SignInStatusEvent>().Publish(IsValidUser());
        }
        private bool IsValidUser()
        {
            //Perform the test here
            var userFetchedFromDatabase = SearchDataBaseForUser(UserEmail);
            if (userFetchedFromDatabase != null)
            {
                bool IsValidUser = ValidateUserPassword(userFetchedFromDatabase);
                return IsValidUser;
            }
            return false;
        }
        private bool ValidateUserPassword(UserProxy userFetchedFromDatabase)
        {
            var databasePassword = userFetchedFromDatabase.UserPassword;
            if (databasePassword == UserPassword) { return true; }
            return false;
        }
        private UserProxy SearchDataBaseForUser(object userEmail)
        {
            var database = UserProxyProvider.GenerateUsers();
            foreach(var user in database)
            {
                if(user.UserEmailAddress == UserEmail)
                {
                    User = user;
                    return user;
                }
            }
            return null;
        }
    }
}
