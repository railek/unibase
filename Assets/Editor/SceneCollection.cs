using UnityEngine;
using UnityEditor.SceneManagement;

namespace Railek.Unibase.Editor
{
    public class SceneCollection : ScriptableObject
    {
        public SceneSetup[] setup;

        public void SaveSetup()
        {
            setup = EditorSceneManager.GetSceneManagerSetup();
        }

        public void LoadSetup()
        {
            EditorSceneManager.RestoreSceneManagerSetup(setup);
        }

        public void LoadSetupInclusive()
        {
            var current = EditorSceneManager.GetSceneManagerSetup();
            var newSetup = new SceneSetup[current.Length + setup.Length];

            var newSetupIndex = 0;
            foreach (var t in current)
            {
                newSetup[newSetupIndex] = t;
                newSetupIndex++;
            }

            foreach (var t in setup)
            {
                newSetup[newSetupIndex] = new SceneSetup
                {
                    path = t.path,
                    isLoaded = t.isLoaded,
                    isActive = false
                };
                newSetupIndex++;
            }

            EditorSceneManager.RestoreSceneManagerSetup(newSetup);
        }
    }
}