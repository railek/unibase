using System;
using UnityEngine;

namespace Railek.Unibase
{
    [Serializable]
    [CreateAssetMenu(fileName = "FloatEvent.asset", menuName = "Events/float")]
    public sealed class FloatEvent : EventBase<float>
    {
    }
}
