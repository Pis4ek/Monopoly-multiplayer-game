using System;

namespace Playmode.PlayData
{
    public interface IEffect 
    {
        public event Action OnAnyValueChanged;

        public EffectType Type { get; }
        public int Counter { get; set; }
    }

    public interface IUseBasedEffect : IEffect
    {
    }

    public interface ITurnBasedEffect : IEffect
    {
    }
}
