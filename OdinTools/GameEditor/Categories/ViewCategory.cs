using System;

namespace UniModules.Editor.OdinTools.GameEditor.Categories
{
    [Serializable]
    public  abstract class ViewCategory<TView> : GameEditorCategory
    {
        private class InnerView : AssetCategoryLazyView<TView> { }

        public sealed override object CreateDrawer()
        {
            var lazyView = new InnerView();
            lazyView.Initialize(this,CreateView);
            return lazyView;
        }

        public abstract TView CreateView();
    }
    
}