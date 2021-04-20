using UniModules.UniGame.Core.Runtime.ScriptableObjects;
using UnityEngine;

#if ODIN_INSPECTOR

namespace UniModules.UniGame.Context.Editor.LifeTimeEditorWindow
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
        
        #region protected methods

        protected override void OnEnable()
        {
            ResetData();
            Bind();
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

        private void Bind()
        {
            editorData.Initialize(LifetimeObjectData.LifeTimes);
        }
        
        #endregion
    }
}


#endif
