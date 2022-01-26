using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;

namespace UniModules.Editor.OdinTools.GameEditor
{
    public class GeneralGameEditorWindow<TConfiguration> : OdinMenuEditorWindow
        where TConfiguration : BaseEditorConfiguration<TConfiguration>
    {

        private TConfiguration _configuration;
        
        private List<IGameEditorCategory> _categories = new List<IGameEditorCategory>();

        protected override void Initialize()
        {
            base.Initialize();
            _configuration = BaseEditorConfiguration<TConfiguration>.Asset;
            _categories = new List<IGameEditorCategory>(_configuration.categories);
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
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
                tree.Add(category.Name,null,category.Icon);
            }

            foreach (var editorCategory in _categories.Where(x => x.Enabled))
            {
                editorCategory.SetupConfiguration(_configuration);
                var category = editorCategory.UpdateCategory();
                var viewer = category.CreateDrawer();
                if(viewer == null) continue;
                tree.Add(GetFullPath(editorCategory),viewer,editorCategory.Icon);
            }
            
            tree.Add(_configuration.Category,_configuration,_configuration.Icon);
            
            var firstCategory = _categories.FirstOrDefault();
            if (firstCategory != null)
                TrySelectMenuItemWithObject(firstCategory);
            
            return tree;
        }

        private string GetFullPath(IGameEditorCategory category) => $"{category.Category}/{category.Name}";
    }
}