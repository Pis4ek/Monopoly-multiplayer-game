using Mirror;

namespace Playmode.NetCommunication
{
    public struct PrisonNetMessage : INetMessage
    {
        public PlayerID ByWho;
        public bool IsTryEscape;

        public PrisonNetMessage(PlayerID byWho, bool isTryEscape)
        {
            ByWho = byWho;
            IsTryEscape = isTryEscape;
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
