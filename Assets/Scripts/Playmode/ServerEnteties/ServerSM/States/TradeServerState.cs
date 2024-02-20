using Playmode.PlayData;
using Playmode.ServerEnteties;
using Playmode.NetCommunication;
using Playmode.CommandSystem;
using Mirror;

namespace Playmode.ServerEnteties
{
    public class TradeServerState : ServerState
    {
        private InputPermissions _permissions = new();
        private TradeOfferInfo _tradeOfferInfo;


        public TradeServerState(IServerStateMachine context, GameData data, 
            NetMessageSender messageSender, CommandHandler commandHander, MessageWaiter waiter, 
            UpdatingDataCollector collector) 
            : base(context, data, messageSender, commandHander, waiter, collector)
        {
            _permissions.Activate(InputType.TradeProposeAccepting);
            _endWaitAction += () => { _context.SwitchToPreviousState(); };
        }

        public override void Reset() 
        {
            _tradeOfferInfo = null;
        }

        public override void Enter(object obj = null)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} enter function. Active player is{_data.GetActivePlayer().ID}");
            if (obj != null)
            {
                if (obj is TradeOfferInfo info)
                {
                    _tradeOfferInfo = info;
                    _data.LoggerData.AddMakeProposeLog(_tradeOfferInfo.Proposer, _tradeOfferInfo.Reciever);
                    var mes = new TradeAcceptRequireNetMessage(_tradeOfferInfo.Reciever, _permissions, 
                        _tradeOfferInfo);
                    SendMessageWithWaiting(mes, typeof(TradeProposeAcceptingNetMessage), 40);
                }
                else
                {
                    UnityEngine.Debug.LogError("TradeServerState has recived not TradeOfferInfo");
                    _context.SwitchToPreviousState();
                    return;
                }
            }
            else
            {
                UnityEngine.Debug.LogError("TradeServerState has recived null");
                _context.SwitchToPreviousState();
                return;
            }
        }

        public override void HandleMessage(NetworkMessage message)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} handle message {message.GetType().Name}. Active player is{_data.GetActivePlayer().ID}");
            if (message is TradeProposeAcceptingNetMessage mes)
            {
                if (mes.IsAccept)
                {
                    _data.LoggerData.AddAcceptProposeLog(_tradeOfferInfo);
                    if (_tradeOfferInfo.Payer == _tradeOfferInfo.Proposer)
                    {
                        _commandHandler.Handle(new ChangeCashCommand(_tradeOfferInfo.Proposer, -_tradeOfferInfo.Surcharge));
                        _commandHandler.Handle(new ChangeCashCommand(_tradeOfferInfo.Reciever, _tradeOfferInfo.Surcharge));
                    }
                    else
                    {
                        _commandHandler.Handle(new ChangeCashCommand(_tradeOfferInfo.Proposer, _tradeOfferInfo.Surcharge));
                        _commandHandler.Handle(new ChangeCashCommand(_tradeOfferInfo.Reciever, -_tradeOfferInfo.Surcharge));
                    }
                    _commandHandler.Handle(new ChangeBusinessOwnerCommand(_tradeOfferInfo.CellsToProposer, _tradeOfferInfo.Proposer));
                    _commandHandler.Handle(new ChangeBusinessOwnerCommand(_tradeOfferInfo.CellsToReciever, _tradeOfferInfo.Reciever));
                    _context.SwitchToPreviousState();
                }
                else
                {
                    _data.LoggerData.AddRejectProposeLog(_tradeOfferInfo.Proposer, _tradeOfferInfo.Reciever);
                    _context.SwitchToPreviousState();
                }
            }
            else CheckDefaultMessages(message);
        }
    }
}
