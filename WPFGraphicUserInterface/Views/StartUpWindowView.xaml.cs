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

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for StartUpWindowView.xaml
    /// </summary>
    public partial class StartUpWindowView : Window
    {
        private ShareProjectWindowView ShareProjectWindowView { get; set; }

        private FrameworkElement RightMenuPane { get; set; } = new SharedUsersPaneView();

        public StartUpWindowView()
        {
            InitializeComponent();
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuPaneView.Visibility = Visibility.Visible;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuPaneView.Visibility = Visibility.Collapsed;
        }

        private void AddUserIcon_Click(object sender, MouseButtonEventArgs e)
        {
            if (ShareProjectWindowView != null) { ShareProjectWindowView = null; }
            ShareProjectWindowView = new ShareProjectWindowView();
            ShareProjectWindowView.Show();
        }

        private void OnClick(object sender, MouseButtonEventArgs eventArgs)
        {

        }

        private void DoctorWho_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public enum PaneWindow
    {
        SharedUserPaneWindow,
        ProjectsPaneWindow,
        PropertyPaneWindow
    }
}
