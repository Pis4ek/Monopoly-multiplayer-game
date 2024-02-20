using Mirror;

namespace Playmode.NetCommunication
{
    public struct BuyOrAuctionNetMessage : INetMessage
    {
        public PlayerID ByWho;
        public bool Buy;

        public BuyOrAuctionNetMessage(PlayerID byWho, bool buy)
        {
            ByWho = byWho;
            Buy = buy;
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
