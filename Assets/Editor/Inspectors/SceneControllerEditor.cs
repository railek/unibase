using System.Collections.Generic;
using System.Linq;
using Railek.Unibase.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Railek.Unibase.Editor
{
    [CustomEditor(typeof(SceneController))]
    public class SceneControllerEditor : EditorBase
    {
        private readonly List<string> _sceneList = new List<string>();
        private bool _allowSceneActivation;

        private Color _defaultBackgroundColor;
        private SerializedProperty _loadAtStartScenes;
        private SerializedProperty _persistentScenes;
        private SceneController _sceneController;

        protected override void OnEnable()
        {
            _sceneController = (SceneController) target;
            _loadAtStartScenes = GetProperty("loadAtStartScenes");
            _persistentScenes = GetProperty("persistentScenes");

            GenerateSceneList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            LoadAtStartScenes();
            PersistentScenes();
            CurrentlyLoadedScenes();
            LightmapOptions();
            serializedObject.ApplyModifiedProperties();
        }

        private void GenerateSceneList()
        {
            var scenes = AssetDatabase.FindAssets("t:scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>)
                .Select(s => s.name)
                .ToList();

            _sceneController.scenes = scenes;
        }

        private void UnloadScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (Application.isPlaying)
            {
                _sceneController.fullyLoadedScenes.Remove(scene);
                SceneManager.UnloadSceneAsync(sceneName);
            }
            else
            {
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        private void LoadScene(string sceneName)
        {
            if (Application.isPlaying)
            {
                _sceneController.LoadLevel(sceneName, true);
            }
            else
            {
                var scenePath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(sceneName + " t:scene")[0]);
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
        }

        private void PersistentScenes()
        {
            GUILayout.Space(16);
            EditorGUILayout.HelpBox("Persistent Scenes", MessageType.Info);
            SceneListPropertyField(_persistentScenes);
        }

        private void LoadAtStartScenes()
        {
            GUILayout.Space(16);
            EditorGUILayout.HelpBox("Scenes to load at Start", MessageType.Info);
            SceneListPropertyField(_loadAtStartScenes);
        }

        private static void LightmapOptions()
        {
            var paths = new string[SceneManager.sceneCount];
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                paths[i] = SceneManager.GetSceneAt(i).path;
            }

            if (GUILayout.Button(new GUIContent("Bake Lightmaps On All Open Scenes")))
            {
                Lightmapping.BakeMultipleScenes(paths);
            }
        }

        private void SceneListPropertyField(SerializedProperty listProperty)
        {
            for (var i = 0; i < listProperty.arraySize; i++)
            {
                var elementProperty = listProperty.GetArrayElementAtIndex(i);
                elementProperty.stringValue =
                    SceneListPropertyObject(elementProperty.stringValue, i, listProperty);

                if (!string.IsNullOrEmpty(elementProperty.stringValue))
                {
                    continue;
                }

                listProperty.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                GenerateSceneList();
                return;
            }

            if (_sceneList.Contains(listProperty.name))
            {
                var newSceneName = SceneListPropertyObject(null, listProperty.arraySize, listProperty);

                if (string.IsNullOrEmpty(newSceneName))
                {
                    return;
                }

                listProperty.arraySize++;
                listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).stringValue = newSceneName;
                _sceneList.Remove(listProperty.name);
                serializedObject.ApplyModifiedProperties();
                GenerateSceneList();
            }
            else
            {
                AddSceneButton(listProperty);
            }
        }

        private string SceneListPropertyObject(string sceneName, int positionInArray, SerializedProperty listProperty)
        {
            var sceneAsset = Asset.GetSceneAsset(sceneName);

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.PrefixLabel(new GUIContent($"Scene {positionInArray}"));

                sceneAsset = (SceneAsset) EditorGUILayout.ObjectField(sceneAsset, typeof(SceneAsset), false);

                if (GUILayout.Button(new GUIContent("-")))
                {
                    sceneAsset = null;
                    if (_sceneList.Contains(listProperty.name))
                    {
                        _sceneList.Remove(listProperty.name);
                    }

                    serializedObject.ApplyModifiedProperties();
                    GenerateSceneList();
                }
            }
            EditorGUILayout.EndHorizontal();

            return sceneAsset != null ? sceneAsset.name : "";
        }

        private void AddSceneButton(SerializedProperty property)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Add Scene", GUILayout.Width(75)))
                {
                    _sceneList.Add(property.name);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void CurrentlyLoadedScenes()
        {
            GUILayout.Space(16);
            EditorGUILayout.HelpBox("Currently Loaded Scenes", MessageType.Info);

            _defaultBackgroundColor = GUI.backgroundColor;

            var inspectorSceneCount = 0;
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                CurrentlyLoadedScene(SceneManager.GetSceneAt(i), i);
                inspectorSceneCount++;
            }

            GUI.backgroundColor = _defaultBackgroundColor;
            LoadNewScene();

            if (inspectorSceneCount != _sceneController.fullyLoadedScenes.Count)
            {
                Repaint();
            }
        }

        private void LoadNewScene()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.PrefixLabel("Load New Scene:");
                var newScene = EditorGUILayout.ObjectField(null, typeof(SceneAsset), false);
                if (newScene != null)
                {
                    LoadScene(newScene.name);
                }
                GUILayout.Space(24);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void CurrentlyLoadedScene(Scene scene, int position)
        {
            var showUnloadButton = !_sceneController.AsyncOperations.ContainsKey(scene.name);

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.PrefixLabel("Scene " + position);
                EditorGUI.BeginDisabledGroup(true);
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                EditorGUILayout.ObjectField(sceneAsset, typeof(SceneAsset), false);

                if (position != 0)
                {
                    EditorGUI.EndDisabledGroup();
                }

                if (showUnloadButton)
                {
                    if (GUILayout.Button(new GUIContent("-")))
                    {
                        UnloadScene(scene.name);
                    }
                }

                if (position == 0)
                {
                    EditorGUI.EndDisabledGroup();
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
