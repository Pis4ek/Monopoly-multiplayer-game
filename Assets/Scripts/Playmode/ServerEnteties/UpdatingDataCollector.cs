using Playmode.NetCommunication;
using Playmode.PlayData;
using System.Collections.Generic;
using Zenject;

namespace Playmode.ServerEnteties
{
    public class UpdatingDataCollector
    {
        [Inject(Id = "Server")] private NetMessageSender _messageSender;
        private GameData _gameData;
        private UpdateGameDataNetMessage _message;

        private Dictionary<PlayerID, PlayerInfoPackage> _addedPlayers = new();
        private Dictionary<int, CellInfoPackage> _addedCells = new();

        public UpdatingDataCollector(GameData gameData)
        {
            _gameData = gameData;
            _message = new(new(), new(), new(), new());
            SubscribeToAll();
        }

        public void SendUpdateMessage()
        {
            foreach(var player in _addedPlayers.Values)
                _message.PlayersData.Add(player); 
            foreach (var cell in _addedCells.Values)
                _message.CellsData.Add(cell);

            _messageSender.SendMessage(_message);
            _message.CellsData.Clear();
            _message.PlayersData.Clear();
            _addedPlayers.Clear();
            _addedCells.Clear();
        }

        private void SubscribeToAll()
        {
            foreach (ICell cell in _gameData.MapData)
            {
                if(cell is BusinessCell c)
                {
                    c.OnAnyValueChanged += AddCell;
                }
            }

            foreach (IPlayer player in _gameData.PlayerData)
            {
                player.OnAnyValueChanged += AddPlayer;
            }

            _gameData.TurnData.OnAnyValueChanged += AddTurnInfo;
            _gameData.LoggerData.OnLogAdded += () => { _message.LogsData.Add(_gameData.LoggerData.LastLog); };
        }

        private void AddPlayer(IPlayer pl)
        {
            if (_addedPlayers.ContainsKey(pl.ID))
            {
                _addedPlayers[pl.ID] = new PlayerInfoPackage(pl);
            }
            else
            {
                _addedPlayers.Add(pl.ID, new PlayerInfoPackage(pl));
            }
        }

        private void AddCell(IBusinessCell bCell)
        {
            if (_addedCells.ContainsKey(bCell.Index))
            {
                _addedCells[bCell.Index] = new CellInfoPackage(bCell);
            }
            else
            {
                _addedCells.Add(bCell.Index, new CellInfoPackage(bCell));
            }
        }

        private void AddTurnInfo()
        {
            var package = new TurnDataInfoPackage();
            package.ActivePlayer = _gameData.TurnData.ActivePlayer;
            package.TurnNumber = _gameData.TurnData.TurnNumber;
            package.TurnCycleNumber = _gameData.TurnData.TurnCycleNumber;
            _message.TurnData = package;
        }
    }
}
