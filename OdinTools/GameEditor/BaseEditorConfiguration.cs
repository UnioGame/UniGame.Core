using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniModules.UniGame.Core.Editor.EditorProcessors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModules.GameEditor
{
    using System.Linq;
    using Editor;
    using global::UniGame.Runtime.Utils;
    using UnityEditor;

    public class BaseEditorConfiguration : ScriptableObject,IGameEditorCategory
    {
        public const string SettingsCategoryName = "Editor Settings";
        public const string SettingsKey = "Editor Settings";
        public const string GroupKey = "Categories";

        #region inspector

        
        [TabGroup(GroupKey)]  
        [Space]
        public Sprite icon;
        
        [TabGroup(GroupKey)]  
        public string menuName = string.Empty;

        [TabGroup(GroupKey)]  
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [SerializeReference] 
        [OnValueChanged(nameof(OnCategoriesChanged))]
        [ListDrawerSettings(ListElementLabelName = nameof(IGameEditorCategory.Name))]
        public List<IGameEditorCategory> categories = new();

        [TabGroup(SettingsKey)]   
        public Object configurationAsset;
        [TabGroup(SettingsKey)]
        public float iconSize             = 24.00f;
        [TabGroup(SettingsKey)]
        public float iconOffset           = -6.00f;
        [TabGroup(SettingsKey)]
        public float notSelectedIconAlpha = 0.90f;
        [TabGroup(SettingsKey)]
        public float iconPadding          = 2.00f;
        
        #endregion
        
        private Action _updateAction;

        #region public properties
        
        public virtual Type ConfigurationType => GetType();
        public bool   Enabled  => true;
        public virtual Sprite Icon     => icon;
        public virtual string Category => SettingsCategoryName;
        public virtual string Name     => menuName;
        public virtual Color  Color    => Color.yellow;
        
        #endregion
        
        public Action UpdateAction
        {
            get => _updateAction;
            set => _updateAction = value;
        }

        [TabGroup(GroupKey)]  
        [ResponsiveButtonGroup("_DefaultTabGroup/Categories/Commands",DefaultButtonSize = ButtonSizes.Medium,Order = -1)]
        [Button]
        public virtual void Refresh() => OnValueChanged();
        
        [TabGroup(GroupKey)]  
        [ResponsiveButtonGroup("_DefaultTabGroup/Categories/Commands",DefaultButtonSize = ButtonSizes.Medium,Order = -1)]
        [Button]
        public virtual void FillConfigurations()
        {
            var categoriesTypes = TypeCache.GetTypesDerivedFrom(typeof(IAutoEditorCategory));
            foreach (var categoryType in categoriesTypes)
            {
                if(categoryType.IsAbstract || categoryType.IsInterface || categoryType.IsGenericTypeDefinition) continue;
                if(categoryType.IsSubclassOf(typeof(Object))) continue;
                if(categories.Any(x => x?.GetType() == categoryType)) continue;
                if(categoryType.HasDefaultConstructor() == false) continue;
                
                var category = (IAutoEditorCategory)categoryType.CreateWithDefaultConstructor();
                if(category == null) continue;
                
                category.UpdateCategory();
                
                categories.Add(category);

                this.MarkDirty();
            }
            
            Refresh();
        }

        public virtual object CreateDrawer() => this;
        
        public virtual IGameEditorCategory UpdateCategory() => this;

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