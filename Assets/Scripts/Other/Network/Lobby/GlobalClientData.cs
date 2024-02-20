using UnityEngine;

namespace Other.Network.Lobby
{
    public class GlobalClientData
    {
        public Texture2D Image { get; private set; }
        public string Nickname { get; private set; }

        public GlobalClientData(Texture2D image, string nickname)
        {
            Image = image;
            Nickname = nickname;
        }

        public void ChangeImage(Texture2D image) 
        {
            Image = image;
        }

        public void ChangeNickname(string nickname) 
        {
            Nickname = nickname;
        }
    }
}
