using Mirror;
using Playmode.CommandSystem;
using Playmode.NetCommunication;
using Playmode.PlayData;

namespace Playmode.ServerEnteties
{
    public class StartTurnServerState : ServerState
    {
        public StartTurnServerState(IServerStateMachine context, GameData data,
            NetMessageSender messageSender, CommandHandler commandHander, MessageWaiter waiter,
            UpdatingDataCollector collector)
            : base(context, data, messageSender, commandHander, waiter, collector) { }

        public override void Enter(object obj = null)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} enter function. Active player is{_data.GetActivePlayer().ID}");
            if (_data[_data.TurnData.ActivePlayer].CurrentCell.Index == (int)CellID.Prison)
            {
                _context.SwitchState<PrisonServerState>();
            }
            else
            {
                _context.SwitchState<DefaultServerState>();
            }
        }

        public override void HandleMessage(NetworkMessage message)
        {
            //UnityEngine.Debug.Log($"{GetType().Name} handle message {message.GetType().Name}. Active player is{_data.GetActivePlayer().ID}");
            CheckDefaultMessages(message);
        }
    }
}
