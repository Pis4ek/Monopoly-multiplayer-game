using Mirror;
using System;

namespace Playmode.NetCommunication
{
    public interface INetMember
    {
        public NetMessageSender MessageSender { get; }

        public void Recieve(NetworkMessage message);
    }
}
