using LDPDatapoints.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Subscriptions
{
    public interface ISubscription
    {
        void SendMessage(string message);
        void SendData(byte[] data);
    }
}
