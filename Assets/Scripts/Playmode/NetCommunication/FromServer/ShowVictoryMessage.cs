using Mirror;

namespace Playmode.NetCommunication
{
    public struct ShowVictoryMessage : INetMessage
    {
        public PlayerID Winner;

        public ShowVictoryMessage(PlayerID winner)
        {
            Winner = winner;
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
