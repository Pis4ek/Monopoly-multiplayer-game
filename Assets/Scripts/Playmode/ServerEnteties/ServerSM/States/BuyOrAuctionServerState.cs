using Mirror;
using Playmode.CommandSystem;
using Playmode.NetCommunication;
using Playmode.PlayData;

namespace Playmode.ServerEnteties
{
    public class BuyOrAuctionServerState : ServerState
    {
        private InputPermissions _permissions = new();

        public BuyOrAuctionServerState(IServerStateMachine context, GameData data,
            NetMessageSender messageSender, CommandHandler commandHander, MessageWaiter waiter,
            UpdatingDataCollector collector)
            : base(context, data, messageSender, commandHander, waiter, collector)
        {
            _permissions.Activate(InputType.BuyOrAuction);
            _permissions.Activate(InputType.DowngradeCell);

            _endWaitAction += () => { _commandHandler.Handle(new LoseCommand(_data.TurnData.ActivePlayer)); };
        }

        public override void Enter(object obj = null)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} enter function. Active player is{_data.GetActivePlayer().ID}");
            var mes = new InputRequireNetMessage(_data.TurnData.ActivePlayer, _permissions);
            SendMessageWithWaiting(mes, typeof(BuyOrAuctionNetMessage));
        }

        public override void HandleMessage(NetworkMessage message)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} handle message {message.GetType().Name}. Active player is{_data.GetActivePlayer().ID}");
            if (message is BuyOrAuctionNetMessage mes)
            {
                if (mes.Buy)
                {
                    var player = _data.GetActivePlayer();
                    _commandHandler.Handle(new BuyCellUnderPlayerCommand(player.ID));
                    _data.LoggerData.AddBuyCellLog(mes.ByWho, player.CurrentCell as IBusinessCell);
                    TryEndTurnOrRestart();
                }
                else
                {
                    _data.LoggerData.AddLeaveToAuctionCellLog(mes.ByWho, _data.GetActivePlayer().CurrentCell as IBusinessCell);
                    _context.SwitchState<AuctionServerState>();
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
    }
}
