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
using WPFGraphicUserInterface.Services;
using WPFGraphicUserInterface.ViewModels;

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for StartUpWindowView.xaml
    /// </summary>
    /// 
   
    public partial class StartUpWindowView : Window
    {
        public StartUpWindowView()
        {
            InitializeComponent();
        }

        private void DrawingObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement component = null;

            if (sender is Rectangle)
            {
                component = DrawingComponentService.GetDefaultComponent(ControlEnum.Rectangle);
            }
            else if (sender is Ellipse)
            {
                component = DrawingComponentService.GetDefaultComponent(ControlEnum.Ellipse);
            }
            else if (sender is Path p)
            {
                var item1 = Geometry.Parse(p.Data.ToString()).ToString();
                var item2 = DrawingComponentService.GetDefaultShapeGeometry(ControlEnum.Triangle).ToString();
                if (item1 == item2)
                {
                    component = DrawingComponentService.GetDefaultComponent(ControlEnum.Triangle);
                }
                else 
                {
                    component = DrawingComponentService.GetDefaultComponent(ControlEnum.Line);
                }
            }
            else if (sender is TextBlock)
            {
                component = DrawingComponentService.GetDefaultComponent(ControlEnum.TextBox);
            }

            if (component != null)
            {
                DataObject dataObject = new DataObject("toolboxitem", component);
                DragDrop.DoDragDrop(component, dataObject, DragDropEffects.Copy);
            }
        }
    }
}
