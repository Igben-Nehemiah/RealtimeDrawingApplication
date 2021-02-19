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
using System.Xaml;

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for SharedUsersPaneView.xaml
    /// </summary>
    public partial class SharedUsersPaneView : UserControl
    {
        public bool IsPathUp { get; set; } = false;
        public SharedUsersPaneView()
        {
            InitializeComponent();
        }

        private void UpPath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pathUpData = Geometry.Parse("M7.41,15.41L12,10.83L16.59,15.41L18,14L12,8L6,14L7.41,15.41Z");
            var pathDownData = Geometry.Parse("m 150,0 A 150,0 0 0 0 150,0 A 150,150 0 0 0 150,0");
            var currentPath = Geometry.Parse("M7.41,15.41L12,10.83L16.59,15.41L18,14L12,8L6,14L7.41,15.41Z");

            if (IsPathUp == false)
            {
                IsPathUp = true;
                currentPath = pathUpData;
                //var newPath = new Path();
                //newPath.Data = "M7.41,8.58L12,13.17L16.59,8.58L18,10L12,16L6,10L7.41,8.58Z";
                //SharedUsersWindowPath = newPath;
                //Show pop up

                //newPath.Data = Geometry.Parse( "m 150,0 A 150,0 0 0 0 150,0 A 150,150 0 0 0 150,0");
                //SharedUsersWindowPath.Data = newPath.Data;
                //PaneOptionsPopUp.IsOpen = true;
            }
            else
            {
                //Remove pop up
                IsPathUp = false;
                currentPath = pathDownData;
                //var newPath = new Path();
                ////newPath.Data = "M7.41,8.58L12,13.17L16.59,8.58L18,10L12,16L6,10L7.41,8.58Z";
                ////SharedUsersWindowPath = newPath;
                ////Show pop up

                //newPath.Data = Geometry.Parse("M7.41,15.41L12,10.83L16.59,15.41L18,14L12,8L6,14L7.41,15.41Z");
                //SharedUsersWindowPath.Data = newPath.Data;
                PaneOptionsPopUp.IsOpen = false;
            }
        }
    }
}
