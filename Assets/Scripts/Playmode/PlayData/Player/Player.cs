using System;
using System.Collections.Generic;
using UniRx;

namespace Playmode.PlayData
{
    public class Player : IPlayer
    {
        public event Action<IPlayer> OnAnyValueChanged;

        public string Name { get; private set; }
        public PlayerID ID { get; private set; }
        public int Cash { 
            get => _cash;
            set {
                _cash = value;
                OnAnyValueChanged?.Invoke(this);
            } 
        }
        public PlayerState State { 
            get => _state; 
            set { 
                _state = value;
                OnAnyValueChanged?.Invoke(this);
            } 
        }
        public ICell CurrentCell { 
            get => _currentCell;
            set
            {
                _currentCell = value;
                OnAnyValueChanged?.Invoke(this);
            }
        }
        public ReactiveDictionary<Type, IEffect> Effects { get; private set; } = new();

        private int _cash = 100000;
        private PlayerState _state = PlayerState.Default;
        private ICell _currentCell;

        public Player(string name, PlayerID playerID, ICell startCell)
        {
            Name = name;
            ID = playerID;
            CurrentCell = startCell;
            Effects.ObserveAdd().Subscribe(SubscribeEffect);
            Effects.ObserveRemove().Subscribe(UnsubscribeEffect);
        }

        public bool HasEffect<T>() where T : IEffect 
        { 
            return Effects.ContainsKey(typeof(T));
        }

        public bool HasEffect(Type type)
        {
            return Effects.ContainsKey(type);
        }

        public bool TryGetEffect<T>(out T effect) where T : class, IEffect
        {
            effect = null;
            if(Effects.TryGetValue(typeof(T), out var e))
            {
                effect = e as T;
                return true;
            }
            return false;
        }

        public void Update(int  cash, PlayerState state, ICell currentCell)
        {
            Cash = cash;
            State = state;
            CurrentCell = currentCell;

            OnAnyValueChanged?.Invoke(this);
        }

        #region Subsribing
        private void SubscribeEffect(DictionaryAddEvent<Type, IEffect> ev)
        {
            InvokeAnyChangedEvent();
            ev.Value.OnAnyValueChanged += InvokeAnyChangedEvent;
        }

        private void UnsubscribeEffect(DictionaryRemoveEvent<Type, IEffect> ev)
        {
            InvokeAnyChangedEvent();
            ev.Value.OnAnyValueChanged -= InvokeAnyChangedEvent;
        }

        private void InvokeAnyChangedEvent() => OnAnyValueChanged?.Invoke(this);
        #endregion

        public override string ToString()
        {
            var effectMes = "";
            foreach (var effect in Effects)
            {
                if (effect.Value is ITurnBasedEffect e)
                    effectMes += $"Turn.{effect.Key}:{e.Counter} , ";
                if (effect.Value is IUseBasedEffect ef)
                    effectMes += $"Use.{effect.Key}:{ef.Counter} , ";
            }

            return $"Name - {Name}\n" +
                $"ID - {ID}\n" +
                $"State - {State}\n" +
                $"CurrentCell - {CurrentCell.Index}\n" +
                $"Cash - {Cash}\n" +
                $"Effects - {effectMes}\n";
        }
    }
}