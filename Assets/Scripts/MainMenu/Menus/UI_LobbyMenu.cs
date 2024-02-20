using UnityEngine;
using UnityEngine.UI;
using Other.Network.Lobby;
using UniRx;
using MainMenu.StateMachine;
using Zenject;
using Mirror;
using TMPro;
using Other.Network;

namespace MainMenu
{
    public class UI_LobbyMenu : UI_MenuElement
    {
        [Inject] MainMenuStateMachine _stateMachine;
        [Inject] NetworkAdapter _networkAdapter;
        [Inject] MyNetworkDiscovery _networkDiscovery;

        [SerializeField] ScrollList<ClientData> _scrollList;
        [SerializeField] Lobby lobby;

        [Space(20)]
        [SerializeField] Button _exit;
        [SerializeField] Button _readyStart;

        private void Start()
        {
            _scrollList.AddHandler = (ClientData item, RectTransform content, GameObject prefab) =>
            {
                UI_PlayerField playerField = Instantiate(prefab, content).GetComponent<UI_PlayerField>();
                playerField.UpdateData(item.Nickname, item.Image);

                return playerField.gameObject;
            };

            lobby.Clients.ObserveAdd().Subscribe((CollectionAddEvent<ClientData> addEvent) => 
                _scrollList.Add(addEvent.Value));
            lobby.Clients.ObserveRemove().Subscribe((CollectionRemoveEvent<ClientData> removeEvent) => 
                _scrollList.RemoveAt(removeEvent.Index));

            _exit.onClick.AddListener(OnExitButton);
        }

        public override void Enter() 
        {
            this.Activate();
            if (NetworkServer.active)
            {
                _readyStart.gameObject.GetComponentInChildren<TMP_Text>().text = "Start";
                _readyStart.onClick.RemoveAllListeners();
                _readyStart.onClick.AddListener(OnButtonStart);
            }
            else 
            {
                _readyStart.gameObject.GetComponentInChildren<TMP_Text>().text = "Ready";
                _readyStart.onClick.RemoveAllListeners();
                _readyStart.onClick.AddListener(OnButtonReady);
            }

            if (!NetworkServer.active)
                _networkDiscovery.StopDiscovery();
        }

        public void OnExitButton() 
        {
            _stateMachine.SwitchState<UI_NetworkModes>();
            if (NetworkClient.activeHost)
                NetworkManager.singleton.StopHost();
            else if(NetworkClient.active)
                NetworkManager.singleton.StopClient();

            _networkDiscovery.StopDiscovery();
            _scrollList.Clear();
        }

        private void OnButtonStart() 
        {
            _networkDiscovery.StopDiscovery();
            _networkAdapter.StartGame();
        }

        private void OnButtonReady() 
        {

        }
    }
}
