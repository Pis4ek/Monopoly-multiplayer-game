namespace Playmode.NetCommunication
{
    public interface INetMemberStateMachine
    {
        public void SwitchState<TStateType>(object obj = null) where TStateType : NetMemberState;
        public void SwitchToPreviousState(object obj = null);
    }
}
