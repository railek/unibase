using UnityEngine;

namespace Railek.Unibase
{
    [CreateAssetMenu(fileName = "IntVariable.asset", menuName = "Variables/int")]
    public class IntVariable : Variable<int>
    {
        public override bool Clampable => true;

        protected override int ClampValue(int v)
        {
            if (v.CompareTo(MinClampValue) < 0)
            {
                return MinClampValue;
            }

            return v.CompareTo(MaxClampValue) > 0 ? MaxClampValue : v;
        }
    }
}
