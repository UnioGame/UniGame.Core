namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System.Collections.Generic;
    using System.Linq;
    using EditorTools.Editor.AssetOperations;
    using UnityEditor;
    using Runtime.Extension;
    using UniCore.EditorTools.Editor.Utility;
    using UnityEngine;

    [GeneratedAssetInfo("ScriptableProcessors")]
    public class ScriptableProcessorsControllerAsset :
        GeneratedAsset<ScriptableProcessorsControllerAsset>
    {
        #region static methods

        [InitializeOnLoadMethod]
        [MenuItem(itemName:"UniGame/ScriptableProcessors/Restart")]
        public static void Initialize()
        {
            var asset = Asset;
            
            var allProcessors = AssetEditorTools.
                GetAssets<ScriptableObject>().
                Where(x => x is IProcess).
                ToList();
            
            asset.processors.Clear();
            asset.processors.AddRange(allProcessors);
            asset.MarkDirty();
            
            if (!asset.activateOnLoad)
                return;

            asset.Stop();
            asset.Start();
        }

        #endregion

        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        public List<ScriptableObject> processors = new List<ScriptableObject>();

        public bool activateOnLoad = true;
        
        #endregion

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Restart()
        {
            Initialize();
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Start()
        {
            processors.OfType<IProcess>().
                ForEach(x => x.Start());
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Stop()
        {
            processors.OfType<IProcess>().
                ForEach(x => x.Stop());
        }
    }
}