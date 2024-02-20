using Assets.Scripts.Other;
using Mirror;
using Other;
using Playmode.NetCommunication;
using System;
using UnityEngine;
using Zenject;

namespace Playmode.View
{
    public class ThrowCubesInputWindow : BaseInputWindow, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [Inject] private LastGameClientsSession _clientsInfo;

        [SerializeField] ScalableButton _throwButton;

        private bool _isInputEntered = false;

        private void Start()
        {
            _throwButton.OnClick += SendInput;
            _throwButton.Init();
        }

        private void SendInput()
        {
            if(_isInputEntered == false)
            {
                _isInputEntered = true;
                OnInputEntered?.Invoke(new ThrowCubesNetMessage(_clientsInfo.PlayerID));
            }
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            _isInputEntered = false;
            if (inputMessage.InputPermissions[InputType.ThrowCubes])
            {
                _throwButton.SetDefaultState();
                AnimateShowing();
            }
        }

        public void HideInput()
        {
            if (this.IsActive())
            {
                AnimateHiding();
            }
        }
    }
}
