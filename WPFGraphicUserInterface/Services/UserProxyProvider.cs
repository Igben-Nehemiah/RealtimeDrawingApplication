using System;
using System.Collections.Generic;
using System.Text;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFGraphicUserInterface.Services
{
    public static class UserProxyProvider
    {
        public static IList<UserProxy> GenerateUsers()
        {
            var generatedUsers = new List<UserProxy>();
            var User1 = new UserProxy()
            {
                UserFirstName = "Obomaese",
                UserLastName = "Igben",
                UserEmailAddress = "igbennehemiah@gmail.com",
                UserId = 1,
                UserPassword = "Bart Allen"
            };
            generatedUsers.Add(User1);
            return generatedUsers;
        }
    }
}
