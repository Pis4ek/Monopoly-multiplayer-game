using System;

namespace Playmode.PlayData
{
    public class IgnoreRentEffect : ITurnBasedEffect
    {
        public event Action OnAnyValueChanged;

        public EffectType Type => EffectType.IgnoreRent;
        public int Counter
        {
            get => _turnsToEnd;
            set
            {
                _turnsToEnd = value;
                OnAnyValueChanged?.Invoke();
            }
        }
        public float Scaler
        {
            get => _percent;
            set
            {
                _percent = value;
                OnAnyValueChanged?.Invoke();
            }
        }

        protected int _turnsToEnd = 1;
        protected float _percent = 0.25f;
    }

    public class IncreaceIncomeEffect : ITurnBasedEffect
    {
        public event Action OnAnyValueChanged;

        public EffectType Type => EffectType.IncreaceRent;
        public int Counter
        {
            get => _turnsToEnd;
            set
            {
                _turnsToEnd = value;
                OnAnyValueChanged?.Invoke();
            }
        }
        public float Scaler
        {
            get => _percent;
            set
            {
                _percent = value;
                OnAnyValueChanged?.Invoke();
            }
        }

        protected int _turnsToEnd = 1;
        protected float _percent = 1.5f;
    }

    public class DecreaceIncomeEffect : ITurnBasedEffect
    {
        public event Action OnAnyValueChanged;

        public EffectType Type => EffectType.DecreaceRent;
        public int Counter
        {
            get => _turnsToEnd;
            set
            {
                _turnsToEnd = value;
                OnAnyValueChanged?.Invoke();
            }
        }
        public float Scaler
        {
            get => _percent;
            set
            {
                _percent = value;
                OnAnyValueChanged?.Invoke();
            }
        }

        protected int _turnsToEnd = 1;
        protected float _percent = 0.5f;
    }

    public class ReversiveMoveEffect : IUseBasedEffect
    {
        public event Action OnAnyValueChanged;

        public EffectType Type => EffectType.ReverceMove;
        public int Counter
        {
            get => _usesToEnd;
            set
            {
                _usesToEnd = value;
                OnAnyValueChanged?.Invoke();
            }
        }

        protected int _usesToEnd = 1;
    }

    public class SkipTurnEffect : IUseBasedEffect
    {
        public event Action OnAnyValueChanged;

        public EffectType Type => EffectType.SkipTurn;
        public int Counter
        {
            get => _usesToEnd;
            set
            {
                _usesToEnd = value;
                OnAnyValueChanged?.Invoke();
            }
        }

        protected int _usesToEnd = 1;
    }
}
