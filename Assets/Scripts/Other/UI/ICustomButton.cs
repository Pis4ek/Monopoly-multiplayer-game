using System;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Other
{
    public interface ICustomButton : IPointerClickHandler
    {
        public event Action OnClick;

        public bool IsInteractable { get; }

        public void SetInteractable(bool isInteractable);
    }
}