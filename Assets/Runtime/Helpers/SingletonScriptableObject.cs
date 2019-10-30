using System.IO;
using Railek.Unibase.Utilities;
using UnityEngine;

namespace Railek.Unibase.Helpers
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var typeName = typeof(T).Name;
                var classPath = $"{Application.dataPath}/Resources/{typeName}.asset";

                if (!File.Exists(classPath))
                {
                    var createInstance = Asset.Create<T>("Assets/Resources/");
                    _instance = createInstance;
                    return createInstance;
                }

                var loadInstance = (T) Resources.Load(typeName, typeof(T));
                _instance = loadInstance;
                return loadInstance;
            }
        }
    }
}
