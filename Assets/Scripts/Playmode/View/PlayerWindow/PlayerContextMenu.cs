using Mirror;
using Other;
using Playmode.NetCommunication;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    [RequireComponent(typeof(RectTransform))]
    public class PlayerContextMenu : MonoBehaviour, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [SerializeField] Button _muteButton;
        [SerializeField] Button _tradeButton;
        [SerializeField] Button _giveUpButton;

        public bool IsShown => gameObject.activeInHierarchy;

        [Inject] private LastGameClientsSession _session;
        [Inject] private TradeProposingInputWindow _tradeWindow;
        private RectTransform _transform;
        private PlayerID _shownPlayer;
        private bool _canTrade = false;

        private void Start()
        {
            _transform = transform as RectTransform;
            _tradeButton.onClick.AddListener(() => _tradeWindow.Show(_shownPlayer));
            _giveUpButton.onClick.AddListener(() => OnInputEntered?.Invoke(new GiveUpNetMessage(_shownPlayer)));
        }

        private void Update()
        {
            if (IsShown)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (_transform.GetWorldRect().Contains(Input.mousePosition) == false)
                    {
                        Hide();
                    }
                }
            }
        }

        public void Show(PlayerID player, Vector2 position)
        {
            transform.position = position;
            if (player == _session.PlayerID)
            {
                _giveUpButton.Activate();
            }
            else if(_canTrade == true)
            {
                _shownPlayer = player;
                _tradeButton.Activate();
            }
            this.Activate();
        }

        public void Hide()
        {
            _giveUpButton.Disactivate();
            _tradeButton.Disactivate();
            this.Disactivate();
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            if (inputMessage.InputPermissions[InputType.TradeProposing])
            {
                _canTrade = true;
            }
        }

        public void HideInput() => _canTrade = false;
    }
}