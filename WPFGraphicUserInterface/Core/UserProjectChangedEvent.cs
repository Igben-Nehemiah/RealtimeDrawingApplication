using Prism.Events;
using System.Collections.Generic;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFUserInterface.Core
{
    public class UserProjectChangedEvent : PubSubEvent<ICollection<ProjectProxy>>
    {
    }
}
