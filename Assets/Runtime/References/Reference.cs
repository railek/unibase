using System;
using UnityEngine;

namespace Railek.Unibase
{
    [Serializable]
    public class Reference<TBase, TVariable> : Reference where TVariable : Variable<TBase>
    {
        [SerializeField] protected TBase constantValue;
        [SerializeField] protected bool useConstant;
        [SerializeField] protected TVariable variable;

        public Reference()
        {
        }

        public Reference(TBase baseValue)
        {
            useConstant = true;
            constantValue = baseValue;
        }

        public TBase Value
        {
            get => useConstant || variable == null ? constantValue : variable.Value;
            set
            {
                if (!useConstant && variable != null)
                {
                    variable.Value = value;
                }
                else
                {
                    useConstant = true;
                    constantValue = value;
                }
            }
        }

        public Reference CreateCopy()
        {
            var copy = (Reference<TBase, TVariable>) Activator.CreateInstance(GetType());
            copy.useConstant = useConstant;
            copy.constantValue = constantValue;
            copy.variable = variable;

            return copy;
        }

        public void AddListener(IEventListener listener)
        {
            if (variable != null)
            {
                variable.AddListener(listener);
            }
        }

        public void RemoveListener(IEventListener listener)
        {
            if (variable != null)
            {
                variable.RemoveListener(listener);
            }
        }

        public void AddListener(Action action)
        {
            if (variable != null)
            {
                variable.AddListener(action);
            }
        }

        public void RemoveListener(Action action)
        {
            if (variable != null)
            {
                variable.RemoveListener(action);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public abstract class Reference
    {
    }
}
