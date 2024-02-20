using Mirror;
using Other;
using Playmode.PlayData.ClientsData;
using Playmode.View;
using Zenject;

namespace Playmode.NetCommunication
{
    public class Client : INetMember
    {
        public NetMessageSender MessageSender { get; private set; }

        private ClientsGameData _gameData;
        private PlaymodeView _playmodeView;
        private PlayerID _player;
        [Inject] private bool _debugMode = false;

        [Inject]
        public void Init(ClientsGameData gameData, PlaymodeView playmodeView,
            LastGameClientsSession gameInfo, NetMessageSender messageSender)
        {
            _gameData = gameData;
            _playmodeView = playmodeView;
            _player = gameInfo.PlayerID;
            MessageSender = messageSender;
        }

        public void Recieve(NetworkMessage message)
        {
            //UnityEngine.Debug.Log($"Client revieve message {message.GetType()}");
            if (message is IInputRequireNetMessage inputMes)
            {
                if(_debugMode)
                {
                    _playmodeView.ShowInput(inputMes);
                    return;
                }
                else if (inputMes.Reciever != _player) return;
                _playmodeView.ShowInput(inputMes);
            }
            else if (message is UpdateGameDataNetMessage updateMes)
            {
                _gameData.MapData.Update(updateMes.CellsData);
                _gameData.PlayerData.Update(updateMes.PlayersData);
                _gameData.TurnData.Update(updateMes.TurnData);
                _gameData.LogData.Update(updateMes.LogsData);
            }
            else if (message is ShowCubesThrowNetMessage showCubesMes)
            {
                _playmodeView.ShowThrowCubesResult(showCubesMes.Result);
            }
            else if (message is SetTimerNetMessage timerMes)
            {
                _playmodeView.SetWaitedPlayer(timerMes.Player, timerMes.EndTime);
            }
            else if (message is ShowLoseMessage loseMes)
            {
            }
            else if (message is ShowVictoryMessage victoryMes)
            {
            }
            else ThrowNotHandledExeption(message);
        }

        private void ThrowNotHandledExeption(NetworkMessage message)
        {
            UnityEngine.Debug.Log($"{GetType().Name} can not handle net message \"{message.GetType().Name}\"");
        }
    }
}
