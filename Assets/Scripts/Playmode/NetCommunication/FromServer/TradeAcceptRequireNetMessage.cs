using Mirror;
using Playmode.ServerEnteties;
using System;

namespace Playmode.NetCommunication
{
    public struct TradeAcceptRequireNetMessage : IInputRequireNetMessage
    {
        public InputPermissions InputPermissions => inputPermissions;
        public PlayerID Reciever => reciever;

        public InputPermissions inputPermissions;
        public PlayerID reciever;
        public TradeOfferInfo tradeOffer;

        public TradeAcceptRequireNetMessage(PlayerID reciever, InputPermissions permissions, 
            TradeOfferInfo tradeOfferInfo)
        {
            this.reciever = reciever;
            inputPermissions = permissions;
            tradeOffer = tradeOfferInfo;
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
