using UnityEngine;

namespace Railek.Unibase.Helpers
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
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

                _instance = (T) FindObjectOfType(typeof(T));
                return _instance;
            }
        }

        private void Awake()
        {
            if (CheckInstance())
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private bool CheckInstance()
        {
            if (this == Instance)
            {
                return true;
            }

            Destroy(this);
            return false;
        }
    }
}
