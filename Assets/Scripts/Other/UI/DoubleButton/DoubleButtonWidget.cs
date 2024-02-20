using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Other
{
    public class DoubleButtonWidget : MonoBehaviour
    {
        public event Action OnAnyClick;
        public event Action OnLeftClick;
        public event Action OnRightClick;

        [SerializeField] DoubleButtonWidgetElement _leftButton;
        [SerializeField] DoubleButtonWidgetElement _rightButton;
        [SerializeField] ScaleSettings _scaleSettings;
        [SerializeField] TimeSettings _timeSettings;
        [SerializeField] EasingSettings _easingSettings;
        [SerializeField] InteractionSettings _interactionSettings;

        [field: SerializeField] public bool IsInteractable { get; private set; } = true;

        Vector2 _leftMin = new Vector2(0.1f, 0.1f);
        Vector2 _leftMax = new Vector2(0.45f, 0.9f);
        Vector2 _rightMin = new Vector2(0.55f, 0.1f);
        Vector2 _rightMax = new Vector2(0.9f, 0.9f);
        private bool[] _elementsInteractionState;

        public void Init()
        {
            if (_leftButton == null || _rightButton == null)
            {
                Debug.Log("DoubleButtonWidget requared two DoubleButtonWidgetElement refs. One of then is null.");
                return;
            }
            _leftButton.Init(_scaleSettings, _timeSettings, _easingSettings, _interactionSettings);
            _rightButton.Init(_scaleSettings, _timeSettings, _easingSettings, _interactionSettings);

            _elementsInteractionState = new bool[2] { _leftButton.IsInteractable, _rightButton.IsInteractable };

            _leftMin = _leftButton.RectTransform.anchorMin;
            _leftMax = _leftButton.RectTransform.anchorMax;
            _rightMin = _rightButton.RectTransform.anchorMin;
            _rightMax = _rightButton.RectTransform.anchorMax;

            _leftButton.OnClick += OnPointerClickOnElement;
            _leftButton.OnEnter += OnPointerEnterOnElement;
            _leftButton.OnExit += OnPointerExitOnElement;
            _leftButton.OnUp += OnPointerUpOnElement;
            _leftButton.OnDown += OnPointerDownOnElement;

            _rightButton.OnClick += OnPointerClickOnElement;
            _rightButton.OnEnter += OnPointerEnterOnElement;
            _rightButton.OnExit += OnPointerExitOnElement;
            _rightButton.OnUp += OnPointerUpOnElement;
            _rightButton.OnDown += OnPointerDownOnElement;
        }

        [Button("SetTrue")]
        public void SetTrue() => SetInteractable(true);

        [Button("SetFalse")]
        public void SetFalse() => SetInteractable(false);


        [Button("SetTrueForLeft")]
        public void SetTrueForLeft() => SetInteractableForElement(IsLeftOrRight.Left, true);

        [Button("SetFalseForLeft")]
        public void SetFalseForLeft() => SetInteractableForElement(IsLeftOrRight.Left, false);
        [Button("SetTrueForRight")]
        public void SetTrueForRight() => SetInteractableForElement(IsLeftOrRight.Left, true);

        [Button("SetFalseForRight")]
        public void SetFalseForRight() => SetInteractableForElement(IsLeftOrRight.Left, false);



        [Button("ActivateLeft")]
        public void ActivateLeft() => SetActivityForElement(IsLeftOrRight.Left, true);

        [Button("DisactivateLeft")]
        public void DisactivateLeft() => SetActivityForElement(IsLeftOrRight.Left, false); 
        [Button("ActivateRight")]
        public void ActivateRight() => SetActivityForElement(IsLeftOrRight.Right, true);

        [Button("DisactivateRight")]
        public void DisactivateRight() => SetActivityForElement(IsLeftOrRight.Right, false);

        public void SetDefaultState()
        {
            _leftButton.CurrentAnim?.Kill();
            _rightButton.CurrentAnim?.Kill();
            _leftButton.transform.localScale = _leftButton.DefaultScale;
            _rightButton.transform.localScale = _rightButton.DefaultScale;
        }

        public void SetTextInElement(IsLeftOrRight element, string text)
        {
            if (element == IsLeftOrRight.Left)
            {
                _leftButton.TextContent.text = text;
            }
            else
            {
                _rightButton.TextContent.text = text;
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            IsInteractable = isInteractable;

            if (isInteractable)
            {
                _leftButton.SetInteractable(_elementsInteractionState[0]);
                _rightButton.SetInteractable(_elementsInteractionState[1]);
            }
            else
            {
                _elementsInteractionState[0] = _leftButton.IsInteractable;
                _elementsInteractionState[1] = _rightButton.IsInteractable;


                _leftButton.SetInteractable(false);
                _rightButton.SetInteractable(false);
            }
        }

        public void SetInteractableForElement(IsLeftOrRight element, bool isInteractable)
        {
            if (element == IsLeftOrRight.Left)
            {
                if (IsInteractable)
                    _leftButton.SetInteractable(isInteractable);
                _elementsInteractionState[0] = isInteractable;
            }
            else
            {
                if (IsInteractable)
                    _rightButton.SetInteractable(isInteractable);
                _elementsInteractionState[1] = isInteractable;
            }
        }

        public void SetActivityForElement(IsLeftOrRight element, bool isActive)
        {
            if (isActive)
            {
                if(element == IsLeftOrRight.Left)
                {
                    _leftButton.Activate();
                    if (_rightButton.IsActive() == false && _interactionSettings.AllowMergeButtons)
                    {
                        _leftButton.RectTransform.anchorMax = _rightMax;
                    }
                    else
                    {
                        _rightButton.RectTransform.anchorMin = _rightMin;
                        _leftButton.RectTransform.anchorMax = _leftMax;
                    }
                }
                else
                {
                    _rightButton.Activate();
                    if (_leftButton.IsActive() == false && _interactionSettings.AllowMergeButtons)
                    {
                        _rightButton.RectTransform.anchorMin = _leftMin;
                    }
                    else
                    {
                        _leftButton.RectTransform.anchorMax = _leftMax;
                        _rightButton.RectTransform.anchorMin = _rightMin;
                    }
                }
            }
            else
            {
                if (element == IsLeftOrRight.Left)
                {
                    _leftButton.Disactivate();
                    if (_rightButton.IsActive() && _interactionSettings.AllowMergeButtons)
                    {
                        _rightButton.RectTransform.anchorMin = _leftMin;
                    }
                }
                else
                {
                    _rightButton.Disactivate();
                    if (_leftButton.IsActive() && _interactionSettings.AllowMergeButtons)
                    {
                        _leftButton.RectTransform.anchorMax = _rightMax;
                    }
                }
            }
            //_leftButton.RectTransform.sizeDelta = Vector2.zero;
            //_rightButton.RectTransform.sizeDelta = Vector2.zero;
        }

        private void OnPointerClickOnElement(DoubleButtonWidgetElement el, PointerEventData data)
        {
            if (el.IsLeftOrRight == IsLeftOrRight.Left)
                OnLeftClick?.Invoke();
            else
                OnRightClick?.Invoke();

            OnAnyClick?.Invoke();
        }

        #region Up/Down   
        private void OnPointerDownOnElement(DoubleButtonWidgetElement el, PointerEventData data)
        {
            var tween = el.transform
                    .DOScale(el.DefaultScale * _scaleSettings.DownScale, _timeSettings.DownAnimTime)
                    .SetEase(_easingSettings.DownEasing);
            el.SetAnimation(DOTween.Sequence().Append(tween));
        }
        private void OnPointerUpOnElement(DoubleButtonWidgetElement el, PointerEventData data)
        {
            if (el.IsEntered)
            {
                var tween = el.transform
                    .DOScale(el.DefaultScale * _scaleSettings.EnterScale, _timeSettings.UpAnimTime)
                    .SetEase(_easingSettings.UpEasing);
                el.SetAnimation(DOTween.Sequence().Append(tween));
            }
            else
            {
                var tween = el.transform
                    .DOScale(el.DefaultScale, _timeSettings.UpAnimTime)
                    .SetEase(_easingSettings.UpEasing);
                el.SetAnimation(DOTween.Sequence().Append(tween));
            }

            
        }
        #endregion

        #region Enter/Exit
        private void OnPointerEnterOnElement(DoubleButtonWidgetElement el, PointerEventData data)
        {
            if (el.IsLeftOrRight == IsLeftOrRight.Left)
            {
                var tween = _leftButton.transform
                    .DOScale(_leftButton.DefaultScale * _scaleSettings.EnterScale, _timeSettings.EnterAnimTime)
                    .SetEase(_easingSettings.EnterEasing);
                _leftButton.SetAnimation(DOTween.Sequence().Append(tween));

                var tween2 = _rightButton.transform
                    .DOScale(_rightButton.DefaultScale * _scaleSettings.NonEnterScale, _timeSettings.EnterAnimTime)
                    .SetEase(_easingSettings.EnterEasing);
                _rightButton.SetAnimation(DOTween.Sequence().Append(tween2));
            }
            else
            {
                var tween = _rightButton.transform
                    .DOScale(_rightButton.DefaultScale * _scaleSettings.EnterScale, _timeSettings.EnterAnimTime)
                    .SetEase(_easingSettings.EnterEasing);
                _rightButton.SetAnimation(DOTween.Sequence().Append(tween));

                var tween2 = _leftButton.transform
                    .DOScale(_leftButton.DefaultScale * _scaleSettings.NonEnterScale, _timeSettings.EnterAnimTime)
                    .SetEase(_easingSettings.EnterEasing);
                _leftButton.SetAnimation(DOTween.Sequence().Append(tween2));
            }
        }
        private void OnPointerExitOnElement(DoubleButtonWidgetElement el, PointerEventData data)
        {
            var tween = _rightButton.transform
                    .DOScale(_rightButton.DefaultScale, _timeSettings.ExitAnimTime)
                    .SetEase(_easingSettings.ExitEasing);
            _rightButton.SetAnimation(DOTween.Sequence().Append(tween));

            var tween2 = _leftButton.transform
                .DOScale(_leftButton.DefaultScale, _timeSettings.ExitAnimTime)
                .SetEase(_easingSettings.ExitEasing);
            _leftButton.SetAnimation(DOTween.Sequence().Append(tween2));
        }
        #endregion

        #region Sttings
        [Serializable]
        public class ScaleSettings
        {
            public float EnterScale = 1.1f;
            public float NonEnterScale = 0.8f;
            public float DownScale = 0.8f;
        }

        [Serializable]
        public class TimeSettings
        {
            public float EnterAnimTime = 0.17f;
            public float ExitAnimTime = 0.3f;
            public float DownAnimTime = 0.2f;
            public float UpAnimTime = 0.3f;
        }

        [Serializable]
        public class EasingSettings
        {
            public Ease EnterEasing = Ease.OutCubic;
            public Ease ExitEasing = Ease.OutCubic;
            public Ease DownEasing = Ease.OutCubic;
            public Ease UpEasing = Ease.OutCubic;
        }

        [Serializable]
        public class InteractionSettings
        {
            public bool AllowMergeButtons = true;
            public float ColorMultiplier = 0.9f;
            public float AlphaMultiplier = 0.6f;
        }
        #endregion
    }
}