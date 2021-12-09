using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UniModules.OdinTools.GameEditor
{
    [Serializable]
    public class GameEditorCategory<TConfiguration> : IGameEditorCategory
        where TConfiguration : BaseEditorConfiguration<TConfiguration>
    {
        private const string PathFormat = "{0}/{1}";
        
        #region inspector

        public string name = "DEFAULT";

        [ValueDropdown(nameof(GetCategories))]
        public string category = "BASE";

        public Sprite icon;
        
        #endregion
        
        public Sprite        Icon     => icon;
        public virtual string Category => category;

        public virtual string Name     => name;
        
        public virtual object CreateDrawer() => null;

        public IEnumerable<string> GetCategories()
        {
            return BaseEditorConfiguration<TConfiguration>.Asset.editorCategories;
        }

        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;

            var result = Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         Category.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         (icon != null && icon.name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);

            return result;
        }
    }
}