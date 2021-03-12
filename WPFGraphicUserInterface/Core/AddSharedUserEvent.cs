﻿using Prism.Events;
using System;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFUserInterface.Core
{
    public class AddSharedUserEvent : PubSubEvent<Tuple<UserProxy, bool>>
    {
    }
}
