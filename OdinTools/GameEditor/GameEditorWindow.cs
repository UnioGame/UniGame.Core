using UnityEditor;
using UnityEngine;

namespace UniModules.GameEditor
{
    public class GameEditorWindow : GeneralGameEditorWindow<GameEditorConfiguration>
    {
        #region static data
        
        [MenuItem("UniGame/Editors/GameEditor")]
        public static void OpenWindow()
        {
            var window = GetWindow<GameEditorWindow>();
            window.minSize      = new Vector2(512f, 512f);
            window.titleContent = new GUIContent(nameof(GameEditorWindow));
            window.Show();
        }

        #endregion

    }
}
