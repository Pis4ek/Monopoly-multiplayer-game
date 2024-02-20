using Mirror;
using Other;
using Playmode.NetCommunication;
using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public class TradeProposingInputWindow : BaseInputWindow, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [SerializeField] Button _sendButton;
        [SerializeField] Button _closeButton;
        [SerializeField] Button _acceptButton;
        [SerializeField] Button _rejectButton;

        [SerializeField] TradeSurchargePanel _surchargePanel;
        [SerializeField] TradeMemberInfoPanel _recieverInfo;
        [SerializeField] TradeMemberInfoPanel _propouserInfo;

        [SerializeField] TradeCompanyLayout _propouserGiveCells; 
        [SerializeField] TradeCompanyLayout _recieverGiveCells;

        [Inject] LastGameClientsSession _session;
        [Inject] ClientsGameData _gameData;
        [Inject] ViewInputStateMachine _viewInputSM;
        private TradeOfferInfo _tradeOfferInfo = new();
        private bool _canShow = false;

        private void Start()
        {
            _sendButton.onClick.AddListener(SendInput);
            _closeButton.onClick.AddListener(() => { 
                this.Disactivate();
                _viewInputSM.State = ViewInputState.Default;
                _propouserGiveCells.Clear();
                _recieverGiveCells.Clear();
            }
            );

            _acceptButton.onClick.AddListener(() => SendAccepting(true));
            _rejectButton.onClick.AddListener(() => SendAccepting(false));

            _tradeOfferInfo.Proposer = _session.PlayerID;
        }

        public void Show(PlayerID playerID)
        {
            if(_canShow)
            {
                this.Activate();
                _tradeOfferInfo.Reciever = playerID;

                _sendButton.Activate();
                _closeButton.Activate();
                _acceptButton.Disactivate();
                _rejectButton.Disactivate();

                _viewInputSM.State = ViewInputState.Trade;
                _surchargePanel.SetPlayers(_session.PlayerID, playerID);
                _propouserInfo.SetPlayer(_gameData.PlayerData[_session.PlayerID]);
                _recieverInfo.SetPlayer(_gameData.PlayerData[playerID]);
            }
        }

        public void TryAddOrDeleteCell(ClientsBusinessCellData cell)
        {
            if(cell.Owner == _propouserInfo.PlayerID)
            {
                if (_propouserGiveCells.Contains(cell))
                {
                    _propouserGiveCells.RemoveElement(cell);
                    _tradeOfferInfo.CellsToReciever.Remove(cell.Index);
                }
                else
                {
                    _propouserGiveCells.AddElement(cell);
                    _tradeOfferInfo.CellsToReciever.Add(cell.Index);
                }
            }
            else if (cell.Owner == _recieverInfo.PlayerID)
            {
                if (_recieverGiveCells.Contains(cell))
                {
                    _recieverGiveCells.RemoveElement(cell);
                    _tradeOfferInfo.CellsToProposer.Remove(cell.Index);
                }
                else
                {
                    _recieverGiveCells.AddElement(cell);
                    _tradeOfferInfo.CellsToProposer.Add(cell.Index);
                }
            }
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            if (inputMessage.InputPermissions[InputType.TradeProposing])
            {
                _canShow = true;
            }
            if(inputMessage is TradeAcceptRequireNetMessage mes)
            {
                ShowTradeProposition(mes.tradeOffer);
            }
        }

        public void HideInput()
        {
            this.Disactivate();
            _canShow = false;
        }

        private void ShowTradeProposition(TradeOfferInfo info)
        {
            if (info == null)
            {
                Debug.LogError("TradeOfferInfo is null");
            }

            this.Activate();
            _sendButton.Disactivate();
            _closeButton.Disactivate();
            _acceptButton.Activate();
            _rejectButton.Activate();

            foreach(var cell in info.CellsToProposer)
            {
                _propouserGiveCells.AddElement(_gameData.MapData[cell] as ClientsBusinessCellData);
            }
            foreach (var cell in info.CellsToReciever)
            {
                _recieverGiveCells.AddElement(_gameData.MapData[cell] as ClientsBusinessCellData);
            }

            _surchargePanel.SetPlayers(info.Proposer, info.Reciever);
            _surchargePanel.SetPayerAndSurcharge(info.Payer, info.Surcharge);

            _propouserInfo.SetPlayer(_gameData.PlayerData[info.Proposer]);
            _recieverInfo.SetPlayer(_gameData.PlayerData[info.Reciever]);
        }

        private void SendInput()
        {
            _tradeOfferInfo.Surcharge = _surchargePanel.Surcharge;
            _tradeOfferInfo.Payer = _surchargePanel.Payer;


            OnInputEntered?.Invoke(new TradeProposeNetMessage(_session.PlayerID, _tradeOfferInfo));
        }

        private void SendAccepting(bool accepting)
        {
            _propouserGiveCells.Clear();
            _recieverGiveCells.Clear();
            var mes = new TradeProposeAcceptingNetMessage(_session.PlayerID, accepting);
            OnInputEntered?.Invoke(mes);
        }
    }
}
