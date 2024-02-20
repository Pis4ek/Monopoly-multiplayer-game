using Mirror;
using Playmode.CommandSystem;
using Playmode.NetCommunication;
using Playmode.PlayData;

namespace Playmode.ServerEnteties
{
    public class DefaultServerState : ServerState
    {
        private InputPermissions _permissions = new();

        public DefaultServerState(IServerStateMachine context, GameData data,
            NetMessageSender messageSender, CommandHandler commandHander, MessageWaiter waiter,
            UpdatingDataCollector collector)
            : base(context, data, messageSender, commandHander, waiter, collector)
        {
            _permissions.Activate(InputType.ThrowCubes);
            _permissions.Activate(InputType.UpgradeCell);
            _permissions.Activate(InputType.DowngradeCell);
            _permissions.Activate(InputType.TradeProposing);

            _endWaitAction += () => { _commandHandler.Handle(new LoseCommand(_data.TurnData.ActivePlayer)); };
        }

        public override void Reset() 
        {
            _permissions.Activate(InputType.UpgradeCell);
        }

        public override void Enter(object obj = null)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} enter function. Active player is{_data.GetActivePlayer().ID}");
            var mes = new InputRequireNetMessage(_data.TurnData.ActivePlayer, _permissions);
            SendMessageWithWaiting(mes, typeof(ThrowCubesNetMessage));
        }

        public override void HandleMessage(NetworkMessage message)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} handle message {message.GetType().Name}. Active player is {_data.GetActivePlayer().ID}");
            if (message is ThrowCubesNetMessage cubesMes)
            {
                var player = _data[cubesMes.ByWho] as Player;
                var result = new ThrowCubesResult(cubesMes.ByWho);
                _context.TurnCycleData.LastThrowCubesResult = result;
                _messageSender.SendMessage(new ShowCubesThrowNetMessage(result));

                if(_context.TurnCycleData.SameCubesResultsCount >= 3)
                {
                    _data.LoggerData.AddTripleDoubleDicesLog(cubesMes.ByWho, result);
                    _commandHandler.Handle(new SetPrisonPlayerStateCommand(cubesMes.ByWho, true));
                    _commandHandler.Handle(new EndTurnCommand());
                    return;
                }

                if(player.TryGetEffect<ReversiveMoveEffect>(out var effect))
                {
                    _data.LoggerData.AddThrowCubesReverciveLog(cubesMes.ByWho, result);
                    _commandHandler.Handle(new DecrementEffectCounter(cubesMes.ByWho, effect.GetType()));
                    _commandHandler.Handle(new ChangePositionCommand(-result.ResultSum, cubesMes.ByWho));
                }
                else
                {
                    _data.LoggerData.AddThrowCubesLog(cubesMes.ByWho, result);
                    _commandHandler.Handle(new ChangePositionCommand(result));
                }
                _context.SwitchState<StandOnCellServerState>(result);
            }
            else if (message is TradeProposeNetMessage tradeMes)
            {
                _context.SwitchState<TradeServerState>(tradeMes.TradeOffer);
            }
            else if (message is CellUpgradeNetMessage upgradeMes)
            {
                _commandHandler.Handle(new ChangeCellLevelCommand(upgradeMes.CellIndex, upgradeMes.IsUpgrade));
                _data.LoggerData.AddUpgradeCellLog(upgradeMes.ByWho, _data[upgradeMes.CellIndex] as IBusinessCell, upgradeMes.IsUpgrade);

                if (upgradeMes.IsUpgrade)
                {
                    _permissions.Disactivate(InputType.UpgradeCell);
                }
                SendMessage(new InputRequireNetMessage(_data.TurnData.ActivePlayer, _permissions));
            }
            else CheckDefaultMessages(message);
        }
    }
}
