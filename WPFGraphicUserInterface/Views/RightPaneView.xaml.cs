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
    /// Interaction logic for RightMenuPaneView.xaml
    /// </summary>
    public partial class RightPaneView : UserControl
    {
        public RightPaneView()
        {
            InitializeComponent();
        }

        private void CollapseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
