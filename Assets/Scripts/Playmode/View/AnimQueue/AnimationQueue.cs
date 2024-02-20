using System.Collections.Generic;

namespace Playmode.View
{
    public enum AnimationType { Cubes, PlayerOnMap, Input, PopUp}
    public class AnimationQueue
    {
        private Queue<AnimationContainer> _queue = new();
        private List<AnimationType> _momentalInvokedAnimations;
        private AnimationContainer _currentAnimation;

        public AnimationQueue(List<IAnimatable> animatables)
        {
            _momentalInvokedAnimations = new() { AnimationType.PopUp };
            foreach (var animatable in animatables)
            {
                animatable.OnAnimationCreated += EnterQueue;
            }
        }

        private void EnterQueue(AnimationContainer container)
        {
            if (_queue.Count == 0)
            {
                if (_currentAnimation == null || _currentAnimation.IsComplete)
                {
                    _currentAnimation = container;
                    _currentAnimation.OnComplete += GetAnimation;
                    container.Play();
                    return;
                }
            }

            if (_momentalInvokedAnimations.Contains(container.Type))
            {
                container.Play();
            }
            else
            {
                _queue.Enqueue(container);
            }
        }

        private void GetAnimation()
        {
            _currentAnimation.OnComplete -= GetAnimation;

            if(_queue.Count > 0)
            {
                _currentAnimation = _queue.Dequeue();
                _currentAnimation.OnComplete += GetAnimation;
                _currentAnimation.Play();
            }
        }
    }
}
