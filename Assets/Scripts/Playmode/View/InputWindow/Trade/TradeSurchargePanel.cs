using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Playmode.View
{
    public enum TradeMemberType { Propouser, Reciever }

    public class TradeSurchargePanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image _cashIcon;
        [SerializeField] InputField _inputField;
        [SerializeField] Image _leftArrow;
        [SerializeField] Image _rightArrow;

        public int Surcharge { 
            get => _surcharge;
            private set 
            {
                _surcharge = value;
                _inputField.text = value.ToString();
            }
        }
        public PlayerID Payer { get; private set; }

        private PlayerID _propouser;
        private PlayerID _reciever;
        private RectTransform _inputTransform;
        private Rect _inputRect;
        private int _surcharge;

        public void Start()
        {
            _inputTransform = _inputField.transform as RectTransform;
            _inputRect = _inputTransform.GetWorldRect();
            _inputField.onValueChanged.AddListener((string text) => {
                _surcharge = Convert.ToInt32(text);
                }
            );
        }

        public void SetPlayers(PlayerID propouser, PlayerID reciever)
        {
            _propouser = propouser;
            _reciever = reciever;
            Payer = _propouser;
            _inputField.interactable = true;
            UpdatePayerView();
        }

        public void SetPayerAndSurcharge(PlayerID payer, int surcharge)
        {
            Payer = payer;
            Surcharge = surcharge;
            _inputField.text = surcharge.ToString();
            _inputField.interactable = false;
            UpdatePayerView();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_inputRect.Contains(eventData.position) == false)
            {
                if (Payer == _reciever)
                    Payer = _propouser;
                else
                    Payer = _reciever;
                UpdatePayerView();
            }
        }

        private void UpdatePayerView()
        {
            if (Surcharge == 0)
            {
                _leftArrow.Disactivate();
                _rightArrow.Disactivate();
            }
            if (Payer == _reciever)
            {
                _leftArrow.Activate();
                _rightArrow.Disactivate();
            }
            else
            {
                _rightArrow.Activate();
                _leftArrow.Disactivate();
            }
        }
    }
}