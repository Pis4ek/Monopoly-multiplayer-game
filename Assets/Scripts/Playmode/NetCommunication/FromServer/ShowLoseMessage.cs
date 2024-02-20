using Mirror;

namespace Playmode.NetCommunication
{
    public struct ShowLoseMessage : INetMessage
    {
        public PlayerID Loser;

        public ShowLoseMessage(PlayerID loser)
        {
            Loser = loser;
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
