using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using WPFGraphicUserInterface.Views;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.ViewModels
{
    public class MenuPaneViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        
        //Create Project view
        private CreateProjectWindowView _CreateProjectWindowView;

        //Commands
        public DelegateCommand ShowCreateProjectWindowCommand { get; private set; }



        public MenuPaneViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            ShowCreateProjectWindowCommand = new DelegateCommand(ExecuteCreateProjectView, CanExecuteCreateProjectView);
        }

        private void ExecuteCreateProjectView()
        {
            _CreateProjectWindowView = new CreateProjectWindowView();
            _CreateProjectWindowView.Show();
        }

        private bool CanExecuteCreateProjectView()
        {
            return true;
        }
    }
}
