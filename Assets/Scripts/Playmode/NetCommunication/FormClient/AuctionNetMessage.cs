using Mirror;

namespace Playmode.NetCommunication
{
    public struct AuctionNetMessage : INetMessage
    {
        public PlayerID ByWho;
        public bool IsStakeIncreased;

        public AuctionNetMessage(PlayerID byWho, bool isStakeIncreased)
        {
            ByWho = byWho;
            IsStakeIncreased = isStakeIncreased;
        }

        public void SendToServer()
        {
            if(NetworkClient.active)
                NetworkClient.Send(this);
        }

        public void SendToClient()
        {
            if (NetworkServer.active)
                NetworkServer.SendToAll(this);
        }
    }
}
