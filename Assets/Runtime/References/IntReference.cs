using System;

namespace Railek.Unibase
{
    [Serializable]
    public sealed class IntReference : Reference<int, IntVariable>
    {
        public IntReference()
        {
        }

        public IntReference(int value) : base(value)
        {
        }
    }
}
