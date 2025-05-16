using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace UniModules.GameEditor.Categories
{
    public class AssetViewWindow : OdinEditorWindow
    {
        
        #region static data

        public static AssetViewWindow ShowWindow()
        {
            var window = GetWindow<AssetViewWindow>();
            
            window.minSize      = new Vector2(256f, 256f);
            window.titleContent = new GUIContent("Asset View", EditorIcons.Table.Highlighted);
            window.Show();

            return window;
        }
        
        #endregion

        [InlineEditor()]
        [HideLabel]
        public Object asset;
        
        public AssetViewWindow UpdateView(Object target)
        {
            asset        = target;
            titleContent = new GUIContent("Asset View", EditorIcons.Table.Highlighted);
            return this;
        }
        
    }
}