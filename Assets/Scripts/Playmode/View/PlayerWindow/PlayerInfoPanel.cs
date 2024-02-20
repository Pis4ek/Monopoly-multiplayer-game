using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using System;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public class PlayerInfoPanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Text _name;
        [SerializeField] Text _cash;
        [SerializeField] Image _avatarImage;
        [SerializeField] Image _lostImage;
        [SerializeField] Image _prisonImage;
        [SerializeField] PlayerInfoTimer _timer; 
        [SerializeField] PlayerInfoEffectsLayout _effects; 

        public ClientsPlayer Player { get; private set; }
        public Image BGImage { get; private set; }

        [Inject] PlayerContextMenu _contextMenu;
        [Inject] ViewInputStateMachine _viewInputSM;
        [Inject] Converter _viewConfig;
        private Color _activeColor;

        public PlayerInfoPanel Init(ClientsPlayer player)
        {
            BGImage = GetComponent<Image>();
            Player = player;
            _name.text = player.Name;
            _effects.SetPlayer(player);
            UpdateInfo(player);

            _activeColor = _viewConfig.PlayerIDToColor(Player.ID);
            _activeColor.a = 0.4f;

            Player.OnAnyValueChanged += UpdateInfo;
            return this;
        }

        public void ActivateWaiting(DateTime date)
        {
            if (_timer.IsActive() == false)
            {
                _timer.SetTimerData(date);
                _timer.Activate();
                transform.localScale = new Vector3(1.1f, 1.1f, 1);
                BGImage.color = _activeColor;
            }
        }
        public void DisactivateWaiting()
        {
            if(_timer.IsActive())
            {
                _timer.Disactivate();
                transform.localScale = Vector3.one;
                BGImage.color = new Color(0.3f, 0.3f, 0.3f, 0.4f);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_viewInputSM.State == ViewInputState.Default)
            {
                _contextMenu.Show(Player.ID, eventData.position);
                var pos = eventData.pointerClick.transform.position;

                //Debug.Log($"Ev - { eventData.position}, Panel - {pos}");
            }
        }

        private void UpdateInfo(ClientsPlayer player)
        {
            if(Player.State == PlayerState.Lost)
            {
                _lostImage.Activate();
                _cash.Disactivate();
                _effects.Disactivate();
                Player.OnAnyValueChanged -= UpdateInfo;
            }
            else if (Player.State == PlayerState.Prisoned)
            {
                _prisonImage.Activate();
                _cash.Activate();
            }
            else
            {
                _prisonImage.Disactivate();
                _cash.Activate();
                _cash.text = $"${Player.Cash}";
            }
        }
    }
}
