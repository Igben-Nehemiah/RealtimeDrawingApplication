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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for MenuPaneView.xaml
    /// </summary>
    public partial class MenuPaneView : UserControl
    {
        private CreateAccountWindowView CreateAccountWindowView { get; set; }
        private CreateProjectWindowView CreateProjectWindowView { get; set; }
        private ShareProjectWindowView ShareProjectWindowView { get; set; }

        public MenuPaneView()
        {
            InitializeComponent();
        }

        private void CreateAccount_Click(object sender, MouseButtonEventArgs e)
        {
            if (CreateAccountWindowView != null) { CreateAccountWindowView = null; }
            CreateAccountWindowView = new CreateAccountWindowView();
            CreateAccountWindowView.Show();
        }

        private void CreateProject_Click(object sender, MouseButtonEventArgs e)
        {
            if (CreateProjectWindowView != null) { CreateProjectWindowView = null; }
            CreateProjectWindowView = new CreateProjectWindowView();
            CreateProjectWindowView.Show();
        }

        private void ShareProject_Click(object sender, MouseButtonEventArgs e)
        {
            if (ShareProjectWindowView != null) { ShareProjectWindowView = null; }
            ShareProjectWindowView = new ShareProjectWindowView();
            ShareProjectWindowView.Show();
        }

        private void MenuPaneViewClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
