using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UniModules.Editor.OdinTools.GameEditor.Categories;
using UniModules.UniCore.Runtime.DataFlow;

namespace UniModules.Editor.OdinTools.GameEditor
{
    public class GeneralGameEditorWindow<TConfiguration> : OdinMenuEditorWindow
        where TConfiguration : BaseEditorConfiguration<TConfiguration>
    {

        private TConfiguration _configuration;

        private HashSet<OdinMenuItem> _selectedItems = new HashSet<OdinMenuItem>();
        private List<IGameEditorCategory> _categories = new List<IGameEditorCategory>();
        private LifeTimeDefinition _lifeTimeDefinition = new LifeTimeDefinition();

        protected override void Initialize()
        {
            base.Initialize();
            
            _lifeTimeDefinition?.Release();
            _lifeTimeDefinition = new LifeTimeDefinition();
            
            _configuration = AssetEditorTools.GetAsset<TConfiguration>();
            _configuration = _configuration == null
                ? BaseEditorConfiguration<TConfiguration>.Asset
                : _configuration;

            _configuration.UpdateAction -= Rebuild;
            _configuration.UpdateAction += Rebuild;
        }

        private void Rebuild()
        {
            ForceMenuTreeRebuild();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            _categories = new List<IGameEditorCategory>(_configuration.categories);
            
            var tree = new OdinMenuTree(false)
            {
                Config = {
                    DrawSearchToolbar = true
                }
            };

            tree.DefaultMenuStyle.IconSize             = 24.00f;
            tree.DefaultMenuStyle.IconOffset           = -6.00f;
            tree.DefaultMenuStyle.NotSelectedIconAlpha = 0.90f;
            tree.DefaultMenuStyle.IconPadding          = 2.00f;

            foreach (var category in _configuration.editorGroups)
            {
                if(string.IsNullOrEmpty(category.Name))continue;
                
                tree.Add(category.Name,null,category.Icon);
            }

            foreach (var editorCategory in _categories)
            {
                AddEditorCategory(editorCategory,editorCategory.Category,tree);
            }
            
            tree.Add(_configuration.Category,_configuration,_configuration.Icon);
            
            var firstCategory = _categories.FirstOrDefault();
            if (firstCategory != null)
                TrySelectMenuItemWithObject(firstCategory);
            
            return tree;
        }

        protected void OnSelectionChange()
        {
            _selectedItems.Clear();
            foreach (var selectedItem in MenuTree.Selection)
            {
                _selectedItems.Add(selectedItem);
                if(selectedItem.Value is not IGameEditorView view)
                    continue;
                if(view.Active) continue;
                view.Focus();
            }

            foreach (var menuItem in MenuTree.MenuItems)
            {
                if(menuItem.Value is not IGameEditorView view)
                    continue;
                if(!_selectedItems.Contains(menuItem) && view.Active)
                    view.FocusLost();
            }
        }

        private void AddEditorCategory(IGameEditorCategory editorCategory,string categoryPath,OdinMenuTree tree )
        {
            if (editorCategory is not {Enabled: true}) 
                return;
                
            editorCategory.SetupConfiguration(_configuration);
                
            var category = editorCategory.UpdateCategory();
            var viewer = category.CreateDrawer();
                
            if(viewer == null) return;

            var path = GetFullPath(editorCategory, categoryPath);
            
            tree.Add(path,viewer,editorCategory.Icon);

            if (category is not IGameEditorCategoryList categoryList) 
                return;
            
            foreach (var item in categoryList.Categories)
            {
                AddEditorCategory(item, path,tree);
            }
        }
        
        private string GetFullPath(IGameEditorCategory category) => $"{category.Category}/{category.Name}";
        
        private string GetFullPath(IGameEditorCategory category,string categoryName) => $"{categoryName}/{category.Name}";


        protected override void OnDestroy()
        {
            base.OnDestroy();
            _configuration.UpdateAction -= Rebuild;
            _lifeTimeDefinition.Release();
        }
    }
}