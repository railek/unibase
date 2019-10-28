using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Railek.Unibase.Editor
{
    public class EditorBase : UnityEditor.Editor
    {
        private readonly Dictionary<string, AnimBool> _animBools = new Dictionary<string, AnimBool>();

        private readonly Dictionary<string, SerializedProperty> _serializedProperties =
            new Dictionary<string, SerializedProperty>();

        protected virtual void OnEnable()
        {
            LoadSerializedProperty();
            InitAnimBool();
        }

        protected virtual void OnDisable()
        {
        }

        protected AnimBool GetAnimBool(string key, bool defaultValue = false)
        {
            if (_animBools.ContainsKey(key))
            {
                return _animBools[key];
            }

            _animBools.Add(key, new AnimBool(defaultValue, Repaint));
            return _animBools[key];
        }

        protected SerializedProperty GetProperty(string propertyName)
        {
            var key = propertyName;
            if (_serializedProperties.ContainsKey(key))
            {
                return _serializedProperties[key];
            }

            var s = serializedObject.FindProperty(propertyName);
            if (s == null)
            {
                return null;
            }

            _serializedProperties.Add(key, s);
            return s;
        }

        protected SerializedProperty GetProperty(string propertyName, SerializedProperty parentProperty)
        {
            var key = parentProperty.propertyPath + "." + propertyName;
            if (_serializedProperties.ContainsKey(key))
            {
                return _serializedProperties[key];
            }

            var s = parentProperty.FindPropertyRelative(propertyName);
            if (s == null)
            {
                return null;
            }

            _serializedProperties.Add(key, s);
            return s;
        }

        public override void OnInspectorGUI()
        {
        }

        protected virtual void InitAnimBool()
        {
        }

        protected virtual void LoadSerializedProperty()
        {
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }
    }
}
