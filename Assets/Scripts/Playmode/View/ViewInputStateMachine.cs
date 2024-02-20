using System;

namespace Playmode.View
{
    public enum ViewInputState { Default, Trade, Exeption }
    public class ViewInputStateMachine
    {
        public event Action OnStateChanged;
        public ViewInputState State 
        { 
            get => state; 
            set
            {
                OnStateChanged?.Invoke();
                state = value;
            }
        }

        private ViewInputState state = ViewInputState.Default;
    }
}
