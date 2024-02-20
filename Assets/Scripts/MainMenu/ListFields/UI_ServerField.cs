using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Other.Network.Discovery;
using UnityEngine.EventSystems;
using Mirror;
using MainMenu.StateMachine;

namespace MainMenu 
{
    class UI_ServerField : MonoBehaviour , IPointerClickHandler
    {
        [SerializeField] TMP_Text _serverName;
        [SerializeField] TMP_Text _lobbySize;
        [SerializeField] Image _hasPassword;

        private ServerRes _serverReq;
        private MainMenuStateMachine _stateMachine;

        public void ChangeData(ServerRes serverRes, MainMenuStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _serverReq = serverRes;
            _serverName.text = serverRes.serverName;
            _lobbySize.text = $"{serverRes.clientsInLobby}/{serverRes.lobbySize}";
            _hasPassword.gameObject.SetActive(serverRes.hasPassword);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            NetworkManager.singleton.StartClient(_serverReq.uri);
            _stateMachine.SwitchState<UI_LobbyMenu>();
        }
    }
}
