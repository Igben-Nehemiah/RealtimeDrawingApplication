using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFUserInterface.Core
{
    public class UserLoggedInEvent : PubSubEvent<UserProxy>
    {
    }
}
