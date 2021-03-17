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
        public CreateProjectWindowView()
        {
            InitializeComponent();
            ProjectNameTxtbox.Focus();
        }
    }
}
