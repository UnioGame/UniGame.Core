using System;

namespace UniModules.GameEditor.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using UnityEngine;

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
    
    [Serializable]
    public abstract class DefaultViewCategory<TView> : ViewCategory<TView>
        where TView : new()
    {
        private class InnerView : AssetCategoryLazyView<TView> { }
        
        public override TView CreateView() => new TView();
    }
    
    [Serializable]
    public abstract class CategoryList : GameEditorCategory, IGameEditorCategoryList
    {
        private List<IGameEditorCategory> _categories = new List<IGameEditorCategory>();

        public List<IGameEditorCategory> Categories => _categories;
        
        public sealed override object CreateDrawer()
        {
            _categories = CreateCategories();
            return this;
        }

        public virtual List<IGameEditorCategory> CreateCategories()
        {
            return _categories;
        }
    }
    
    [Serializable]
    public class CategoryItemView<TView> : GameEditorCategory
    {
        [SerializeReference]
        public TView view;

        public sealed override object CreateDrawer()
        {
            return view;
        }
    }

}