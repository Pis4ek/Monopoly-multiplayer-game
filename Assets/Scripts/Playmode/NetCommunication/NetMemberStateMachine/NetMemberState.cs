using Playmode.PlayData;
using Mirror;

namespace Playmode.NetCommunication
{
    public abstract class NetMemberState
    {
        protected readonly GameData _data;
        protected readonly NetMessageSender _messageSender;

        public NetMemberState(GameData data, NetMessageSender messageSender)
        {
            _data = data;
            _messageSender = messageSender;
        }

        public virtual void Reset() { }
        public abstract void Enter(object obj = null);
        public abstract void HandleMessage(NetworkMessage message);

        protected void ThrowNotHandledExeption(NetworkMessage message)
        {
            UnityEngine.Debug.LogError($"{GetType().Name} can not handle net message \"{message.GetType().Name}\"");
        }
    }
}
