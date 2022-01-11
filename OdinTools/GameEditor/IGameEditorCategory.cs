using Sirenix.OdinInspector;
using UnityEngine;

namespace UniModules.Editor.OdinTools.GameEditor
{
    public interface IGameEditorCategory : ISearchFilterable
    {
        public bool   Enabled  { get; }
        public Sprite Icon     { get; }
        string        Category { get; }
        string        Name     { get; }
        Color         Color    { get; }
        object        CreateDrawer();
        IGameEditorCategory UpdateCategory();
    }
}