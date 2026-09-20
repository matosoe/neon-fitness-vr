using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEditor.XR.OpenXR;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features;
using UnityEngine.XR.OpenXR.Features.MetaQuestSupport;

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

        public static void ReportMetaProjectSetup()
        {
            const BindingFlags staticNonPublic = BindingFlags.Static | BindingFlags.NonPublic;
            var getTasks = typeof(OVRProjectSetup).GetMethod("GetTasks", staticNonPublic);

            if (getTasks == null)
            {
                throw new InvalidOperationException("Meta Project Setup Tool não expôs as tarefas de Android.");
            }

            var tasks = (IEnumerable)getTasks.Invoke(null, new object[] { BuildTargetGroup.Android });
            Debug.Log("[NeonFitnessVR][MetaSetup] BEGIN");

            foreach (var task in tasks)
            {
                var taskType = task.GetType();
                var done = ((Delegate)taskType.GetProperty("IsDone").GetValue(task))
                    .DynamicInvoke(BuildTargetGroup.Android);
                var message = GetTaskValue(task, "Message");
                var level = GetTaskValue(task, "Level");
                var automatic = taskType.GetProperty("FixAutomatic").GetValue(task);
                var tags = taskType.GetProperty("Tags").GetValue(task);

                Debug.Log($"[NeonFitnessVR][MetaSetup] done={done}; level={level}; automatic={automatic}; tags={tags}; message={message}");
            }

            Debug.Log("[NeonFitnessVR][MetaSetup] END");
        }

        public static void ApplyMetaProjectSetupCore()
        {
            var messagesToFix = new[]
            {
                "Minimum Android API Level must be at least 32",
                "Target API must be set to 34 to upload to the Meta Quest Store.",
                "Always specify single \"GameActivity\" application entry on Unity 2023.2+",
                "Subsampled Layout should be enabled to improve GPU performance when foveation is enabled.",
            };

            const BindingFlags staticNonPublic = BindingFlags.Static | BindingFlags.NonPublic;
            var getTasks = typeof(OVRProjectSetup).GetMethod("GetTasks", staticNonPublic);
            var tasks = (IEnumerable)getTasks.Invoke(null, new object[] { BuildTargetGroup.Android });

            foreach (var task in tasks)
            {
                var message = GetTaskValue(task, "Message") as string;
                if (Array.IndexOf(messagesToFix, message) < 0)
                {
                    continue;
                }

                var taskType = task.GetType();
                var isDone = (bool)((Delegate)taskType.GetProperty("IsDone").GetValue(task))
                    .DynamicInvoke(BuildTargetGroup.Android);
                if (isDone)
                {
                    continue;
                }

                var fixedTask = (bool)taskType.GetMethod("Fix", BindingFlags.Instance | BindingFlags.Public)
                    .Invoke(task, new object[] { BuildTargetGroup.Android });
                Debug.Log($"[NeonFitnessVR][MetaSetup] applied={fixedTask}; message={message}");
            }

            AssetDatabase.SaveAssets();
        }

        [MenuItem("Neon Fitness VR/Configure Meta OpenXR Features")]
        public static void ConfigureMetaOpenXrFeatures()
        {
            var settings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Android);
            if (settings == null)
            {
                throw new InvalidOperationException("As configurações OpenXR para Android não foram encontradas.");
            }

            var metaQuest = settings.GetFeature<MetaQuestFeature>();
            var foveation = settings.GetFeature<FoveatedRenderingFeature>();
            if (metaQuest == null || foveation == null)
            {
                throw new InvalidOperationException("Os recursos Meta Quest e Foveated Rendering do OpenXR não estão disponíveis.");
            }

            metaQuest.enabled = true;
            foveation.enabled = true;

            var serializedFoveation = new SerializedObject(foveation);
            var subsampledLayout = serializedFoveation.FindProperty("enableSubsampledLayout");
            if (subsampledLayout == null)
            {
                throw new InvalidOperationException("A opção Subsampled Layout não foi encontrada no recurso de foveação.");
            }

            subsampledLayout.boolValue = true;
            serializedFoveation.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(settings);
            EditorUtility.SetDirty(metaQuest);
            EditorUtility.SetDirty(foveation);
            AssetDatabase.SaveAssets();
            Debug.Log("[NeonFitnessVR][OpenXR] Meta Quest, Foveated Rendering e Subsampled Layout foram habilitados para Android.");
        }

        public static void ReportOpenXrValidation()
        {
            var issues = new List<OpenXRFeature.ValidationRule>();
            OpenXRProjectValidation.GetCurrentValidationIssues(issues, BuildTargetGroup.Android);
            Debug.Log($"[NeonFitnessVR][OpenXRValidation] issues={issues.Count}; BEGIN");

            foreach (var issue in issues)
            {
                Debug.Log($"[NeonFitnessVR][OpenXRValidation] error={issue.error}; message={issue.message}");
            }

            Debug.Log("[NeonFitnessVR][OpenXRValidation] END");
        }

        private static object GetTaskValue(object task, string propertyName)
        {
            var wrapper = task.GetType().GetProperty(propertyName).GetValue(task);
            var getValue = wrapper.GetType().GetMethod("GetValue");
            return getValue.Invoke(wrapper, new object[] { BuildTargetGroup.Android });
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
