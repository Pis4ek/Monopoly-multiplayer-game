using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Playmode.View
{
    public class AnimationContainer
    {
        public event Action OnStart;
        public event Action OnKill;
        public event Action OnComplete;

        public IReadOnlyList<Sequence> Animations => _animations;
        public AnimationType Type { get; private set; }
        public bool IsComplete { get; private set; } = false;

        private List<Sequence> _animations;
        private int _completedAnimsCounter = 0;


        public AnimationContainer(AnimationType type, List<Sequence> animations = null)
        {
            _animations = animations;
            if (Animations == null)
                _animations = new();

            Type = type;

            foreach(var anim in Animations)
            {
                anim.onComplete += OnOneSequenceCompleted;
            }
        }

        public AnimationContainer(AnimationType type, Sequence animation)
        {
            _animations = new() { animation };
            Type = type;

            animation.onComplete += OnOneSequenceCompleted;
        }

        public void Play()
        {
            IsComplete = false;
            OnStart?.Invoke();
            foreach (var anim in Animations)
            {
                anim.Play();
            }
        }

        public void Kill()
        {
            OnKill?.Invoke();
            foreach (var anim in Animations)
            {
                anim.Kill();
            }
            OnComplete?.Invoke();
        }

        public void Add(Sequence animation)
        {
            animation.onComplete += OnOneSequenceCompleted;
            _animations.Add(animation);
        }

        public void Remove(Sequence animation)
        {
            if (_animations.Contains(animation))
            {
                animation.onComplete -= OnOneSequenceCompleted;
                _animations.Remove(animation);
            }
            else
            {
                UnityEngine.Debug.LogError($"Tried to remove animation from container than not contains this one.");
            }
        }

        public void Clear()
        {
            foreach (var animation in _animations)
            {
                animation.onComplete -= OnOneSequenceCompleted;
            }
            _animations.Clear();
        }

        public void Merge(AnimationContainer container)
        {
            foreach (var animation in container.Animations)
            {
                Add(animation);
            }
            container.Clear();
        }

        private void OnOneSequenceCompleted()
        {
            _completedAnimsCounter++;
            if(Animations.Count == _completedAnimsCounter)
            {
                //UnityEngine.Debug.Log($"Conteiner {Type.ToString()} completed.");
                _completedAnimsCounter = 0;
                IsComplete = true;
                OnComplete?.Invoke();
            }
        }
    }
}
