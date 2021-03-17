using Prism.Events;
using System;

namespace WPFUserInterface.Core
{
    public class SharedUserInfoChangedEvent : PubSubEvent<(string, bool)>
    {

    }
}
