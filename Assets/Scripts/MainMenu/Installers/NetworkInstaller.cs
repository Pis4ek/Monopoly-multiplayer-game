using Zenject;
using UnityEngine;
using Other.Network;
using Other.Network.Lobby;
using UnityEngine.UI;

namespace MainMenu.Installers
{
    class NetworkInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] PopUpMessage _popUpPrefab;
        [SerializeField] RectTransform _UIcanvas;

        [Header("NetworkClasses")]
        [SerializeField] NetworkAdapter _networkAdapter;
        [SerializeField] MyNetworkDiscovery _networkDiscovery;
        [SerializeField] CustomAuthenticator _authenticator;
        [SerializeField] Lobby _lobby;

        [Header("Debug PlayerData")]
        [SerializeField] Texture2D Image;
        [SerializeField] string Nickname;
        public override void InstallBindings() 
        {
            Container.Bind<NetworkAdapter>().FromInstance(_networkAdapter).AsSingle();
            Container.Bind<MyNetworkDiscovery>().FromInstance(_networkDiscovery).AsSingle();
            Container.Bind<CustomAuthenticator>().FromInstance(_authenticator).AsSingle();
            Container.Bind<Lobby>().FromInstance(_lobby).AsSingle();

            //Debug
            Container.Bind<GlobalClientData>().FromInstance(new GlobalClientData(Image, Nickname)).AsSingle().NonLazy();
            Container.Bind<PopUpMessage>().FromInstance(_popUpPrefab).AsSingle();
            Container.Bind<RectTransform>().FromInstance(_UIcanvas).AsSingle();
        }
    }
}
