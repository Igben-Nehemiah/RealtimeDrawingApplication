using Models;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFGraphicUserInterface.Services
{
    public static class ModelToProxyConverter
    {
        public static UserProxy UserModelToUserProxyConverter(User userModel)
        {
            var userProxy = new UserProxy();
            userProxy.UserFirstName = userModel.UserFirstName;
            userProxy.UserLastName = userModel.UserLastName;
            userProxy.UserEmailAddress = userModel.UserEmailAddress;
            userProxy.UserPassword = userModel.UserPassword;

            return userProxy;
        }
    }
}
