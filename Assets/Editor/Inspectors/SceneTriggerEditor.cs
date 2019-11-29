using Railek.Unibase.Utilities;
using UnityEditor;
using UnityEngine;

namespace Railek.Unibase.Editor
{
    [CustomEditor(typeof(SceneTrigger))]
    public class SceneTriggerEditor : EditorBase
    {
        private bool _addMode;
        private SerializedProperty _visibleScenes;
        private SerializedProperty _unselectedGizmoColor;
        private SerializedProperty _selectedGizmoColor;

        protected override void OnEnable()
        {
            _visibleScenes = GetProperty("visibleScenes");
            _unselectedGizmoColor = GetProperty("unselectedGizmoColor");
            _selectedGizmoColor = GetProperty("selectedGizmoColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(16);
            EditorGUILayout.PropertyField(_unselectedGizmoColor);
            EditorGUILayout.PropertyField(_selectedGizmoColor);

            SceneListPropertyField(_visibleScenes);

            serializedObject.ApplyModifiedProperties();
        }

        private void SceneListPropertyField(SerializedProperty listProperty)
        {
            GUILayout.Space(16);
            EditorGUILayout.HelpBox("Visible Scenes:", MessageType.Info);

            for (var i = 0; i < listProperty.arraySize; i++)
            {
                var elementProperty = listProperty.GetArrayElementAtIndex(i);
                elementProperty.stringValue = SceneListPropertyObject(elementProperty.stringValue, i);

                if (!string.IsNullOrEmpty(elementProperty.stringValue))
                {
                    continue;
                }

                listProperty.DeleteArrayElementAtIndex(i);
                return;
            }

            if (_addMode)
            {
                var newSceneName = SceneListPropertyObject(null, listProperty.arraySize);
                if (!string.IsNullOrEmpty(newSceneName))
                {
                    listProperty.arraySize++;
                    listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).stringValue = newSceneName;
                    _addMode = false;
                }
            }
            else
            {
                AddSceneButton();
            }

            GUILayout.Space(8);
        }

        private string SceneListPropertyObject(string sceneName, int positionInArray)
        {
            var sceneAsset = Asset.GetSceneAsset(sceneName);

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.PrefixLabel(new GUIContent($"Scene {positionInArray}"));
                sceneAsset = (SceneAsset) EditorGUILayout.ObjectField(sceneAsset, typeof(SceneAsset), false);

                var removeSceneFromList = GUILayout.Button(new GUIContent("-"));
                if (removeSceneFromList)
                {
                    sceneAsset = null;
                    _addMode = false;
                }
            }
            EditorGUILayout.EndHorizontal();

            return sceneAsset != null ? sceneAsset.name : "";
        }

        private void AddSceneButton()
        {
            bool enterAddMode;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space();
                enterAddMode = GUILayout.Button("Add Scene", GUILayout.Width(75));
            }
            EditorGUILayout.EndHorizontal();

            if (enterAddMode)
            {
                _addMode = true;
            }
        }
    }
}
