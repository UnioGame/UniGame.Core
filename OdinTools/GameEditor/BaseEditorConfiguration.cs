using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniModules.UniGame.Core.Editor.EditorProcessors;
using UnityEngine;

namespace UniModules.Editor.OdinTools.GameEditor
{
    public class BaseEditorConfiguration<TConfiguration> : GeneratedAsset<TConfiguration>, 
        IGameEditorConfiguration
        where TConfiguration : BaseEditorConfiguration<TConfiguration>
    {
        public const string SettingsCategoryName = "Editor Settings";

        public Sprite icon;
        public string menuName = string.Empty;

        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [SerializeReference] 
        [OnValueChanged(nameof(OnCategoriesChanged))]
        public List<IGameEditorCategory> categories = new List<IGameEditorCategory>();

        public List<EditorSettingsCategory> editorGroups = new List<EditorSettingsCategory>();

        public bool   Enabled  => true;
        public Sprite Icon     => icon;
        public string Category => SettingsCategoryName;
        public string Name     => menuName;
        public Color  Color    => Color.yellow;

        public object CreateDrawer() => this;
        public IGameEditorCategory UpdateCategory() => this;

        public bool IsMatch(string searchString)
        {
            return GameEditorCategoryFilter.IsMatch(this, searchString);
        }

        private void OnCategoriesChanged()
        {
            foreach (var category in categories)
            {
                if(category == null) continue;
                category.UpdateCategory();
            }
        }
    }
}