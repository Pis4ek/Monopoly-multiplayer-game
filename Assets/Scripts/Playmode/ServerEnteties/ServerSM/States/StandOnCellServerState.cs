using Mirror;
using Playmode.CommandSystem;
using Playmode.NetCommunication;
using Playmode.PlayData;
using System.Collections.Generic;

namespace Playmode.ServerEnteties
{
    public class StandOnCellServerState : ServerState
    {
        private InputPermissions _permissions = new();
        private ChanceCellEventToCommandConverter _converter;
        private ForfeitInfo _info;
        private List<ICommand> _forfeitCommands;

        public StandOnCellServerState(IServerStateMachine context, GameData data,
            NetMessageSender messageSender, CommandHandler commandHander, MessageWaiter waiter,
            UpdatingDataCollector collector)
            : base(context, data, messageSender, commandHander, waiter, collector)
        {
            _converter = new(data);
            _permissions.Activate(InputType.Forfeit);
            _permissions.Activate(InputType.DowngradeCell);

            _endWaitAction += () => { _commandHandler.Handle(new LoseCommand(_data.TurnData.ActivePlayer)); };
        }

        public override void Reset()
        {
            _info = null;
            _forfeitCommands = null;
        }

        public override void Enter(object obj = null)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} enter function. Active player is{_data.GetActivePlayer().ID}");
            _info = null;
            _forfeitCommands = null;
            var player = _data.GetActivePlayer();

            if (player.CurrentCell is BusinessCell cell)
            {
                if (cell.Owner == PlayerID.Nobody)
                {
                    //_data.LoggerData.AddStandOnBusinessCellLog(player.ID, player.CurrentCell as IBusinessCell);
                    _context.SwitchState<BuyOrAuctionServerState>();
                    return;
                }
                else if (cell.Owner != player.ID)
                {
                    _info = GetForfeitForBusinessCell(cell);
                }
                //_data.LoggerData.AddStandOnBusinessCellLog(player.ID, player.CurrentCell as IBusinessCell);
            }
            else if (player.CurrentCell is ChanceCell chanceCell)
            {
                var ev = chanceCell.GetRandomEvent();
                _info = _converter.Convert(ev, out _forfeitCommands);
            }

            if (_info == null)
            {
                _commandHandler.Handle(_forfeitCommands);
                if(player.State == PlayerState.Default)
                {
                    TryEndTurnOrRestart();
                }
                else
                {
                    _commandHandler.Handle(new EndTurnCommand());
                }
            }
            else
            {
                var mes = new ForfeitRequireNetMessage(player.ID, _permissions, _info.CashToPay);
                SendMessageWithWaiting(mes, typeof(ForfeitNetMessage));
            }

        }

        public override void HandleMessage(NetworkMessage message)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} handle message {message.GetType().Name}. Active player is{_data.GetActivePlayer().ID}");
            if (message is ForfeitNetMessage forfeitMes)
            {
                //UnityEngine.Debug.Log($"Message is {forfeitMes.GetType().Name} from {forfeitMes.ByWho}");
                if (_info.Reciever != PlayerID.Nobody)
                {
                    _commandHandler.Handle(new ChangeCashCommand(_info.Reciever, _info.CashToPay));
                }
                _data.LoggerData.AddPayForfeitLog(_info);
                _commandHandler.Handle(new ChangeCashCommand(_info.Payer, -_info.CashToPay));
                TryEndTurnOrRestart();
            }
            else if (message is CellUpgradeNetMessage upgradeMes && upgradeMes.IsUpgrade == false)
            {
                _commandHandler.Handle(new ChangeCellLevelCommand(upgradeMes.CellIndex, upgradeMes.IsUpgrade));
                _data.LoggerData.AddUpgradeCellLog(upgradeMes.ByWho, _data[upgradeMes.CellIndex] as IBusinessCell, upgradeMes.IsUpgrade);
                var mes = new ForfeitRequireNetMessage(upgradeMes.ByWho, _permissions, _info.CashToPay);
                SendMessage(mes);
            }
            else CheckDefaultMessages(message);
        }

        private ForfeitInfo GetForfeitForBusinessCell(BusinessCell cell)
        {
            var result = _context.TurnCycleData.LastThrowCubesResult;
            var player = _data.GetActivePlayer();
            var cellOwner = _data.GetPlayerByID(cell.Owner);
            ForfeitInfo forfeit = null;

            if (cell.Type == BusinessType.AutoIndustry)
            {
                var count = _data.GetPlayersCellsCountByType(cell.Owner, BusinessType.AutoIndustry);
                var cash = 250 * cell.Config.IncomeByLevel[count];
                forfeit =  new ForfeitInfo(cash, player.ID, cell.Owner);
            }
            else if (cell.Type == BusinessType.GameDev)
            {
                var count = _data.GetPlayersCellsCountByType(cell.Owner, BusinessType.GameDev);
                var cash = cell.Config.IncomeByLevel[count] * result.ResultSum;
                forfeit = new ForfeitInfo(cash, player.ID, cell.Owner);
            }
            else
            {
                forfeit = new ForfeitInfo(cell.Income, player.ID, cell.Owner);
            }

            if (cellOwner.TryGetEffect<IncreaceIncomeEffect>(out var incEffect))
            {
                var oldCash = forfeit.CashToPay;
                forfeit.CashToPay = (int)(forfeit.CashToPay * incEffect.Scaler);

                UnityEngine.Debug.Log($"Because cellOwner {cellOwner.Name} has IncreaceIncomeEffect" +
                    $" forfeit was increaced by {incEffect.Scaler} multiplier " +
                    $"from {oldCash} into {forfeit.CashToPay}");
            }
            else if(cellOwner.TryGetEffect<DecreaceIncomeEffect>(out var decEffect))
            {
                var oldCash = forfeit.CashToPay;
                forfeit.CashToPay = (int)(forfeit.CashToPay * decEffect.Scaler);

                UnityEngine.Debug.Log($"Because cellOwner {cellOwner.Name} has DecreaceIncomeEffect" +
                $" forfeit was decreaced by {decEffect.Scaler} multiplier " +
                $"from {oldCash} into {forfeit.CashToPay}");
            }

            if (player.TryGetEffect<IgnoreRentEffect>(out var ignoreEffect))
            {
                var oldCash = forfeit.CashToPay;
                forfeit.CashToPay = (int)(forfeit.CashToPay * ignoreEffect.Scaler);

                UnityEngine.Debug.Log($"Because player {player.Name} has IgnoreRentEffect" +
                $" forfeit was decreaced by {ignoreEffect.Scaler} multiplier " +
                $"from {oldCash} into {forfeit.CashToPay}");
            }

            if (forfeit.CashToPay <= 0) forfeit = null;
            //UnityEngine.Debug.Log(forfeit);

            return forfeit;
        }
    }
}
