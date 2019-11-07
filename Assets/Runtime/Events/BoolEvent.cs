using System;
using UnityEngine;

namespace Railek.Unibase
{
    [Serializable]
    [CreateAssetMenu(fileName = "BoolEvent.asset", menuName = "Events/bool")]
    public sealed class BoolEvent : EventBase<bool>
    {
    }
}
