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
                UserPassword = "Bart Allen"
            };

            var User2 = new UserProxy()
            {
                UserFirstName = "Sarah",
                UserLastName = "Allen",
                UserEmailAddress = "sarahallen@gmail.com",
                UserPassword = "Sarah Allen"
            };

            generatedUsers.Add(User1);
            generatedUsers.Add(User2);
            return generatedUsers;
        }
    }
}
