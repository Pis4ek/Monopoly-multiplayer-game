using Mirror;
using Playmode.CommandSystem;
using Playmode.NetCommunication;
using Playmode.PlayData;
using System.Collections.Generic;

namespace Playmode.ServerEnteties
{
    public class AuctionServerState : ServerState
    {
        private InputPermissions _permissions = new();
        private List<PlayerID> _auctionMembers = new(5);
        private int _auctionistIndex;
        private BusinessCell _auctionTarget;
        private int _stake;

        public AuctionServerState(IServerStateMachine context, GameData data, 
            NetMessageSender messageSender, CommandHandler commandHander, MessageWaiter waiter, 
            UpdatingDataCollector collector) 
            : base(context, data, messageSender, commandHander, waiter, collector)
        {
            _permissions.Activate(InputType.Auction);
            _permissions.Activate(InputType.DowngradeCell);

            _endWaitAction += () => { _commandHandler.Handle(new LoseCommand(_data.TurnData.ActivePlayer)); };
        }

        public override void Enter(object obj = null)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} enter function. Active player is{_data.GetActivePlayer().ID}");
            _auctionMembers?.Clear();
            foreach (IPlayer player in _data.PlayerData)
            {
                if (player.State != PlayerState.Lost)
                {
                    _auctionMembers.Add(player.ID);
                }
            }
            _auctionMembers.Remove(_data.TurnData.ActivePlayer);
            _auctionistIndex = 0;

            _auctionTarget = _data[_data.TurnData.ActivePlayer].CurrentCell as BusinessCell;
            _stake = _auctionTarget.Config.Cost;

            RemovePlayersWithoutCash();

            if(_auctionMembers.Count > 0)
            {
                var mes = new InputRequireNetMessage(_auctionMembers[_auctionistIndex], _permissions);
                SendMessageWithWaiting(mes, typeof(AuctionNetMessage));
            }
            else
            {
                TryEndTurnOrRestart();
            }
        }

        public override void HandleMessage(NetworkMessage message)
        {
           // UnityEngine.Debug.Log($"{GetType().Name} handle message {message.GetType().Name}. Active player is{_data.GetActivePlayer().ID}");
            if (message is AuctionNetMessage mes)
            {
                if (mes.IsStakeIncreased)
                {
                    _stake += 100;
                    _data.LoggerData.AddAuctionRiseStakeLog(mes.ByWho, _stake);
                }
                else
                {
                    _auctionMembers.Remove(mes.ByWho);
                    _data.LoggerData.AddAuctionFallLog(mes.ByWho);
                }

                if(_auctionMembers.Count < 2)
                {
                    if(_auctionMembers.Count == 1)
                    {
                        _commandHandler.Handle(new ChangeBusinessOwnerCommand(_auctionTarget.Index, _auctionMembers[0]));
                        _commandHandler.Handle(new ChangeCashCommand(_auctionMembers[0], -_stake));
                        _data.LoggerData.AddAuctionWinLog(mes.ByWho, _auctionTarget, _stake);
                    }
                    TryEndTurnOrRestart();
                }
                else
                {
                    _auctionistIndex++;
                    if (_auctionistIndex >= _auctionMembers.Count)
                        _auctionistIndex = 0;

                    var player = _data[_auctionMembers[_auctionistIndex]];
                    if(player.Cash < _stake)
                    {
                        RemovePlayersWithoutCash();
                    }

                    var msg = new InputRequireNetMessage(_auctionMembers[_auctionistIndex], _permissions);
                    SendMessageWithWaiting(msg, typeof(AuctionNetMessage));
                }
            }
            else if (message is CellUpgradeNetMessage upgradeMes)
            {
                _commandHandler.Handle(new ChangeCellLevelCommand(upgradeMes.CellIndex, upgradeMes.IsUpgrade));
                _data.LoggerData.AddUpgradeCellLog(upgradeMes.ByWho, _data[upgradeMes.CellIndex] as IBusinessCell, upgradeMes.IsUpgrade);
                SendMessage(new InputRequireNetMessage(_data.TurnData.ActivePlayer, _permissions));
            }
            else CheckDefaultMessages(message);
        }

        private void RemovePlayersWithoutCash()
        {
            var list = new List<PlayerID>(4);
            foreach (var playerID in _auctionMembers)
            {
                var player = _data[playerID];
                if (player.Cash < _stake)
                {
                    list.Add(playerID);
                }
            }

            foreach (var playerID in list)
            {
                _auctionMembers.Remove(playerID);
            }
        }
    }
}
