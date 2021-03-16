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
using WPFGraphicUserInterface.Services;
using WPFGraphicUserInterface.ViewModels;
using Prism.Ioc;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.Views
{
    /// <summary>
    /// Interaction logic for StartUpWindowView.xaml
    /// </summary>
    /// 
   
    public partial class StartUpWindowView : Window
    {
        private IEventAggregator _eventAggregator;
        public StartUpWindowView()
        {
            InitializeComponent();
            _eventAggregator = App.ShellContainer.Resolve<IEventAggregator>();

            _eventAggregator.GetEvent<SigningOutEvent>().Subscribe(SignOut);
        }

        private void SignOut()
        {
            //var logInView = new LoginWindowView();
            //logInView.DataContext = new LoginWindowViewModel(_eventAggregator);

            Close();
        }

        //private void SaveProjectShortCut(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
        //    {
        //        _eventAggregator.GetEvent<SaveProjectEvent>().Publish();
        //    }
        //}

        //private void DeleteProjectShortCut(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.D & Keyboard.Modifiers == ModifierKeys.Control)
        //    {
        //        _eventAggregator.GetEvent<DeleteProjectBtnClickEvent>().Publish();
        //    }
        //}

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
