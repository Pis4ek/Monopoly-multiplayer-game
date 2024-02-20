using Mirror;
using System;

namespace Playmode.NetCommunication
{
    public class NetMessageSender
    {
        public event Action<NetworkMessage> OnCallInvoked;

        public void SendMessage(NetworkMessage message)
        {
            OnCallInvoked?.Invoke(message);
        }
    }
}
