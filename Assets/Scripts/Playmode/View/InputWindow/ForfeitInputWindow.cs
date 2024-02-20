using Assets.Scripts.Other;
using Mirror;
using Other;
using Playmode.NetCommunication;
using Playmode.PlayData.ClientsData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public class ForfeitInputWindow : BaseInputWindow, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [SerializeField] DoubleButtonWidget _buttons;
        [SerializeField] TextMeshProUGUI _textTitle;

        private LastGameClientsSession _session;
        private ClientsGameData _gameData;

        [Inject]
        private void Init(LastGameClientsSession session, ClientsGameData gameData)
        {
            _session = session;
            _gameData = gameData;
            _buttons.OnLeftClick += SendInput;
            _buttons.Init();
        }

        private void SendInput()
        {
            OnInputEntered?.Invoke(new ForfeitNetMessage(_session.PlayerID));
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            if (inputMessage.InputPermissions[InputType.Forfeit])
            {
                _buttons.SetDefaultState();
                AnimateShowing();
            }
            if(inputMessage is ForfeitRequireNetMessage mes)
            {
                if (_gameData.PlayerData[_session.PlayerID].Cash < mes.cashToPay)
                {
                    _buttons.SetInteractableForElement(IsLeftOrRight.Left, false);
                    _buttons.SetActivityForElement(IsLeftOrRight.Right, true);
                }
                else
                {
                    _buttons.SetActivityForElement(IsLeftOrRight.Right, false);
                    _buttons.SetInteractableForElement(IsLeftOrRight.Left, true);
                }
                _buttons.SetTextInElement(IsLeftOrRight.Left, "Pay " + mes.cashToPay + "k");
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
