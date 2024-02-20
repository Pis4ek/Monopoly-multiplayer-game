using Mirror;
using Other;
using Playmode.NetCommunication;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public class TradeAcceptingInputWindow : MonoBehaviour, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [SerializeField] Button _acceptButton;
        [SerializeField] Button _rejectButton;

        [Inject] private LastGameClientsSession _clientsInfo;
        private TradeOfferInfo _tradeOfferInfo = new();

        private void Start()
        {
            _acceptButton.onClick.AddListener(() => SendInput(true));
            _rejectButton.onClick.AddListener(() => SendInput(false));
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            //if (inputPermissions[InputType.TradeProposeAccepting]) 
                //this.Activate();
        }

        public void HideInput() => this.Disactivate();

        private void SendInput(bool accepting)
        {
            var mes = new TradeProposeAcceptingNetMessage(_clientsInfo.PlayerID, accepting);
            OnInputEntered?.Invoke(mes);
        }
    }
}
