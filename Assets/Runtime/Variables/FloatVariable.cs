using UnityEngine;

namespace Railek.Unibase
{
    [CreateAssetMenu(fileName = "FloatVariable.asset", menuName = "Variables/float")]
    public class FloatVariable : Variable<float>
    {
        public override bool Clampable => true;

        protected override float ClampValue(float v)
        {
            if (v.CompareTo(MinClampValue) < 0)
            {
                return MinClampValue;
            }

            return v.CompareTo(MaxClampValue) > 0 ? MaxClampValue : v;
        }
    }
}
