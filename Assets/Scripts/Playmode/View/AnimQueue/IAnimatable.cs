using System;

namespace Playmode.View
{
    public interface IAnimatable
    {
        public event Action<AnimationContainer> OnAnimationCreated;
    }
}
