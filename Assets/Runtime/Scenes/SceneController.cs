using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Railek.Unibase.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Railek.Unibase
{
    public class SceneController : SingletonMonoBehaviour<SceneController>
    {
        public List<Scene> fullyLoadedScenes = new List<Scene>();
        public readonly Dictionary<string, AsyncOperation> AsyncOperations = new Dictionary<string, AsyncOperation>();
        public List<string> scenes = new List<string>();
        public List<string> loadAtStartScenes = new List<string>();
        public List<string> persistentScenes = new List<string>();

        private void Start()
        {
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            foreach (var sceneName in loadAtStartScenes)
            {
                LoadLevel(sceneName, true);
            }
        }

        public bool SceneExists(string sceneName)
        {
            return scenes.Contains(sceneName, StringComparer.OrdinalIgnoreCase);
        }

        public void LoadLevel(string sceneName)
        {
            StartCoroutine(LoadLevelCoroutine(sceneName));
        }

        private IEnumerator LoadLevelCoroutine(string sceneName)
        {
            yield return new WaitForEndOfFrame();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (sceneName == SceneManager.GetSceneAt(i).name)
                {
                    yield break;
                }
            }

            var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            async.allowSceneActivation = true;

            AsyncOperations.Add(sceneName, async);

            yield return async;

            StartCoroutine(SetLoadedScene(sceneName));
        }

        public void LoadLevel(string sceneName, bool allowSceneActivation)
        {
            StartCoroutine(LoadLevelCoroutine(sceneName, allowSceneActivation));
        }

        private IEnumerator LoadLevelCoroutine(string sceneName, bool allowSceneActivation)
        {
            yield return new WaitForEndOfFrame();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (sceneName == SceneManager.GetSceneAt(i).name)
                {
                    yield break;
                }
            }

            var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            async.allowSceneActivation = allowSceneActivation;

            AsyncOperations.Add(sceneName, async);

            yield return async;

            StartCoroutine(SetLoadedScene(sceneName));
        }

        private IEnumerator SetLoadedScene(string sceneName)
        {
            yield return new WaitForEndOfFrame();
            AsyncOperations[sceneName] = null;
            AsyncOperations.Remove(sceneName);
            fullyLoadedScenes.Add(SceneManager.GetSceneByName(sceneName));
        }

        public void UnloadLevels()
        {
            StartCoroutine(UnloadLevelsCoroutine());
        }

        private IEnumerator UnloadLevelsCoroutine()
        {
            yield return new WaitForEndOfFrame();

            var scenesToUnload = new List<string>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (persistentScenes.Contains(SceneManager.GetSceneAt(i).name) == false)
                {
                    scenesToUnload.Add(SceneManager.GetSceneAt(i).name);
                }
            }

            if (scenesToUnload.Count == 0)
            {
                yield break;
            }

            foreach (var sceneName in scenesToUnload)
            {
                fullyLoadedScenes.Remove(SceneManager.GetSceneByName(sceneName));
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        public void UnloadLevels(string exception)
        {
            StartCoroutine(UnloadLevelsCoroutine(exception));
        }

        private IEnumerator UnloadLevelsCoroutine(string exception)
        {
            yield return new WaitForEndOfFrame();

            var scenesToUnload = new List<string>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (persistentScenes.Contains(SceneManager.GetSceneAt(i).name))
                {
                    continue;
                }

                if (exception != SceneManager.GetSceneAt(i).name == false)
                {
                    scenesToUnload.Add(SceneManager.GetSceneAt(i).name);
                }
            }

            if (scenesToUnload.Count == 0)
            {
                yield break;
            }

            foreach (var sceneName in scenesToUnload)
            {
                fullyLoadedScenes.Remove(SceneManager.GetSceneByName(sceneName));
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        public void UnloadLevels(string[] exceptions)
        {
            StartCoroutine(UnloadLevelsCoroutine(exceptions));
        }

        private IEnumerator UnloadLevelsCoroutine(string[] exceptions)
        {
            yield return new WaitForEndOfFrame();

            var scenesToUnload = new List<string>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (persistentScenes.Contains(SceneManager.GetSceneAt(i).name))
                {
                    continue;
                }

                if (exceptions.Contains(SceneManager.GetSceneAt(i).name) == false)
                {
                    scenesToUnload.Add(SceneManager.GetSceneAt(i).name);
                }
            }

            if (scenesToUnload.Count == 0)
            {
                yield break;
            }

            foreach (var sceneName in scenesToUnload)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }
}
