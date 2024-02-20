using Mirror;
using Playmode.PlayData;

namespace Playmode.NetCommunication
{
    public struct ShowLogNetMessage : INetMessage
    {
        public Log Log;

        public ShowLogNetMessage(Log log)
        {
            Log = log;
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
