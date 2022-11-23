using UniGame.Core.Runtime.SerializableType;
using UnityEditor;

namespace UniModules.UniGame.CoreModules.UniGame.Core.Editor.Controls.SerializableTypeDrawer
{
    [CustomEditor(typeof(SType))]
    public class STypeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
