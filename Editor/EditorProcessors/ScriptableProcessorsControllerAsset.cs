namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System;
    using UniModules.UniCore.Runtime.ReflectionUtils;
    using System.Collections;
    using Unity.EditorCoroutines.Editor;
    using System.Collections.Generic;
    using System.Linq;
    using UniModules.Editor;
    using UnityEditor;
    using global::UniGame.Core.Runtime.Extension;
    using UnityEngine;

    [GeneratedAssetInfo("ScriptableProcessors")]
    public class ScriptableProcessorsControllerAsset : GeneratedAsset<ScriptableProcessorsControllerAsset>
    {
        #region static methods

        public static Type ProcessorApiType = typeof(IEditorProcess);
        
        [InitializeOnLoadMethod]
        [MenuItem(itemName: "UniGame/ScriptableProcessors/Restart")]
        public static void ProcessorsInitialize() => InitializeRoutine();
        
        #endregion

        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor]
#endif
        public List<ScriptableObject> processors = new List<ScriptableObject>();

        public bool activateOnLoad = true;
        
        #endregion

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Restart() => InitializeRoutine();
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Start() =>  processors
            .OfType<IEditorProcess>()
            .ForEach(static x => x.Start());

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Stop() => processors
            .OfType<IEditorProcess>()
            .ForEach(static x => x.Stop());

        private static void InitializeRoutine()
        {
            Load(InitializeInline);
        }

        private static void InitializeInline(ScriptableProcessorsControllerAsset source)
        {
            var asset = source;
            var types = ProcessorApiType.GetAssignableTypes();
            
            var allProcessors = types
                .Select(x => AssetEditorTools.GetAssets(x))
                .SelectMany(x => x)
                .Where(x => x is ScriptableObject and IEditorProcess)
                .Select(x => x as ScriptableObject)
                .ToList();
            
            asset.processors.Clear();
            asset.processors.AddRange(allProcessors);
            asset.MarkDirty();
            
            if (!asset.activateOnLoad) return;

            asset.Stop();
            asset.Start();
        }
    }
}