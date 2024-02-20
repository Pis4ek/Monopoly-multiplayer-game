using Mirror;

namespace Playmode.NetCommunication
{
    public struct GiveUpNetMessage : INetMessage
    {
        public PlayerID ByWho;

        public GiveUpNetMessage(PlayerID byWho)
        {
            ByWho = byWho;
        }

        public void SendToServer()
        {
            if (NetworkClient.active)
                NetworkClient.Send(this);
        }

        public void SendToClient()
        {
            if (NetworkServer.active)
                NetworkServer.SendToAll(this);
        }
    }
}
