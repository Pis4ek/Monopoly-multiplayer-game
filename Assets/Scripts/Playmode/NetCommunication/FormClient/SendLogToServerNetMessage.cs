using Mirror;

namespace Playmode.NetCommunication
{
    public struct SendLogToServerNetMessage : INetMessage
    {
        public PlayerID Author;
        public string Text;

        public SendLogToServerNetMessage(PlayerID author, string text) : this()
        {
            Author = author;
            Text = text;
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
