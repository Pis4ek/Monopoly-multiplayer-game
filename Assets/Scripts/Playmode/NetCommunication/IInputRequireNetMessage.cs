using Playmode.ServerEnteties;
using System;

namespace Playmode.NetCommunication
{
    public interface IInputRequireNetMessage : INetMessage
    {
        public InputPermissions InputPermissions { get; }
        public PlayerID Reciever { get; }
    }
}
