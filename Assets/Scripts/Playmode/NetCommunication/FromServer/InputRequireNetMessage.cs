using Mirror;
using Playmode.ServerEnteties;
using System;

namespace Playmode.NetCommunication
{
    public struct InputRequireNetMessage : IInputRequireNetMessage
    {
        public InputPermissions InputPermissions => inputPermissions;
        public PlayerID Reciever => reciever;

        public InputPermissions inputPermissions;
        public PlayerID reciever;

        public InputRequireNetMessage(PlayerID reciever, InputPermissions permissions)
        {
            this.inputPermissions = permissions;
            this.reciever = reciever;
        }

        public void SendToServer()
        {
            NetworkClient.Send(this);
        }

        public void SendToClient()
        {
            NetworkServer.SendToAll(this);
        }
    }
}
