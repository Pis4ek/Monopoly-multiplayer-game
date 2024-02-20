using Mirror;
using Playmode.CommandSystem;
using Playmode.NetCommunication;
using Playmode.PlayData;
using System;

namespace Playmode.ServerEnteties
{
    public abstract class ServerState : NetMemberState
    {
        protected readonly IServerStateMachine _context;
        protected readonly CommandHandler _commandHandler;
        protected readonly MessageWaiter _waiter;
        protected readonly UpdatingDataCollector _collector;
        protected Action _endWaitAction;

        public ServerState(IServerStateMachine context, GameData data, NetMessageSender messageSender, 
            CommandHandler commandHander, MessageWaiter waiter, UpdatingDataCollector collector) 
            : base(data, messageSender)
        {
            _context = context;
            _commandHandler = commandHander;
            _waiter = waiter;
            _collector = collector;
        }

        public void StopWaiting() => _waiter.OnWaitingEnded -= EndWait;

        protected void TryEndTurnOrRestart()
        {
            if (_context.TurnCycleData.LastThrowCubesResult.HasSameResults())
            {
                _context.SwitchState<StartTurnServerState>();
            }
            else
            {
                _commandHandler.Handle(new EndTurnCommand());
            }
        }

        protected void CheckDefaultMessages(NetworkMessage message)
        {
            if(message is SendLogToServerNetMessage logMessage)
            {
                _data.AddLog(logMessage.Author, logMessage.Text);
            }
            else ThrowNotHandledExeption(message);
        }

        protected void SendMessageWithWaiting(IInputRequireNetMessage message, Type waitedMessage, 
            int secondsForWait = 60)
        {
            _collector.SendUpdateMessage();

            _waiter.OnWaitingEnded += EndWait;
            var time = DateTime.Now.AddSeconds(secondsForWait);
            _waiter.SetMessageForWaiting(waitedMessage, time);
            _messageSender.SendMessage(new SetTimerNetMessage(time, message.Reciever)); 

            _messageSender.SendMessage(message);
        }

        protected void SendMessage(IInputRequireNetMessage message)
        {
            _collector.SendUpdateMessage();
            _messageSender.SendMessage(message);
        }

        private void EndWait()
        {
            _waiter.OnWaitingEnded -= EndWait;
            _endWaitAction?.Invoke();
        }
    }
}
