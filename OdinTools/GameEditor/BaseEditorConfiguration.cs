using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniModules.UniGame.Core.Editor.EditorProcessors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModules.Editor.OdinTools.GameEditor
{
    public class BaseEditorConfiguration<TConfiguration> : BaseEditorConfiguration
        where TConfiguration : BaseEditorConfiguration<TConfiguration>
    {
        #region static data
        
        public static TConfiguration Asset => GeneratedTypeItem<TConfiguration>.Asset;
        
        #endregion

    }
    
    public class BaseEditorConfiguration : ScriptableObject, IGameEditorConfiguration
    {
        public const string SettingsCategoryName = "Editor Settings";

        #region inspector
        
        public Object configurationAsset;

        [Space]
        public Sprite icon;
        
        public string menuName = string.Empty;

        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [SerializeReference] 
        [OnValueChanged(nameof(OnCategoriesChanged))]
        [ListDrawerSettings(ListElementLabelName = nameof(IGameEditorCategory.Name))]
        public List<IGameEditorCategory> categories = new();

        [OnValueChanged(nameof(OnValueChanged))]
        [ListDrawerSettings(ListElementLabelName = nameof(EditorSettingsCategory.Name))]
        public List<EditorSettingsCategory> editorGroups = new();

        #endregion

        private Action _updateAction;
        
        public bool   Enabled  => true;
        public Sprite Icon     => icon;
        public string Category => SettingsCategoryName;
        public string Name     => menuName;
        public Color  Color    => Color.yellow;

        public Action UpdateAction
        {
            get => _updateAction;
            set => _updateAction = value;
        }

        [Button]
        public void Refresh() => OnValueChanged();
        
        public List<EditorSettingsCategory> EditorSettingsCategories => editorGroups;
        
        public object CreateDrawer() => this;
        
        public IGameEditorCategory UpdateCategory() => this;

        public void SetupConfiguration(BaseEditorConfiguration configuration) {}
        
        public bool IsMatch(string searchString)
        {
            return GameEditorCategoryFilter.IsMatch(this, searchString);
        }
        
        private void OnValidate()
        {
            configurationAsset = this;
        }

        private void OnCategoriesChanged()
        {
            foreach (var category in categories)
            {
                if(category == null) continue;
                category.UpdateCategory();
            }

            OnValueChanged();
        }

        private void OnValueChanged()
        {
            _updateAction?.Invoke();
        }
    }
}