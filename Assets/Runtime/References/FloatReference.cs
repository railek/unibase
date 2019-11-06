using System;

namespace Railek.Unibase
{
    [Serializable]
    public sealed class FloatReference : Reference<float, FloatVariable>
    {
        public FloatReference()
        {
        }

        public FloatReference(float value) : base(value)
        {
        }
    }
}
