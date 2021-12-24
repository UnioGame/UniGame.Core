using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniModules.UniGame.Core.Editor.EditorProcessors;
using UnityEngine;

namespace UniModules.OdinTools.GameEditor
{
    public class BaseEditorConfiguration<TConfiguration> : GeneratedAsset<TConfiguration>, 
        IGameEditorConfiguration
        where TConfiguration : BaseEditorConfiguration<TConfiguration>
    {
        public const string SettingsCategoryName = "Editor Settings";
        
        public       Sprite icon;
        public       string menuName = string.Empty;
        
        [SerializeReference]
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        public List<IGameEditorCategory> categories = new List<IGameEditorCategory>();

        public List<string> editorCategories = new List<string>()
        {
            SettingsCategoryName
        };
        
        public Sprite Icon     => icon;
        public virtual string Category => SettingsCategoryName;
        public virtual string Name     => menuName;
        
        public object CreateDrawer() => this;

        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return true;
            return Category.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}