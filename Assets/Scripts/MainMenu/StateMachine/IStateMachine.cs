namespace MainMenu.StateMachine
{
    public interface IStateMachine<State> where State : IState
    {
        public void SwitchState<U>() where U : State;
    }
}
