using Mirror;

namespace Playmode.NetCommunication
{
    public interface INetMessage : NetworkMessage
    {
        public void SendToServer();
        public void SendToClient();
    }
}
