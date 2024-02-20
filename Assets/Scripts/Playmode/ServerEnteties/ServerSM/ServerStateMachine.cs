using Mirror;
using Playmode.CommandSystem;
using Playmode.NetCommunication;
using Playmode.PlayData;
using System;
using System.Collections.Generic;
using Zenject;

namespace Playmode.ServerEnteties
{
    public class ServerStateMachine : IServerStateMachine, IInitializable
    {
        public TurnCycleData TurnCycleData { get; private set; } = new();
        public NetMemberState PreviousState { get; private set; }

        private Dictionary<Type, ServerState> _states = new();
        private ServerState _currentState;
        [Inject(Id = "Server")] NetMessageSender _messageSender;
        [Inject] GameData _data;
        [Inject] private CommandHandler _commandHandler;
        [Inject] private MessageWaiter _messageWaiter;
        [Inject] private UpdatingDataCollector _collector;

        public void Initialize()
        {
            InitStates();
            _data.TurnData.OnTurnEnded += StartTurn;
            StartTurn();
        }

        public void HandleMessage(NetworkMessage message)
        {
            _messageWaiter.CheckMessage(message.GetType());
            _currentState.HandleMessage(message);
            _collector.SendUpdateMessage();
        }

        public void StartTurn()
        {
            TurnCycleData.SameCubesResultsCount = 0;
            foreach (var state in _states.Values) state.Reset();

            SwitchState<StartTurnServerState>();
        }

        #region StateSwitching
        public void SwitchState<TStateType>(object obj = null) where TStateType : NetMemberState
        {
            SwitchState(typeof(TStateType), obj);
        }

        public void SwitchToPreviousState(object obj = null)
        {
            if (PreviousState != null)
            {
                SwitchState(PreviousState.GetType(), obj);
            }
            else
            {
                UnityEngine.Debug.LogError($"{GetType().Name} has not link to PreviousState and " +
                    $"can not get out of state.");
            }
        }

        private void SwitchState(Type stateType, object obj = null)
        {
            if (_states.TryGetValue(stateType, out var state))
            {
                PreviousState = _currentState;
                _collector.SendUpdateMessage();
                _currentState.StopWaiting();
                _currentState = state;
                _currentState.Enter(obj);
            }
            else
            {
                UnityEngine.Debug.Log($"{GetType().Name} has not such state as {stateType.Name}");
            }
        }
        #endregion

        private void InitStates()
        {
            ServerState state = new TradeServerState(this, _data, _messageSender,
                _commandHandler, _messageWaiter, _collector);
            _states.Add(typeof(TradeServerState), state);

            state = new AuctionServerState(this, _data, _messageSender,
                _commandHandler, _messageWaiter, _collector);
            _states.Add(typeof(AuctionServerState), state);

            state = new StandOnCellServerState(this, _data, _messageSender,
                _commandHandler, _messageWaiter, _collector);
            _states.Add(typeof(StandOnCellServerState), state);

            state = new PrisonServerState(this, _data, _messageSender,
                _commandHandler, _messageWaiter, _collector);
            _states.Add(typeof(PrisonServerState), state);

            state = new BuyOrAuctionServerState(this, _data, _messageSender,
                _commandHandler, _messageWaiter, _collector);
            _states.Add(typeof(BuyOrAuctionServerState), state);

            state = new DefaultServerState(this, _data, _messageSender,
                _commandHandler, _messageWaiter, _collector);
            _states.Add(typeof(DefaultServerState), state);

            state = new StartTurnServerState(this, _data, _messageSender,
                _commandHandler, _messageWaiter, _collector);
            _states.Add(typeof(StartTurnServerState), state);

            _currentState = state;
        }
    }
}
