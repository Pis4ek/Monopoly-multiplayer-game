using Zenject;
using Mirror;
using UnityEngine;

namespace Playmode.NetCommunication
{
    public class NetMessageProvaider : MonoBehaviour
    {
        [SerializeField] bool _debugMode = false;

        [InjectOptional] private Client _client;
        [InjectOptional] private Server _server;

        public void Awake()
        {
            if (NetworkClient.active)
            {
                _client.MessageSender.OnCallInvoked += SendMessageToServer;
                NetworkClient.RegisterHandler<InputRequireNetMessage>(RegistrateForClient);
                NetworkClient.RegisterHandler<TradeAcceptRequireNetMessage>(RegistrateForClient);
                NetworkClient.RegisterHandler<UpdateGameDataNetMessage>(RegistrateForClient);
                NetworkClient.RegisterHandler<ForfeitRequireNetMessage>(RegistrateForClient);
                NetworkClient.RegisterHandler<ShowCubesThrowNetMessage>(RegistrateForClient);
                NetworkClient.RegisterHandler<SetTimerNetMessage>(RegistrateForClient);
                NetworkClient.RegisterHandler<ShowLoseMessage>(RegistrateForClient);
                NetworkClient.RegisterHandler<ShowVictoryMessage>(RegistrateForClient);
                NetworkClient.RegisterHandler<ShowLogNetMessage>(RegistrateForClient);
            }
            else if (_debugMode)
            {
                _client.MessageSender.OnCallInvoked += _server.Recieve;
            }
            if (NetworkServer.active) 
            {
                _server.MessageSender.OnCallInvoked += SendMessageToClients;
                NetworkServer.RegisterHandler<AuctionNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<BuyOrAuctionNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<CasinoNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<CellUpgradeNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<PrisonNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<ThrowCubesNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<TradeProposeAcceptingNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<TradeProposeNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<ForfeitNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<GiveUpNetMessage>(RegistrateForServer);
                NetworkServer.RegisterHandler<SendLogToServerNetMessage>(RegistrateForServer);
            }
            else if (_debugMode)
            {
                _server.MessageSender.OnCallInvoked += _client.Recieve;
            }
        }

        private void SendMessageToClients(NetworkMessage message)
        {
            if (message is INetMessage netMes)
            {
                netMes.SendToClient();
            }
        }
        private void SendMessageToServer(NetworkMessage message)
        {
            if (message is INetMessage netMes)
            {
                netMes.SendToServer();
            }
        }

        private void RegistrateForClient<T>(T message) where T : struct, NetworkMessage
            => _client.Recieve(message);
        private void RegistrateForServer<T>(NetworkConnectionToClient conn, T message) where T : struct, NetworkMessage
            => _server.Recieve(message);
    }
}
