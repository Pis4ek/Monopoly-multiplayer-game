using Mirror;

namespace Playmode.NetCommunication
{
    public struct TradeProposeNetMessage : INetMessage
    {
        public PlayerID ByWho;
        public TradeOfferInfo TradeOffer;

        public TradeProposeNetMessage(PlayerID byWho, TradeOfferInfo tradeOffer)
        {
            ByWho = byWho;
            TradeOffer = tradeOffer;
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
