using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Railek.Unibase.Editor
{
    [CustomEditor(typeof(Variable<>), true)]
    public sealed class VariableEditor : EditorBase
    {
        private SerializedProperty _isClamped;
        private AnimBool _isClampedVariableAnimation;
        private SerializedProperty _maxValueProperty;
        private SerializedProperty _minValueProperty;
        private SerializedProperty _valueProperty;

        private Variable Target => (Variable) target;
        private bool IsClampable => Target.Clampable;

        protected override void OnEnable()
        {
            _valueProperty = GetProperty("value");
            _isClamped = GetProperty("isClamped");
            _minValueProperty = GetProperty("minClampedValue");
            _maxValueProperty = GetProperty("maxClampedValue");

            _isClampedVariableAnimation = GetAnimBool(_isClamped.propertyPath, _isClamped.boolValue);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_valueProperty);
            DrawClampedFields();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawClampedFields()
        {
            if (!IsClampable)
            {
                return;
            }

            EditorGUILayout.PropertyField(_isClamped);
            _isClampedVariableAnimation.target = _isClamped.boolValue;

            using (var anim = new EditorGUILayout.FadeGroupScope(_isClampedVariableAnimation.faded))
            {
                if (!anim.visible)
                {
                    return;
                }

                EditorGUILayout.PropertyField(_minValueProperty);
                EditorGUILayout.PropertyField(_maxValueProperty);
            }
        }
    }
}
