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
using WPFGraphicUserInterface.ViewModels;

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for StartUpWindowView.xaml
    /// </summary>
    public partial class StartUpWindowView : Window
    {
        public LoginWindowView loginViewWindow { get; set; }
        //private CreateAccountWindowView _loginViewWindow { get; set; }
        //public MenuPaneView menuPaneView { get; set; } = new MenuPaneView();

        public StartUpWindowView()
        {
            InitializeComponent();
        }

        //private void ShowLogInPage(object sender, RoutedEventArgs e)
        //{
        //    loginViewWindow = new LoginWindowView();
        //    loginViewWindow.Topmost = true;
        //    loginViewWindow.ShowDialog();
        //}
    }
}
