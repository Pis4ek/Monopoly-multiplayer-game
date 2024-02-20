using Assets.Scripts.Other;
using Mirror;
using Other;
using Playmode.NetCommunication;
using Playmode.PlayData;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

namespace Playmode.View
{
    public class AuctionInputWindow : BaseInputWindow, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] DoubleButtonWidget _buttons;

        private LastGameClientsSession _clientsInfo;

        [Inject]
        private void Init(LastGameClientsSession clientsInfo)
        {
            _clientsInfo = clientsInfo;
            _buttons.OnLeftClick += IncreaceStake;
            _buttons.OnRightClick += GetOutOfAuction;
            _buttons.Init();
        }

        private void IncreaceStake()
        {
            OnInputEntered?.Invoke(new AuctionNetMessage(_clientsInfo.PlayerID, true));
        }

        private void GetOutOfAuction()
        {
            OnInputEntered?.Invoke(new AuctionNetMessage(_clientsInfo.PlayerID, false));
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            if (inputMessage.InputPermissions[InputType.Auction])
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