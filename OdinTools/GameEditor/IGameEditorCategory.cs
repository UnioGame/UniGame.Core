using Sirenix.OdinInspector;
using UnityEngine;

namespace UniModules.Editor.OdinTools.GameEditor
{
    using System.Collections.Generic;

    public interface IGameEditorCategory : ISearchFilterable
    {
        public bool   Enabled  { get; }
        public Sprite Icon     { get; }
        string        Category { get; }
        string        Name     { get; }
        Color         Color    { get; }
        object        CreateDrawer();

        void SetupConfiguration(BaseEditorConfiguration configuration);
        
        IGameEditorCategory UpdateCategory();
    }
    
    public interface IGameEditorCategoryList : IGameEditorCategory
    {
        List<IGameEditorCategory> Categories { get; }
    }
}