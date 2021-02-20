using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class CreateAccountWindowViewModel : BindableBase
    {
        private CreateAccountWindowView createAccountWindowView;

        public CreateAccountWindowViewModel(IEventAggregator _eventAggregator)
        {
            _eventAggregator.GetEvent<CreateProjectWindowEvent>().Subscribe(ShowCreateProjectWindow);
        }

        private void ShowCreateProjectWindow()
        {
            if (createAccountWindowView != null)
            {
                createAccountWindowView.Show();
            }
            else
            {
                createAccountWindowView = new CreateAccountWindowView();
            }
        }
    }
}
