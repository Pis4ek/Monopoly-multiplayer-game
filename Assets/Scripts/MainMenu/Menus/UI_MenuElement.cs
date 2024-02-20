using MainMenu.StateMachine;
using UnityEngine;

namespace MainMenu
{
    public abstract class UI_MenuElement : MonoBehaviour, IMainMenuState
    {
        public virtual void Enter()
        {
            this.Activate();
        }

        public virtual void Exit()
        {
            this.Disactivate();
        }

        public virtual void ResetState()
        {
            this.Disactivate();
        }
    }
}
