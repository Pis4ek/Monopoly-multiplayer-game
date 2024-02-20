using Zenject;
using TMPro;
using UnityEngine.UI;
using Other.Network;
using UnityEngine;
using MainMenu.StateMachine;
using Mirror;

namespace MainMenu
{
    class UI_EnterPassword : UI_MenuElement
    {
        [Inject] CustomAuthenticator _authenticator;
        [Inject] MainMenuStateMachine _stateMachine;

        [SerializeField] Button _exit;
        [SerializeField] Button _connect;
        [SerializeField] TMP_InputField _password;

        private void Start()
        {
            _exit.onClick.AddListener(OnExit);
            _connect.onClick.AddListener(OnConnect);
        }

        private void OnExit()
        {
            NetworkClient.Disconnect();
            _password.text = "";
            _stateMachine.SwitchState<UI_NetworkModes>();
        }

        private void OnConnect()
        {
            _authenticator.password = _password.text;
            _password.text = "";
            _authenticator.requestPlayerPassword = true;
            Debug.Log($"Entered pass: {_authenticator.password}");
            _authenticator.OnClientAuthenticate();
            _stateMachine.SwitchState<UI_LobbyMenu>();
        }
    }
}
