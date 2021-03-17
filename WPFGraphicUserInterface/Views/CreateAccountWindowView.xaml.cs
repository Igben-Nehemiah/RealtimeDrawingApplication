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
using Prism.Ioc;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for CreateAccountWindowView.xaml
    /// </summary>
    public partial class CreateAccountWindowView : Window
    {
        IEventAggregator _eventAggregator;
        public CreateAccountWindowView()
        {
            _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<AccountCreationStatusEvent>().Subscribe(SetAccount);

            InitializeComponent();
        }

        private void SetAccount(bool isAcccountCreated)
        {
            if (isAcccountCreated)
            {
                this.Close();
            }
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            var logInWindow = new LoginWindowView();
            this.Close();
            Close();
            logInWindow.Show();
        }
    }
}
