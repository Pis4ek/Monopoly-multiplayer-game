using Mirror;
using Playmode.ServerEnteties;
using System;

namespace Playmode.NetCommunication
{
    public struct ForfeitRequireNetMessage : IInputRequireNetMessage
    {
        public InputPermissions InputPermissions => inputPermissions;
        public PlayerID Reciever => reciever;

        public InputPermissions inputPermissions;
        public PlayerID reciever;
        public int cashToPay;

        public ForfeitRequireNetMessage(PlayerID reciever, InputPermissions permissions, 
            int cashToPay)
        {
            inputPermissions = permissions;
            this.reciever = reciever;
            this.cashToPay = cashToPay;
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
