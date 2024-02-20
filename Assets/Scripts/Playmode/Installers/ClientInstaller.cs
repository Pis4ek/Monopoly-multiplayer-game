using Other;
using Playmode.NetCommunication;
using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using Playmode.View;
using UnityEngine;
using Zenject;

namespace Playmode.Installers
{
    public class ClientInstaller : MonoInstaller
    {
        [SerializeField] bool _debugMode = false;
        [SerializeField] PlayersWindow _playersWindow;
        [SerializeField] GameMapWindow _gameMapWindow;
        [SerializeField] MiddleWindow _middleWindow;
        [SerializeField] CubeResultShower _cubeResultShower;
        //[SerializeField] InputHandler _inputHandler;

        [Header("Context menus")]
        [SerializeField] PlayerContextMenu _playerContextMenu;
        [SerializeField] MapContextMenu _mapContextMenu;

        [Header ("Input windows")]
        [SerializeField] BuyOrAuctionInputWindow _buyOrAuctionInputWindow;
        [SerializeField] ThrowCubesInputWindow _throwCubesInputWindow;
        [SerializeField] AuctionInputWindow _auctionInputWindow;
        [SerializeField] PrisonInputWindow _prisonInputWindow;
        [SerializeField] ForfeitInputWindow _forfeitInputWindow;
        [SerializeField] TradeProposingInputWindow _tradeProposingInputWindow;

        [Inject] 
        private LastGameClientsSession gameInfo;

        public override void InstallBindings()
        {
            if (gameInfo.PlayersCount == 0)
            {
                gameInfo.PlayerID = PlayerID.Player1;
                gameInfo.PlayersCount = 2;
            }
            Container.Bind<UIFactory>().AsSingle();
            Container.Bind<ViewInputStateMachine>().AsSingle(); 

            Container.Bind<ClientsGameData>().AsCached();

            BindContextMenus();
            BindInputWindows();
            Container.BindInstance(_playersWindow).AsSingle();
            Container.BindInterfacesAndSelfTo<GameMapWindow>().FromInstance(_gameMapWindow).AsSingle();
            Container.BindInterfacesAndSelfTo<CubeResultShower>().FromInstance(_cubeResultShower).AsSingle();

            Container.Bind<MiddleWindow>().FromInstance(_middleWindow).AsSingle();
            //Container.Bind<InputHandler>().FromInstance(_inputHandler).AsSingle();
            Container.Bind<InputHandler>().AsSingle();
            Container.Bind<PlaymodeView>().AsSingle();
            Container.Bind<AnimationQueue>().AsSingle();

            Container.Bind<NetMessageSender>().AsCached();
            Container.Bind<Client>().AsSingle().WithArguments(_debugMode);
        }

        private void BindContextMenus()
        {
            Container.BindInterfacesAndSelfTo<PlayerContextMenu>()
                .FromInstance(_playerContextMenu)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<MapContextMenu>()
                .FromInstance(_mapContextMenu)
                .AsSingle();
        }

        private void BindInputWindows()
        {
            Container.BindInterfacesAndSelfTo<BuyOrAuctionInputWindow>()
                .FromInstance(_buyOrAuctionInputWindow)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<ThrowCubesInputWindow>()
                .FromInstance(_throwCubesInputWindow)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<AuctionInputWindow>()
                .FromInstance(_auctionInputWindow)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<PrisonInputWindow>()
               .FromInstance(_prisonInputWindow)
               .AsSingle();
            Container.BindInterfacesAndSelfTo<ForfeitInputWindow>()
               .FromInstance(_forfeitInputWindow)
               .AsSingle();
            Container.BindInterfacesAndSelfTo<TradeProposingInputWindow>()
               .FromInstance(_tradeProposingInputWindow)
               .AsSingle();
        }
    }
}