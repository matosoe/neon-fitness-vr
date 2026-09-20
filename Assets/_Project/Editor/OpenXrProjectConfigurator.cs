using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;

namespace NeonFitnessVR.Editor
{
    /// <summary>
    /// Keeps the project-level OpenXR loader assignment deterministic for supported targets.
    /// This class is editor-only and is safe to run repeatedly.
    /// </summary>
    [InitializeOnLoad]
    public static class OpenXrProjectConfigurator
    {
        private const string SettingsPath = "Assets/_Project/Settings/XRGeneralSettingsPerBuildTarget.asset";

        static OpenXrProjectConfigurator()
        {
            EditorApplication.delayCall += Configure;
        }

        [MenuItem("Neon Fitness VR/Configure OpenXR")]
        public static void Configure()
        {
            var settings = FindOrCreateSettings();
            ConfigureLoader(settings, BuildTargetGroup.Standalone);
            ConfigureLoader(settings, BuildTargetGroup.Android);

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        private static XRGeneralSettingsPerBuildTarget FindOrCreateSettings()
        {
            if (EditorBuildSettings.TryGetConfigObject(
                    XRGeneralSettings.k_SettingsKey,
                    out XRGeneralSettingsPerBuildTarget settings))
            {
                return settings;
            }

            settings = ScriptableObject.CreateInstance<XRGeneralSettingsPerBuildTarget>();
            AssetDatabase.CreateAsset(settings, SettingsPath);
            EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, settings, true);
            return settings;
        }

        private static void ConfigureLoader(
            XRGeneralSettingsPerBuildTarget settings,
            BuildTargetGroup targetGroup)
        {
            if (!settings.HasManagerSettingsForBuildTarget(targetGroup))
            {
                settings.CreateDefaultManagerSettingsForBuildTarget(targetGroup);
            }

            var manager = settings.ManagerSettingsForBuildTarget(targetGroup);
            XRPackageMetadataStore.AssignLoader(manager, typeof(OpenXRLoader).FullName, targetGroup);
        }
    }
}
