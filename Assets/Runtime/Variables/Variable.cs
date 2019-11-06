using UnityEngine;

namespace Railek.Unibase
{
    public abstract class Variable : EventBase
    {
        public abstract bool Clampable { get; }
    }

    public abstract class Variable<T> : Variable
    {
        [SerializeField] protected bool isClamped;
        [SerializeField] protected T maxClampedValue;
        [SerializeField] protected T minClampedValue;
        [SerializeField] protected T value;

        public T Value
        {
            get => value;
            set => this.value = SetValue(value);
        }

        protected T MinClampValue => Clampable ? minClampedValue : default;

        protected T MaxClampValue => Clampable ? maxClampedValue : default;

        public override bool Clampable => false;

        private bool IsClamped => isClamped;

        private T SetValue(T v)
        {
            if (Clampable && IsClamped)
            {
                return ClampValue(v);
            }

            return v;
        }

        protected virtual T ClampValue(T v)
        {
            return v;
        }

        public override string ToString()
        {
            return value == null ? "null" : value.ToString();
        }

        public static implicit operator T(Variable<T> variable)
        {
            return variable.Value;
        }
    }
}
