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
    public class BuyOrAuctionInputWindow : BaseInputWindow, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] DoubleButtonWidget _buttons;

        private LastGameClientsSession _clientsInfo;

        [Inject]
        private void Init(LastGameClientsSession clientsInfo)
        {
            _clientsInfo = clientsInfo;
            _buttons.OnLeftClick += BuyInput;
            _buttons.OnRightClick += AuctionInput;
            _buttons.Init();
        }

        private void BuyInput()
        {
            OnInputEntered?.Invoke(new BuyOrAuctionNetMessage(_clientsInfo.PlayerID, true));
        }

        private void AuctionInput()
        {
            OnInputEntered?.Invoke(new BuyOrAuctionNetMessage(_clientsInfo.PlayerID, false));
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            if (inputMessage.InputPermissions[InputType.BuyOrAuction])
            {
                _buttons.SetDefaultState();
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
