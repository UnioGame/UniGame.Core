using UnityEditor.Experimental.SceneManagement;

namespace UniGame.Tools
{
    using System;
    using System.Diagnostics;
    using UniCore.Runtime.ProfilerTools;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using Debug = UnityEngine.Debug;
    using UniModules.UniCore.EditorTools.Editor.PrefabTools;
    using UniModules.Editor;

    public static class PrefabTools
    {
        public static TAsset Save<TAsset>(this TAsset asset)
            where TAsset : Component
        {
            if (EditorApplication.isPlaying ||
                EditorApplication.isPlayingOrWillChangePlaymode || !asset)
                return asset;

            asset.SetDirty();
            AssetDatabase.SaveAssets();

            var prefabResource = asset.GetPrefabDefinition();
            prefabResource.SavePrefab();

            return asset;
        }

        public static bool ApplyComponent(this GameObject target, Component component)
        {
            var definition = target.GetPrefabDefinition();
            if (string.IsNullOrEmpty(definition.AssetPath))
                return false;
            PrefabUtility.ApplyAddedComponent(component, definition.AssetPath, InteractionMode.UserAction);
            return true;
        }

        public static EditorPrefabDefinition GetPrefabDefinition(this Component component)
        {
            return GetPrefabDefinition(component.gameObject);
        }


        [Conditional("UNITY_EDITOR")]
        public static void SaveScenes(this object source)
        {
            EditorSceneManager.SaveOpenScenes();
        }

        [Conditional("UNITY_EDITOR")]
        public static void SaveAll(this object source)
        {
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }

        public static EditorPrefabDefinition GetPrefabDefinition(this GameObject target)
        {
            var definition = new EditorPrefabDefinition();
            if (!target) return definition;

            var prefabStage = PrefabStageUtility.GetPrefabStage(target);

            var targetAsset = prefabStage == null ? target : prefabStage.openedFromInstanceObject;

            var instanceStatus = PrefabUtility.GetPrefabInstanceStatus(targetAsset);
            var prefabAssetType = PrefabUtility.GetPrefabAssetType(targetAsset);
            var isVariant = PrefabUtility.IsPartOfVariantPrefab(targetAsset);

            var resultAsset = PrefabUtility.GetOutermostPrefabInstanceRoot(targetAsset);
            var assetPath = string.Empty;

            assetPath = resultAsset != null
                ? PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(resultAsset)
                : AssetDatabase.GetAssetPath(targetAsset);

            definition.Source = PrefabUtility.GetCorrespondingObjectFromOriginalSource(target);
            definition.SourcePath = definition.Source ? AssetDatabase.GetAssetPath(definition.Source) : string.Empty;
            definition.Asset = target;
            definition.AssetPath = assetPath;
            definition.InstanceStatus = instanceStatus;
            definition.PrefabAssetType = prefabAssetType;
            definition.Asset = targetAsset;
            definition.IsRegularPrefab = prefabAssetType != PrefabAssetType.NotAPrefab;
            definition.IsVariantPrefab = isVariant || prefabAssetType == PrefabAssetType.Variant;
            definition.IsInstance = instanceStatus == PrefabInstanceStatus.Connected;

            return definition;
        }

        public static void SavePrefab(this ref EditorPrefabDefinition prefabDefinition)
        {
            GameLog.Log($"SAVE PREFAB: {prefabDefinition}");

            //regular scene instance
            if (!prefabDefinition.AssetPrefab)
            {
                return;
            }

            try
            {
                var asset = prefabDefinition.Asset;
                var path = prefabDefinition.AssetPath;
                switch (prefabDefinition)
                {
                    case {IsVariantPrefab: true}:
                        PrefabUtility.ApplyObjectOverride(asset, path, InteractionMode.UserAction);
                        break;
                    case {IsInstance: true}:
                        PrefabUtility.ApplyPrefabInstance(asset, InteractionMode.UserAction);
                        break;
                    case {IsRegularPrefab: true, InstanceStatus: PrefabInstanceStatus.NotAPrefab}:
                        asset.MarkDirty();
                        break;
                    case {IsRegularPrefab: true}:
                        PrefabUtility.ApplyObjectOverride(asset, path, InteractionMode.UserAction);
                        break;
                    default:
                        prefabDefinition.Asset = PrefabUtility.SaveAsPrefabAssetAndConnect(prefabDefinition.AssetPrefab,
                            prefabDefinition.AssetPath, InteractionMode.UserAction);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"CANT SAVE \n {prefabDefinition}");
                GameLog.LogError(e);
            }
        }

        public static GameObject SaveAndConnectPrefab(this ref EditorPrefabDefinition prefabDefinition)
        {
            GameLog.Log($"SAVE PREFAB: {prefabDefinition}");

            //regular scene instance
            if (!prefabDefinition.AssetPrefab)
            {
                EditorSceneManager.SaveOpenScenes();
                return prefabDefinition.Asset;
            }

            try
            {
                //prefab instance on scene
                if (prefabDefinition.IsInstance)
                {
                    if (prefabDefinition.IsVariantPrefab)
                    {
                        PrefabUtility.ApplyPrefabInstance(prefabDefinition.Asset, InteractionMode.UserAction);
                    }
                    else
                    {
                        prefabDefinition.Asset = PrefabUtility.SaveAsPrefabAssetAndConnect(prefabDefinition.Asset,
                            prefabDefinition.AssetPath, InteractionMode.UserAction);
                    }
                }
            }
            catch (Exception e)
            {
                GameLog.LogError(e);
            }

            return prefabDefinition.Asset;
        }
    }
}