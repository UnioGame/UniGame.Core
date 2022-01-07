using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace UniModules.Editor.OdinTools.GameEditor.Categories
{
    public class ObjectViewWindow : OdinEditorWindow
    {
       
        #region static data

        public static ObjectViewWindow ShowWindow()
        {
            var window = GetWindow<ObjectViewWindow>();
            
            window.minSize      = new Vector2(256f, 256f);
            window.titleContent = new GUIContent("Asset View", EditorIcons.Table.Highlighted);
            window.Show();

            return window;
        }
        
        #endregion

        [InlineProperty()]
        [ShowIf(nameof(IsEnabled))]
        [HideLabel]
        public object view;

        [InlineEditor(Expanded = true)]
        [ShowIf(nameof(IsAssetEnabled))]
        [HideLabel]
        public Object asset;
        
                
        public bool IsEnabled => view !=null && asset == null;

        public bool IsAssetEnabled => asset != null;

        
        public ObjectViewWindow UpdateView(object target)
        {
            view = target;
            asset = view as Object;
            return this;
        }
        
    }
}