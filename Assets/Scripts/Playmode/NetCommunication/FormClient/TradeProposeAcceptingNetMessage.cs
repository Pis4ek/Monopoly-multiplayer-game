using Mirror;

namespace Playmode.NetCommunication
{
    public struct TradeProposeAcceptingNetMessage : INetMessage
    {
        public PlayerID ByWho;
        public bool IsAccept;

        public TradeProposeAcceptingNetMessage(PlayerID byWho, bool isAccept)
        {
            ByWho = byWho;
            IsAccept = isAccept;
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
