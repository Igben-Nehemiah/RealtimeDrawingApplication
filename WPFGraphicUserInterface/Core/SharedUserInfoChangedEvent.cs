using Prism.Events;
using System;

namespace WPFUserInterface.Core
{
    public class SharedUserInfoChangedEvent : PubSubEvent<Tuple<string, bool>>
    {

    }
}
