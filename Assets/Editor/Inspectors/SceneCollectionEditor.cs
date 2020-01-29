using Railek.Unibase.Utilities;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Railek.Unibase.Editor
{
    [CustomEditor(typeof(SceneCollection))]
    public class SceneCollectionEditor : EditorBase
    {
        [MenuItem("Railek/Save Scene Setup")]
        public static void CreateSceneSetup()
        {
            Create<SceneCollection>("Assets/Resources/");
        }

        public static T Create<T>(string relativePath) where T : SceneCollection
        {
            if (!Directory.Exists(relativePath))
            {
                Directory.CreateDirectory(relativePath);
            }

            if (string.IsNullOrEmpty(relativePath))
            {
                return null;
            }

            var asset = CreateInstance<T>();
            asset.SaveSetup();

            var typeName = typeof(T).Name;

            AssetDatabase.CreateAsset(asset, relativePath + typeName + ".asset");
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            return asset;
        }

        private SerializedProperty _setupProperty;
        private SceneCollection _collection;

        protected override void OnEnable()
        {
            _setupProperty = GetProperty("setup");
        }

        public override void OnInspectorGUI()
        {
            _collection = (SceneCollection)target;

            EditorGUILayout.PropertyField(_setupProperty, true);

            DrawButtons(_collection);

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawButtons(SceneCollection collection)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Load"))
            {
                if (collection != null)
                {
                    collection.LoadSetup();
                }
            }

            if (GUILayout.Button("Load (Inclusive)"))
            {
                if (collection != null)
                {
                    collection.LoadSetupInclusive();
                }
            }

            GUILayout.EndHorizontal();
        }
    }
}
