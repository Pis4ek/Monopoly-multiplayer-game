using Playmode.NetCommunication;

namespace Playmode.ServerEnteties
{
    public interface IServerStateMachine : INetMemberStateMachine
    {
        public TurnCycleData TurnCycleData { get; }
    }
}
