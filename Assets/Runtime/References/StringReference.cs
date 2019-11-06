using System;

namespace Railek.Unibase
{
    [Serializable]
    public sealed class StringReference : Reference<string, StringVariable>
    {
        public StringReference()
        {
        }

        public StringReference(string value) : base(value)
        {
        }
    }
}
