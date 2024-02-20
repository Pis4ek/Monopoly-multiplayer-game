using Mirror;

namespace Playmode.NetCommunication
{
    public struct ForfeitNetMessage : INetMessage
    {
        public PlayerID ByWho;

        public ForfeitNetMessage(PlayerID byWho)
        {
            ByWho = byWho;
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
