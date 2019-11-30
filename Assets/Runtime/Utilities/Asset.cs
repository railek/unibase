using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Railek.Unibase.Utilities
{
    public static class Asset
    {
        public static T Create<T>(string relativePath) where T : ScriptableObject
        {
            if (!Directory.Exists(relativePath))
            {
                Directory.CreateDirectory(relativePath);
            }

            if (string.IsNullOrEmpty(relativePath))
            {
                return null;
            }

            var asset = ScriptableObject.CreateInstance<T>();
            var typeName = typeof(T).Name;

            AssetDatabase.CreateAsset(asset, relativePath + typeName + ".asset");
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }

        public static SceneAsset GetSceneAsset(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return null;
            }

            var sceneGuid = AssetDatabase.FindAssets($"{sceneName} t:scene").FirstOrDefault();
            var guidToAssetPath = AssetDatabase.GUIDToAssetPath(sceneGuid);

            return !string.IsNullOrEmpty(sceneGuid) ? AssetDatabase.LoadAssetAtPath<SceneAsset>(guidToAssetPath) : null;
        }
    }
}
