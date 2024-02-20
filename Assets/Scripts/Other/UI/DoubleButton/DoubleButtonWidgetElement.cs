using DG.Tweening;
using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Other
{
    public enum IsLeftOrRight { Left, Right }

    [RequireComponent(typeof(Image))]
    public class DoubleButtonWidgetElement : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<DoubleButtonWidgetElement, PointerEventData> OnClick;
        public event Action<DoubleButtonWidgetElement, PointerEventData> OnEnter;
        public event Action<DoubleButtonWidgetElement, PointerEventData> OnExit;
        public event Action<DoubleButtonWidgetElement, PointerEventData> OnUp;
        public event Action<DoubleButtonWidgetElement, PointerEventData> OnDown;

        [field: SerializeField] public TextMeshProUGUI TextContent { get; private set; }
        [field:SerializeField] public IsLeftOrRight IsLeftOrRight { get; private set; }
        public bool IsEntered { get; private set; }
        public bool IsInteractable { get; private set; } = true;
        public Vector3 DefaultScale { get; private set; } = Vector3.one;
        public RectTransform RectTransform { get; private set; }
        public Sequence CurrentAnim { get; private set; }

        private DoubleButtonWidget.ScaleSettings _scaleSettings;
        private DoubleButtonWidget.TimeSettings _timeSettings;
        private DoubleButtonWidget.EasingSettings _easingSettings;
        private DoubleButtonWidget.InteractionSettings _interactionSettings;
        private Image _image;
        private Color _originalColor;

        public void Init(DoubleButtonWidget.ScaleSettings scaleSettings, DoubleButtonWidget.TimeSettings timeSettings, 
            DoubleButtonWidget.EasingSettings easingSettings, DoubleButtonWidget.InteractionSettings interactionSettings)
        {
            _scaleSettings = scaleSettings;
            _timeSettings = timeSettings;
            _easingSettings = easingSettings;
            _interactionSettings = interactionSettings;

            RectTransform = transform as RectTransform;
            DefaultScale = transform.localScale;
            _image = GetComponent<Image>();
            _originalColor = _image.color;

            SetInteractable(IsInteractable);
        }

        public void SetAnimation(Sequence animation)
        {
            CurrentAnim?.Kill();
            CurrentAnim = animation;
            CurrentAnim.Play();
        }

        public void SetInteractable(bool isInteractable)
        {
            if (isInteractable)
            {
                IsInteractable = true;

                _image.color = _originalColor;
            }
            else
            {
                IsInteractable = false;

                var color = new Color(
                    _originalColor.r * _interactionSettings.ColorMultiplier,
                    _originalColor.g * _interactionSettings.ColorMultiplier,
                    _originalColor.b * _interactionSettings.ColorMultiplier,
                    _originalColor.a * _interactionSettings.AlphaMultiplier
                    );
                _image.color = color;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (IsInteractable == false) return;

            OnClick?.Invoke(this, eventData);
        }

        #region Up/Down       
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (IsInteractable == false) return;

            OnDown?.Invoke(this, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (IsInteractable == false) return;

            OnUp?.Invoke(this, eventData);
        }
        #endregion

        #region Enter/Exit
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsInteractable == false) return;
            IsEntered = true;

            OnEnter?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsInteractable == false) return;
            IsEntered = false;

            OnExit?.Invoke(this, eventData);
        }
        #endregion
    }
}