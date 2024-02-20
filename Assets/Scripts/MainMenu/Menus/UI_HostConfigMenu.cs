using MainMenu.StateMachine;
using Mirror;
using Other.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    class UI_HostConfigMenu : UI_MenuElement
    {
        [Inject] MainMenuStateMachine _stateMachine;
        [Inject] MyNetworkDiscovery networkDiscovery;
        [Inject] CustomAuthenticator authenticator;

        [SerializeField] TMP_InputField _serverName;
        [SerializeField] TMP_InputField _serverPassword;
        [SerializeField] Toggle _hasPassword;
        [SerializeField] Button _exit;
        [SerializeField] Button _start;

        bool hasPassword = false;

        private void Start()
        {
            _start.onClick.AddListener(OnStartClicked);
            _exit.onClick.AddListener(OnExitClicked);
            _hasPassword.onValueChanged.AddListener(OnHasPasswordClicked);
        }

        private void OnHasPasswordClicked(bool isEnable) 
        {
            hasPassword = isEnable;
            _serverPassword.interactable = isEnable;
            if (!isEnable)
                _serverPassword.text = "";
        }
        private void OnExitClicked() 
        {
            _stateMachine.SwitchState<UI_NetworkModes>();
        }
        private void OnStartClicked() 
        {
            authenticator.serverUsername = _serverName.text;
            authenticator.serverPassword = _serverPassword.text;
            authenticator.password = _serverPassword.text;
            authenticator.hasPassword = hasPassword;
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();

            _stateMachine.SwitchState<UI_LobbyMenu>();
        }
    }
}
