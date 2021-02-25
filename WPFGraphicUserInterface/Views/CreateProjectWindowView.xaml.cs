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
    /// Interaction logic for CreateProjectWindowView.xaml
    /// </summary>
    public partial class CreateProjectWindowView : Window
    {
        private bool _isProjectCreated;
        IEventAggregator _eventAggregator;

        public CreateProjectWindowView()
        {
            _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<ProjectCreationStatusEvent>().Subscribe(SetIsProjectCreated);
            InitializeComponent();
        }

        private void CreateProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            //Raise TriedToCreateProjectEvent
            _eventAggregator.GetEvent<TriedToCreateProjectEvent>().Publish();
            //Subscribe to ProjectCreationStatusEvent
            

            if (_isProjectCreated)
            {
                this.Close();
            }
            //Do something
        }

        private void SetIsProjectCreated(bool pIsProjectCreated)
        {
            _isProjectCreated = pIsProjectCreated;
        }
    }
}
