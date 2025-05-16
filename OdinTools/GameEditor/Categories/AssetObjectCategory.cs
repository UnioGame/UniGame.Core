using System;
using Sirenix.OdinInspector;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UniModules.GameEditor.Categories
{
    using Editor;

    [Serializable]
    public class AssetObjectCategory : ViewCategory<Object>
    {
        [OnInspectorGUI(nameof(OnGuidDrawer), true)]
        public string guid;
        
        [NonSerialized]
        private Object _asset;
        
        public override Object CreateView()
        {
            if (string.IsNullOrEmpty(guid))
                return default;
            var targetAsset = AssetDatabase
                .LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid));
            return targetAsset;
        }

        private void OnGuidDrawer()
        {
            if (_asset == null && !string.IsNullOrEmpty(guid))
                _asset = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid));
            
            //guid = EditorGUILayout.TextField(nameof(guid),guid);
            var newAsset = EditorGUILayout.ObjectField("Asset",_asset, typeof(Object),false);
            if (newAsset != _asset)
            {
                _asset = newAsset;
                guid = _asset == null ? string.Empty : newAsset.GetGUID();
            }
        }
        
    }

}
