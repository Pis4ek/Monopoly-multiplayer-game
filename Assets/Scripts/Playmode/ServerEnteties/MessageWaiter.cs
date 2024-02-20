using Mirror;
using Playmode.NetCommunication;
using System;
using Zenject;

namespace Playmode.ServerEnteties
{
    public class MessageWaiter : IFixedTickable
    {
        public event Action OnWaitingEnded;

        public bool IsWaiting { get; private set; } = false;

        private DateTime _endWaitingTime;
        private Type _waitedMessageType;


        public void FixedTick()
        {
            if (IsWaiting)
            {
                if(_endWaitingTime < DateTime.Now)
                {
                    IsWaiting = false;
                    UnityEngine.Debug.Log("Ended input time!!!");
                    OnWaitingEnded?.Invoke();
                }
            }
        }

        public void SetMessageForWaiting(Type waitedMessage, DateTime endTime)
        {
            _waitedMessageType = waitedMessage;
            _endWaitingTime = endTime;
            IsWaiting = true;
        }

        public void CheckMessage(Type messageType)
        {
            if (_waitedMessageType == null) return;

            if(messageType == _waitedMessageType)
            {
                IsWaiting = false;
            }
        }

        public void StopWaiting()
        {
            IsWaiting = false;
        }
    }
}
