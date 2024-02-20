using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRx
{
    public class ReactiveProperty<T>
    {
        public event Action<T> OnValueChanged;

        T _value = default(T);

        public bool _isDisposed { get; private set; } = false;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    if (_isDisposed)
                        return;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public ReactiveProperty()
            : this(default(T))
        {
        }

        public ReactiveProperty(T initialValue)
        {
            _value = initialValue;
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;
        }

        public override string ToString()
        {
            return (_value == null) ? "(null)" : _value.ToString();
        }
    }
}