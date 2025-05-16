using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UniModules.GameEditor.Categories
{
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetCategoryLazyView<TView>
    {
        #region inspector
        
        [HorizontalGroup()]
        [GUIColor(nameof(GetColor))]
        [PreviewField(height:48,ObjectFieldAlignment.Left)]
        [HideLabel]
        public Sprite icon;
        
        [HorizontalGroup()]
        [HideLabel]
        public string name;
        
        [ShowIf(nameof(IsEnabled))]
        [InlineProperty]
        [HideLabel]
        public TView view;

        [ShowIf(nameof(IsAssetEnabled))]
        [InlineEditor(Expanded = true)]
        [HideLabel]
        public Object assetView;
        
        #endregion
        
        private Func<TView>         _viewFactory;
        private TView               _view;
        private IGameEditorCategory _category;
        
        public bool IsEnabled => view !=null && assetView == null;

        public bool IsAssetEnabled => assetView != null;
        
        public TView View => _view = _view == null ? CreateView() : _view;
        
        public void Initialize(IGameEditorCategory category,Func<TView> viewFactory)
        {
            _category    = category;
            _viewFactory = viewFactory;
            
            name         = category.Name;
            icon         = category.Icon;
        }
        
        [ResponsiveButtonGroup]
        [OnInspectorInit]
        [Button("Show Inspector",ButtonSizes.Large)]
        public void ShowView()
        {
            view       = View;
            assetView  = view as Object;
        }

        [ResponsiveButtonGroup]
        [Button("Show Window",ButtonSizes.Large)]
        public void ShowWindow()
        {
            var targetView = View;
            if (targetView == null)
                return;
            
            ObjectViewWindow.ShowWindow().UpdateView(targetView);
        }

        private Color GetColor() => _category?.Color ?? new Color(0.3f, 0.8f, 0.5f);
        
        protected virtual TView CreateView()
        {
            return _viewFactory == null ? default : _viewFactory();
        }
        
        
    }
}