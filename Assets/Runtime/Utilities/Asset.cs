using System.IO;
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
    }
}
