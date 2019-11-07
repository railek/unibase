using System;
using UnityEngine;

namespace Railek.Unibase
{
    [Serializable]
    [CreateAssetMenu(fileName = "IntEvent.asset", menuName = "Events/int")]
    public sealed class IntEvent : EventBase<int>
    {
    }
}
