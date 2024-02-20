using Mirror;

namespace Other.Network.Lobby
{
    public struct RemoveClientData : NetworkMessage
    {
        public string Nickname;

        public RemoveClientData(string nickname)
        {
            Nickname = nickname;
        }
    }
}
