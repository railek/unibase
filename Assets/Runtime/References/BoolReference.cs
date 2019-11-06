using System;

namespace Railek.Unibase
{
    [Serializable]
    public sealed class BoolReference : Reference<bool, BoolVariable>
    {
        public BoolReference()
        {
        }

        public BoolReference(bool value) : base(value)
        {
        }
    }
}
