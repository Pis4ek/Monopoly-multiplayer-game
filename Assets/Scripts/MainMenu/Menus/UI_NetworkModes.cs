using MainMenu.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    class UI_NetworkModes : UI_MenuElement
    {
        [Inject] MainMenuStateMachine _stateMachine;

        [SerializeField] Button _host;
        [SerializeField] Button _localSearch;
        [SerializeField] Button _ConnectOverIP;

        private void Start()
        {
            _localSearch.onClick.AddListener(OnLocalSearchClicked);
            _ConnectOverIP.onClick.AddListener(OnConnectOverIPClicked);
            _host.onClick.AddListener(OnHostClicked);
        }
        private void OnHostClicked() 
        {
            _stateMachine.SwitchState<UI_HostConfigMenu>();
        }
        private void OnLocalSearchClicked() 
        {
            _stateMachine.SwitchState<UI_LocalServerList>();
        }
        private void OnConnectOverIPClicked() 
        {

        }
    }
}
