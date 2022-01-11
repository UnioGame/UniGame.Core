using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UniModules.Editor.OdinTools.GameEditor
{
    [Serializable]
    public class GameEditorCategory : IGameEditorCategory
    {
        private const string PathFormat = "{0}/{1}";
        
        #region inspector

        [HorizontalGroup(nameof(category))]
        [LabelWidth(8)]
        [VerticalGroup(nameof(category)+"/"+nameof(icon))]
        [PreviewField(ObjectFieldAlignment.Left)]
        [HideLabel]
        public Sprite icon;

        [VerticalGroup(nameof(category)+"/"+nameof(name))]
        [LabelWidth(60)]
        public string name = nameof(GameEditorCategory);
        
        [VerticalGroup(nameof(category)+"/"+nameof(name))]
        [ValueDropdown(nameof(GetCategories))]
        [LabelWidth(60)]
        public string category = "BASE";

        public bool enabled = true;
        
        public Color color;
        
        #endregion
        
        public bool Enabled => enabled;

        public Color Color => color;
        
        public Sprite        Icon     => icon;
        public virtual string Category => category;

        public virtual string Name     => name;
        
        public virtual object CreateDrawer() => null;
        
        public virtual IGameEditorCategory UpdateCategory()
        {
            if (!string.IsNullOrEmpty(name) && name != Name) name = Name;
            return this;
        }

        public IEnumerable<string> GetCategories()
        {
            var asset = GameEditorConfiguration.Asset;
            return asset.editorGroups.Select(x => x.Name);
        }

        public virtual bool IsMatch(string searchString)
        {
            return GameEditorCategoryFilter.IsMatch(this, searchString);
        }
    }

    public static class GameEditorCategoryFilter
    {
        public static bool IsMatch(IGameEditorCategory category,string searchString)
        {
            if (category == null) return false;
            
            if (string.IsNullOrEmpty(searchString))
                return true;
            
            var isMatch = category.Name.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                          category.GetType().Name.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                          category.Category.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0;
            if (isMatch) return true;

            if (category.Icon!=null && 
                category.Icon.name.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0) 
                return true;

            return false;
        }
    }
}