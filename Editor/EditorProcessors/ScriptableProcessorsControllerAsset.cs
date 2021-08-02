using System.Collections;
using Unity.EditorCoroutines.Editor;

namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System.Collections.Generic;
    using System.Linq;
    using UniModules.Editor;
    using UnityEditor;
    using Runtime.Extension;
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
            EditorCoroutineUtility.StartCoroutineOwnerless(InitializeRoutine());
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
            processors.OfType<IEditorProcess>().
                ForEach(x => x.Start());
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Stop()
        {
            processors.OfType<IEditorProcess>().
                ForEach(x => x.Stop());
        }

        private static IEnumerator InitializeRoutine()
        {
            yield return null;
            yield return null;
            yield return null;
            InitializeInline();
        }

        private static void InitializeInline()
        {
            var asset = Asset;
            
            var allProcessors = AssetEditorTools.
                GetAssets<ScriptableObject>().
                Where(x => x is IEditorProcess).
                ToList();
            
            asset.processors.Clear();
            asset.processors.AddRange(allProcessors);
            asset.MarkDirty();
            
            if (!asset.activateOnLoad)
                return;

            asset.Stop();
            asset.Start();
        }
    }
}