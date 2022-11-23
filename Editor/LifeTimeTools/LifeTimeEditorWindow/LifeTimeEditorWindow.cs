using UnityEngine;

#if ODIN_INSPECTOR

namespace UniGame.Context.Editor
{
    
    using Sirenix.OdinInspector;
    using UnityEditor;
    using Sirenix.OdinInspector.Editor;

    public class LifeTimeEditorWindow : OdinEditorWindow
    {
        #region static data
        
        [MenuItem("UniGame/LifeTime/LifeTime Window")]
        private static void OpenWindow()
        {
            var window = GetWindow<LifeTimeEditorWindow>();
            window.titleContent = new GUIContent( "LifeTime Status Window");
            window.Show();
        }
        
        #endregion

        #region inspector

        [InlineProperty]
        [HideLabel]
        public LifeTimeEditorData editorData = new LifeTimeEditorData();
        
        #endregion

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Refresh()
        {
            //editorData.Initialize(LifetimeObjectData.LifeTimes);
        }
        
        #region protected methods

        protected override void OnEnable()
        {
            ResetData();
            Refresh();
            base.OnEnable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            editorData?.Dispose();
        }

        private void ResetData()
        {
            editorData = new LifeTimeEditorData();
        }
        
        #endregion
    }
}


#endif
