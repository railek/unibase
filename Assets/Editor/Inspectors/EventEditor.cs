using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Railek.Unibase.Editor
{
    public abstract class EventEditor : EditorBase
    {
        protected abstract void DrawRaiseButton();

        public override void OnInspectorGUI()
        {
            DrawRaiseButton();
        }
    }

    [CustomEditor(typeof(EventBase), true)]
    public sealed class VoidEventEditor : EventEditor
    {
        private VoidEvent Target => (VoidEvent) target;

        protected override void DrawRaiseButton()
        {
            if (GUILayout.Button("Raise"))
            {
                Target.Raise();
            }
        }
    }

    [CustomEditor(typeof(EventBase<>), true)]
    public class TypedEventEditor : EventEditor
    {
        private MethodInfo _raiseMethod;

        protected override void OnEnable()
        {
            base.OnEnable();

            var memberInfo = target.GetType().BaseType;
            if (memberInfo != null)
            {
                _raiseMethod = memberInfo.GetMethod("Raise",
                    BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            }
        }

        protected override void DrawRaiseButton()
        {
            var property = serializedObject.FindProperty("debugValue");

            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(property);
                if (scope.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            if (GUILayout.Button("Raise"))
            {
                CallMethod(GetDebugValue(property));
            }
        }

        private static object GetDebugValue(SerializedProperty property)
        {
            var targetType = property.serializedObject.targetObject.GetType();
            var targetField = targetType.GetField("debugValue", BindingFlags.Instance | BindingFlags.NonPublic);

            return targetField?.GetValue(property.serializedObject.targetObject);
        }

        private void CallMethod(object value)
        {
            _raiseMethod.Invoke(target, new[] {value});
        }
    }
}
