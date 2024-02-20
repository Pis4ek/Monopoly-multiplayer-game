using Playmode.NetCommunication;
using Playmode.PlayData;
using Zenject;
using UnityEngine;
using Mirror;
using Playmode.ServerEnteties;
using Other;
using Playmode.CommandSystem;

namespace Playmode.Installers
{
    public class ServerInstaller : MonoInstaller
    {
        [SerializeField] bool _debugMode = false;

        [Inject]
        private LastGameClientsSession gameInfo;

        public override void InstallBindings()
        {
            if (gameInfo.PlayersCount == 0)
            {
                gameInfo.PlayerID = PlayerID.Player1;
                gameInfo.PlayersCount = 2;
            }
            if (NetworkServer.active || _debugMode)
            {
                Container.Bind<GameData>().AsCached().WithArguments(gameInfo.PlayersCount);
                Container.Bind<NetMessageSender>().WithId("Server").AsCached();

                Container.BindInterfacesAndSelfTo<CommandHandler>().AsSingle();
                Container.BindInterfacesAndSelfTo<UpdatingDataCollector>().AsSingle(); 
                Container.BindInterfacesAndSelfTo<MessageWaiter>().AsSingle();

                Container.BindInterfacesAndSelfTo<ServerStateMachine>().AsSingle();
                Container.BindInterfacesAndSelfTo<Server>().AsSingle();
            }
        }
    }
}