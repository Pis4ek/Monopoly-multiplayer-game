using Mirror;
using Playmode.CommandSystem;
using Playmode.PlayData;
using Playmode.ServerEnteties;
using System.Collections.Generic;

namespace Playmode.NetCommunication
{
    public class PrisonServerState : ServerState
    {
        private InputPermissions _permissions = new();
        private Dictionary<PlayerID, int> _prisionersFailEscapeCount = new();

        public PrisonServerState(IServerStateMachine context, GameData data,
            NetMessageSender messageSender, CommandHandler commandHander, MessageWaiter waiter,
            UpdatingDataCollector collector)
            : base(context, data, messageSender, commandHander, waiter, collector)
        {
            _permissions.Activate(InputType.Prison);
            _permissions.Activate(InputType.DowngradeCell);
            _permissions.Activate(InputType.TradeProposing);

            foreach(IPlayer player in _data.PlayerData)
            {
                _prisionersFailEscapeCount.Add(player.ID, 0);
            }

            _endWaitAction += () => { _commandHandler.Handle(new LoseCommand(_data.TurnData.ActivePlayer)); };
        }

        public override void Enter(object obj = null)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} enter function. Active player is{_data.GetActivePlayer().ID}");
            if (_prisionersFailEscapeCount[_data.GetActivePlayer().ID] == 3)
            {
                _permissions.Disactivate(InputType.Prison);
                _permissions.Activate(InputType.PrisonWithoutEscape);
            }
            else
            {
                _permissions.Activate(InputType.Prison);
                _permissions.Disactivate(InputType.PrisonWithoutEscape);
            }
            var mes = new InputRequireNetMessage(_data.TurnData.ActivePlayer, _permissions);
            SendMessageWithWaiting(mes, typeof(PrisonNetMessage));
        }

        public override void HandleMessage(NetworkMessage message)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} handle message {message.GetType().Name}. Active player is{_data.GetActivePlayer().ID}");
            if (message is PrisonNetMessage prisonMes)
            {
                if (prisonMes.IsTryEscape)
                {
                    var result = new ThrowCubesResult(prisonMes.ByWho);
                    _data.LoggerData.AddEscapePrisonLog(prisonMes.ByWho, result);
                    _messageSender.SendMessage(new ShowCubesThrowNetMessage(result));

                    if (result.HasSameResults())
                    {
                        _prisionersFailEscapeCount[prisonMes.ByWho] = 0;
                        _commandHandler.Handle(new SetPrisonPlayerStateCommand(prisonMes.ByWho, false));
                        _context.SwitchState<DefaultServerState>();
                    }
                    else
                    {
                        _prisionersFailEscapeCount[prisonMes.ByWho]++;
                        _commandHandler.Handle(new EndTurnCommand());
                    }
                }
                else
                {
                    _data.LoggerData.AddPledgePrisonLog(prisonMes.ByWho);
                    _commandHandler.Handle(new ChangeCashCommand(prisonMes.ByWho, -500));
                    _commandHandler.Handle(new SetPrisonPlayerStateCommand(prisonMes.ByWho, false));
                    _context.SwitchState<DefaultServerState>();
                }
            }
            else if (message is TradeProposeNetMessage tradeMes)
            {
                _context.SwitchState<TradeServerState>();
            }
            else if (message is CellUpgradeNetMessage upgradeMes)
            {
                _commandHandler.Handle(new ChangeCellLevelCommand(upgradeMes.CellIndex, upgradeMes.IsUpgrade));
                _data.LoggerData.AddUpgradeCellLog(upgradeMes.ByWho, _data[upgradeMes.CellIndex] as IBusinessCell, upgradeMes.IsUpgrade);
                SendMessage(new InputRequireNetMessage(_data.TurnData.ActivePlayer, _permissions));
            }
            else CheckDefaultMessages(message);
        }
    }
}
