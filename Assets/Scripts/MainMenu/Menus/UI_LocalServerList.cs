using MainMenu.StateMachine;
using Other.Network.Discovery;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Other.Network;

namespace MainMenu
{
    class UI_LocalServerList : UI_MenuElement
    {
        [Inject] MyNetworkDiscovery _networkDiscovery;
        [Inject] MainMenuStateMachine _stateMachine;

        [SerializeField] ScrollList<ServerRes> _servers;
        [SerializeField] Button _exit;
        [SerializeField] Button _host;

        readonly Dictionary<long, ServerRes> discoveredServers = new();

        private void Start()
        {
            _exit.onClick.AddListener(OnExitClicked);
            _host.onClick.AddListener(OnHostClicked);

            _servers.AddHandler = (ServerRes item, RectTransform content, GameObject prefab) =>
            {
                UI_ServerField serverField = Instantiate(prefab, content).GetComponent<UI_ServerField>();

                serverField.ChangeData(item, _stateMachine);

                return serverField.gameObject;
            };

            _networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);
        }
        private void OnExitClicked() 
        {
            _stateMachine.SwitchState<UI_NetworkModes>();
        }
        private void OnHostClicked() 
        {
            _stateMachine.SwitchState<UI_HostConfigMenu>();
        }

        public void OnDiscoveredServer(ServerRes info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            //Debug.Log($"uri:{info.uri} serverID:{info.serverId}");
            if (!discoveredServers.ContainsKey(info.serverId))
            {
                discoveredServers[info.serverId] = info;
                _servers.Add(info);
            }
        }

        public override void Enter() 
        {
            this.Activate();

            _networkDiscovery.StartDiscovery();
        }

        public override void Exit()
        {
            this.Disactivate();

            _networkDiscovery.StopDiscovery();
            _servers.Clear();
            discoveredServers.Clear();
        }
    }
}
