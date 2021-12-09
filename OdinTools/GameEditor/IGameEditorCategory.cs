using Sirenix.OdinInspector;
using UnityEngine;

namespace UniModules.OdinTools.GameEditor
{
    public interface IGameEditorCategory : ISearchFilterable
    {
        Sprite Icon     { get; }
        string        Category { get; }
        string        Name     { get; }
        object        CreateDrawer();
    }
}