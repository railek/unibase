using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Railek.Unibase.Editor
{
    public class PropertyRelative
    {
        private readonly Dictionary<string, SerializedProperty> _childProperties = new Dictionary<string, SerializedProperty>();

        public void Add(string propertyName, SerializedProperty parentProperty)
        {
            var key = parentProperty.propertyPath + "." + propertyName;

            if (_childProperties.ContainsKey(key))
            {
                return;
            }

            var s = parentProperty.FindPropertyRelative(propertyName);
            if (s == null)
            {
                Debug.Log("Property '" + key + "' was not found.");
                return;
            }

            _childProperties.Add(key, s);
        }

        public SerializedProperty Get(string propertyName, SerializedProperty parentProperty)
        {
            var key = parentProperty.propertyPath + "." + propertyName;
            return _childProperties[key];
        }
    }
}
