using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using UnityEngine;
using UnityEngine.UI;

namespace Playmode.View
{
    public class TradeMemberInfoPanel : MonoBehaviour
    {
        [SerializeField] Image _avatarIcon;
        [SerializeField] Text _nickNameText;

        public PlayerID PlayerID { get; private set; }

        public void SetPlayer(ClientsPlayer player)
        {
            _nickNameText.text = player.Name;
            PlayerID = player.ID;
        }
    }
}