using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Railek.Unibase.Editor
{
    [CustomPropertyDrawer(typeof(Reference), true)]
    public class ReferenceDrawer : PropertyDrawer
    {
        private const BindingFlags NonPublicBindingsFlag = BindingFlags.Instance | BindingFlags.NonPublic;
        private const string ConstantValuePropertyName = "constantValue";

        private static readonly string[] PopupOptions =
        {
            "Use Constant",
            "Use Variable"
        };

        private SerializedProperty _constantValue;
        private SerializedProperty _useConstant;
        private SerializedProperty _variable;

        private Type ValueType => GetValueType(fieldInfo);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            _useConstant = property.FindPropertyRelative("useConstant");
            _constantValue = property.FindPropertyRelative("constantValue");
            _variable = property.FindPropertyRelative("variable");

            var buttonRect = new Rect(position);
            buttonRect.yMin += Styles.PopupStyle.margin.top;
            buttonRect.width = Styles.PopupStyle.fixedWidth + Styles.PopupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var result = EditorGUI.Popup(buttonRect, _useConstant.boolValue ? 0 : 1, PopupOptions, Styles.PopupStyle);

            _useConstant.boolValue = result == 0;

            if (_useConstant.boolValue)
            {
                DrawGenericPropertyField(position);
            }
            else
            {
                EditorGUI.PropertyField(position, _variable, GUIContent.none);
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private void DrawGenericPropertyField(Rect position)
        {
            EditorGUI.PropertyField(position, _constantValue, GUIContent.none);
        }

        private static Type GetValueType(FieldInfo fieldInfo)
        {
            var referenceType = GetReferenceType(fieldInfo);

            if (referenceType.IsArray)
            {
                referenceType = referenceType.GetElementType();
            }
            else if (IsList(referenceType))
            {
                referenceType = referenceType.GetGenericArguments()[0];
            }

            var constantValueField = referenceType?.GetField(ConstantValuePropertyName, NonPublicBindingsFlag);

            return constantValueField?.FieldType;
        }

        private static Type GetReferenceType(FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType;
        }

        private static bool IsList(Type referenceType)
        {
            return referenceType.IsGenericType;
        }

        private static class Styles
        {
            static Styles()
            {
                PopupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
                {
                    imagePosition = ImagePosition.ImageOnly
                };
            }

            public static GUIStyle PopupStyle { get; }
        }
    }
}
