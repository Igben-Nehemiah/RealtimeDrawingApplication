using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFUserInterface.Core;
using Prism.Ioc;
using WPFGraphicUserInterface.Services;

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for LoginViewWindow.xaml
    /// </summary>
    public partial class LoginWindowView : Window
    {
        IEventAggregator _eventAggregator;
        bool isValidUser;
        public LoginWindowView()
        {
            _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<SignInStatusEvent>().Subscribe(SetIsValidUser);
            isValidUser = false;
            InitializeComponent();
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            var _createAccountView = new CreateAccountWindowView();
            this.Close();
            _createAccountView.ShowDialog();
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            //Raise TriedToSignInEvent 
            _eventAggregator.GetEvent<TriedToSignInEvent>().Publish();
            //Check for SignInStatusEvent
            //Close view
            if (isValidUser)
            {
                this.Close();
            }
            //Do something
        }

        private void SetIsValidUser(bool pIsValidUser)
        {
            isValidUser = pIsValidUser;
        }
    }
}
