using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniModules.UniGame.Core.Editor.EditorProcessors;
using UnityEngine;

namespace UniModules.Editor.OdinTools.GameEditor
{
    public class BaseEditorConfiguration<TConfiguration> : BaseEditorConfiguration
        where TConfiguration : BaseEditorConfiguration<TConfiguration>
    {
        #region static data
        
        public static TConfiguration Asset => GeneratedTypeItem<TConfiguration>.Asset;
        
        #endregion
        
    }
    
    public class BaseEditorConfiguration : ScriptableObject, 
        IGameEditorConfiguration
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
        

        public List<EditorSettingsCategory> EditorSettingsCategories => editorGroups;
        
        public object CreateDrawer() => this;
        public IGameEditorCategory UpdateCategory() => this;

        public void SetupConfiguration(BaseEditorConfiguration configuration) {}
        
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