using Mirror;

namespace Playmode.NetCommunication
{
    public struct ShowCubesThrowNetMessage : INetMessage
    {
        public ThrowCubesResult Result;

        public ShowCubesThrowNetMessage(ThrowCubesResult result)
        {
            Result = result;
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
