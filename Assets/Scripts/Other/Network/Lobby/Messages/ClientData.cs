using Mirror;
using UnityEngine;

namespace Other.Network.Lobby
{
    public struct ClientData : NetworkMessage
    {
        public Texture2D Image;
        public string Nickname;

        public ClientData(Texture2D image, string nickname)
        {
            Image = image;
            Nickname = nickname;
        }
    }
}
