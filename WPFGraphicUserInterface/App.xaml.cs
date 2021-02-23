using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFGraphicUserInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {

        }

        protected override Window CreateShell()
        {
            ShellContainer = Container;
            return Container.Resolve<Views.StartUpWindowView>();
        }
        public static IContainerProvider ShellContainer { get; private set; }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            return;
        }
    }
}
