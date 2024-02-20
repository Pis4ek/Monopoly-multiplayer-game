using System;
using UnityEngine;
using DG.Tweening;

namespace Playmode.View
{
    public abstract class BaseInputWindow : MonoBehaviour, IAnimatable
    {
        public event Action<AnimationContainer> OnAnimationCreated;

        private float _animationTime = 0.1f;

        protected void AnimateShowing()
        {
            var animation = DOTween.Sequence();
            animation.Append(transform.DOScale(Vector3.one, _animationTime).SetEase(Ease.OutCubic));
            animation.OnPlay(() => {
                transform.localScale = new Vector3(0.01f, 0.01f, 1f);
                this.Activate();
            });

            var container = new AnimationContainer(AnimationType.Input, animation);
            OnAnimationCreated?.Invoke(container);
        }

        protected void AnimateHiding()
        {
            var animation = DOTween.Sequence();
            animation.Append(transform.DOScale(new Vector3(0.01f, 0.01f, 1f), _animationTime).SetEase(Ease.OutCubic));
            animation.OnComplete(() => {
                this.Disactivate();
            });
            animation.OnKill(() => {
                this.Disactivate();
            });

            var container = new AnimationContainer(AnimationType.Input, animation);
            OnAnimationCreated?.Invoke(container);
        }
    }
}
