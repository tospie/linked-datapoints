﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Subscriptions
{
    public interface ISubscription<T>
    {
        void SendMessage(ISubscriptionMessage message);
    }
}
