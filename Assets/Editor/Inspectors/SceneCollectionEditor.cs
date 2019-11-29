using Railek.Unibase.Utilities;
using UnityEditor;
using UnityEngine;

namespace Railek.Unibase.Editor
{
    [CustomEditor(typeof(SceneCollection))]
    public class SceneCollectionEditor : EditorBase
    {
        [MenuItem("Scene/Save Scene Setup")]
        public static void SaveSetup()
        {
            var setupFile = Asset.Create<SceneCollection>("Assets/Resources/");
            setupFile.SaveSetup();
        }

        private SerializedProperty _setupProperty;

        protected override void OnEnable()
        {
            _setupProperty = GetProperty("setup");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var setup = target as SceneCollection;

            EditorGUILayout.PropertyField(_setupProperty, true);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save Current Setup"))
            {
                if (setup != null)
                {
                    setup.SaveSetup();
                }
            }

            if (GUILayout.Button("Load"))
            {
                if (setup != null)
                {
                    setup.LoadSetup();
                }
            }

            if (GUILayout.Button("Load (Inclusive)"))
            {
                if (setup != null)
                {
                    setup.LoadSetupInclusive();
                }
            }

            GUILayout.EndHorizontal();
        }
    }
}
