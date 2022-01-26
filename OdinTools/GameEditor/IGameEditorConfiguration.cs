using System.Collections.Generic;

namespace UniModules.Editor.OdinTools.GameEditor
{
    public interface IGameEditorConfiguration : IGameEditorCategory
    {
        List<EditorSettingsCategory> EditorSettingsCategories { get; }
    }
}