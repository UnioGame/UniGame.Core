using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;

namespace UniModules.OdinTools.GameEditor
{
    public class GeneralGameEditorWindow<TConfiguration>: OdinMenuEditorWindow
        where TConfiguration : BaseEditorConfiguration<TConfiguration>
    {

        private TConfiguration _configuration;

        private List<IGameEditorCategory> _categories = new List<IGameEditorCategory>();
        
        protected override void Initialize()
        {
            base.Initialize();
            _configuration = BaseEditorConfiguration<TConfiguration>.Asset;
            _categories    = new List<IGameEditorCategory>(_configuration.categories);
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(false)
            {
                Config = { DrawSearchToolbar = true }
            };

            foreach (var editorCategory in _categories)
            {
                var viewer = editorCategory.CreateDrawer();
                if(viewer == null) continue;
                tree.Add(GetFullPath(editorCategory),viewer,editorCategory.Icon);
            }
            
            tree.Add(GetFullPath(_configuration),_configuration,_configuration.Icon);
            
            var firstCategory = _categories.FirstOrDefault();
            if (firstCategory != null)
                TrySelectMenuItemWithObject(firstCategory);
            
            return tree;
        }

        private string GetFullPath(IGameEditorCategory category) => $"{category.Category}/{category.Name}";
    }
}