using System;
using System.Collections.Generic;
using UniRx;

namespace Playmode.PlayData
{
    public interface IPlayer
    {
        public event Action<IPlayer> OnAnyValueChanged;

        public string Name { get; }
        public PlayerID ID { get; }
        public int Cash { get; }
        public PlayerState State { get; }
        public ICell CurrentCell { get; }
        public ReactiveDictionary<Type, IEffect> Effects { get; }

        public bool HasEffect<T>() where T : IEffect;

        public bool HasEffect(Type type);

        public bool TryGetEffect<T>(out T effect) where T : class, IEffect;

        public void Update(int cash, PlayerState state, ICell currentCell);
    }
}
