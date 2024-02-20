using Mirror;

namespace Playmode.NetCommunication
{
    public struct ThrowCubesNetMessage : INetMessage
    {
        public PlayerID ByWho;

        public ThrowCubesNetMessage(PlayerID byWho)
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
