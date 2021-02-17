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
        public CreateAccountWindowView CreateAccountWindowView { get; set; }
        public CreateProjectWindowView CreateProjectWindowView { get; set; }
        public ShareProjectWindowView ShareProjectWindowView { get; set; }

        public MenuPaneView()
        {
            InitializeComponent();
        }

        public void CreateAccount_Click(object sender, MouseButtonEventArgs e)
        {
            if (CreateAccountWindowView != null) { CreateAccountWindowView = null; }
            CreateAccountWindowView = new CreateAccountWindowView();
            CreateAccountWindowView.Show();
        }

        public void CreateProject_Click(object sender, MouseButtonEventArgs e)
        {
            if (CreateProjectWindowView != null) { CreateProjectWindowView = null; }
            CreateProjectWindowView = new CreateProjectWindowView();
            CreateProjectWindowView.Show();
        }

        public void ShareProject_Click(object sender, MouseButtonEventArgs e)
        {
            if (ShareProjectWindowView != null) { ShareProjectWindowView = null; }
            ShareProjectWindowView = new ShareProjectWindowView();
            ShareProjectWindowView.Show();
        }

        public void MenuPaneViewClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
