using UnityEditor;
using UnityEngine;

namespace Railek.Unibase.Editor
{
    public abstract class EventListenerEditor : EditorBase
    {
        private SerializedProperty _event;
        private SerializedProperty _response;

        protected override void OnEnable()
        {
            _event = GetProperty("event");
            _response = GetProperty("response");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.ObjectField(_event, new GUIContent("Event"));
            EditorGUILayout.PropertyField(_response, new GUIContent("Response"));

            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(EventListener<,>), true)]
    public class VoidEventListenerEditor : EventListenerEditor
    {
    }

    [CustomEditor(typeof(EventListener<,,>), true)]
    public class TypedEventListenerEditor : EventListenerEditor
    {
    }
}
