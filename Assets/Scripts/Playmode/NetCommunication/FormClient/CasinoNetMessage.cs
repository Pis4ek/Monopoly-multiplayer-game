using Mirror;

namespace Playmode.NetCommunication
{
    public struct CasinoNetMessage : INetMessage
    {
        public PlayerID ByWho;

        public CasinoNetMessage(PlayerID byWho)
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
