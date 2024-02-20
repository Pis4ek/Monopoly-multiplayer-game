using Mirror;
using System;

namespace Playmode.NetCommunication
{
    public struct SetTimerNetMessage : INetMessage
    {
        public DateTime EndTime;
        public PlayerID Player;

        public SetTimerNetMessage(DateTime endTime, PlayerID player)
        {
            EndTime = endTime;
            Player = player;
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
