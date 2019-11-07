using System;
using UnityEngine;

namespace Railek.Unibase
{
    [Serializable]
    [CreateAssetMenu(fileName = "StringEvent.asset", menuName = "Events/string")]
    public sealed class StringEvent : EventBase<string>
    {
    }
}
