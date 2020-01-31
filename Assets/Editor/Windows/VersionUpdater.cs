using System.Globalization;
using UnityEngine;
using UnityEditor;

namespace Railek.Unibase.Editor
{
    public class VersionUpdater : EditorWindow
    {
        private string versionNumber;
        private string lastVersion;
        private bool changed;

        [MenuItem("Railek/Bundle and Version Updater")]
        public static void ShowWindow()
        {
            GetWindow(typeof(VersionUpdater));
        }

        void OnGUI()
        {
            if (!changed)
            {
                versionNumber = PlayerSettings.bundleVersion;
                lastVersion = versionNumber;
            }

            versionNumber = EditorGUILayout.TextField(versionNumber);

            if (lastVersion != versionNumber)
            {
                changed = true;
            }

            if (changed)
            {
                if (GUILayout.Button("Set Version"))
                {
                    PlayerSettings.bundleVersion = versionNumber;
                    PlayerSettings.macOS.buildNumber = versionNumber;
                    PlayerSettings.iOS.buildNumber = versionNumber;
                    PlayerSettings.Android.bundleVersionCode = int.Parse(versionNumber.Replace(".", string.Empty), NumberStyles.Any);

                    changed = false;
                }
            }
        }
    }
}


