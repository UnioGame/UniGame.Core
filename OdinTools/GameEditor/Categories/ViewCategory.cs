using System;

namespace UniModules.Editor.OdinTools.GameEditor.Categories
{
    [Serializable]
    public class ViewCategory<TView> : GameEditorCategory
    {
        private class InnerView : AssetCategoryLazyView<TView> { }

        public sealed override object CreateDrawer()
        {
            var lazyView = new InnerView();
            lazyView.Initialize(this,CreateView);
            return lazyView;
        }

        public virtual TView CreateView()
        {
            return default;
        }
    }
    
}