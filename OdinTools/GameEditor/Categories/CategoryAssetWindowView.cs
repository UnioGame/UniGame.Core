using Sirenix.OdinInspector;
using UniModules.Editor;
using UnityEngine;

namespace UniModules.GameEditor.Categories
{
    [HideLabel]
    [InlineProperty]
    public class CategoryAssetWindowView<TAsset>
        where TAsset : Object
    {

        [HideIf(nameof(IsEmpty))]
        [InlineEditor(Expanded = true)]
        [HideLabel]
        public TAsset asset;

        public bool IsEmpty => asset == null;
        
        [Button("Load Inspector",ButtonSizes.Large)]
        public void LoadInspector()
        {
            asset = Load();
        }
        
        [Button("Show Editor",ButtonSizes.Large)]
        public void ShowEditorWindow()
        {
            var target = Load();
            if (target == null) return;
            AssetViewWindow.ShowWindow().UpdateView(target);
        }
        
        protected virtual TAsset Load() => AssetEditorTools.GetAsset<TAsset>();
        
    }
}