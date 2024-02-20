using Playmode.ServerEnteties;
using System.Collections.Generic;
using Mirror;
using Zenject;

namespace Playmode.NetCommunication
{
    public class Server : IFixedTickable, INetMember
    {
        [Inject(Id = "Server")] public NetMessageSender MessageSender { get; private set; }

        [Inject] private ServerStateMachine _stateMachine;
        private Queue<NetworkMessage> _mesQueue = new();
        private bool _isHandling = false;

        public void FixedTick()
        {
            if (_mesQueue.Count > 0 && _isHandling == false)
            {
                //UnityEngine.Debug.Log($"Server revieve message count {_mesQueue.Count}");
                _isHandling = true;
                Handle();
            }
        }

        public void Recieve(NetworkMessage message)
        {
            //UnityEngine.Debug.Log($"Server revieve message {message.GetType()}");
            _mesQueue.Enqueue(message);
        }

        public void Handle()
        {
            var message = _mesQueue.Dequeue();
            _stateMachine.HandleMessage(message);
            _isHandling = false;
        }

        public void StartTurn()
        {
            _stateMachine.StartTurn();
        }
    }
}
