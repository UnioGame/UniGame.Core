namespace UniModules.GameEditor
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class EditorSettingsCategory : IGameEditorCategory
    {
        public string name;
        
        [PreviewField]
        public Sprite icon;

        public virtual bool   Enabled        => true;
        public virtual Sprite Icon           => icon;
        public virtual string Category       => string.Empty;
        public virtual string Name           => name;
        public         Color  Color          => Color.yellow;
        public virtual object CreateDrawer() => icon;
        
        public void SetupConfiguration(BaseEditorConfiguration configuration) { }
        
        public IGameEditorCategory UpdateCategory()
        {
            return this;
        }

        //public IGameEditorCategory UpdateCategory() => this;

        public virtual bool IsMatch(string searchString)
        {
            return true;
            //return GameEditorCategoryFilter.IsMatch(this, searchString);
        }
    }
}