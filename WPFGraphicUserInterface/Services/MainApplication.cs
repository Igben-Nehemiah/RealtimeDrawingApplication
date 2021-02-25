using WPFGraphicUserInterface.Views;

namespace WPFGraphicUserInterface.Services
{
    public static class MainApplication
    {
        public static StartUpWindowView CreateUserWindow()
        {
            var startUpWindow = new StartUpWindowView();
            startUpWindow.Show();
            return startUpWindow;
        }
    }
}
