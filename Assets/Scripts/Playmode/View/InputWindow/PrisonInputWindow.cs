using Assets.Scripts.Other;
using Mirror;
using Other;
using Playmode.NetCommunication;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public class PrisonInputWindow : BaseInputWindow, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [SerializeField] DoubleButtonWidget _buttons;
        [SerializeField] TextMeshProUGUI _textTitle;

        private LastGameClientsSession _clientsInfo;

        [Inject]
        private void Init(LastGameClientsSession clientsInfo)
        {
            _clientsInfo = clientsInfo;
            _buttons.OnLeftClick += SendTryEscape;
            _buttons.OnRightClick += SendPledge;
            _buttons.Init();
        }

        private void SendTryEscape()
        {
            OnInputEntered?.Invoke(new PrisonNetMessage(_clientsInfo.PlayerID, true));
        }

        private void SendPledge()
        {
            OnInputEntered?.Invoke(new PrisonNetMessage(_clientsInfo.PlayerID, false));
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            if (inputMessage.InputPermissions[InputType.Prison])
            {
                _buttons.SetDefaultState();
                AnimateShowing();
                _buttons.SetInteractableForElement(IsLeftOrRight.Left, true);
            }
            else if (inputMessage.InputPermissions[InputType.PrisonWithoutEscape])
            {
                _buttons.SetDefaultState();
                AnimateShowing();
                _buttons.SetInteractableForElement(IsLeftOrRight.Left, false);
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