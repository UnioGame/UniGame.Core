            
#if ODIN_INSPECTOR

using Sirenix.Utilities.Editor;

#endif

namespace UniModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DrawersTools;
    using Runtime.Attributes;
    using Runtime.Extension;
    using UniCore.Runtime.Utils;
    using UniModules.Editor;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CustomPropertyDrawer(typeof(AssetFilterAttribute))]
    public class AssetFilterDrawer : PropertyDrawer
    {
        private const string emptyValue = "none";

        private bool hasOdinSupport = false;
        private static List<string> assetsItems = new List<string>();
        private static Func<AssetFilterAttribute, List<Object>> typeAssets = MemorizeTool.Create<AssetFilterAttribute,List<Object>>(FindFiltered);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
#if ODIN_INSPECTOR
            hasOdinSupport = true;
#endif
            
            var targetAttribute = attribute as AssetFilterAttribute;

            var filterType = targetAttribute.FilterType ?? fieldInfo.FieldType;
            var folderFilter = targetAttribute.FolderFilter;
            
            assetsItems.Clear();
            assetsItems.Add(emptyValue);
            
            var searchTarget = new AssetFilterAttribute() {
                FilterType = filterType,
                FolderFilter = folderFilter,
                FoldOutOpen = targetAttribute.FoldOutOpen,
                DrawWithOdin = targetAttribute.DrawWithOdin
            };

            var assets = typeAssets(searchTarget);
            var target = property.objectReferenceValue;
            var currentValue = assets.FirstOrDefault(x => target == x);
            var index = assets.IndexOf(currentValue) + 1;
            var assetValue = property.objectReferenceValue;
            
            assetsItems.AddRange(assets.Select(x => x.name));

            DrawTypeDropDown(property, assets, index);

            if (!hasOdinSupport)
            {
                EditorGUILayout.PropertyField(property, label,true);
            }
            
#if ODIN_INSPECTOR
            if (searchTarget.DrawWithOdin && assetValue)
            {
                SirenixEditorFields.UnityObjectField(label,assetValue, assetValue.GetType(),false);
            }
#endif

        }

        private bool DrawTypeDropDown(SerializedProperty property,List<Object> assets,int index)
        {
            
            //type dropdown
            var newIndex = EditorGUILayout.Popup(string.Empty, index, assetsItems.ToArray());

            if (newIndex == index) {
                return false;
            }

            var targetIndex = newIndex - 1;
            property.objectReferenceValue = targetIndex < 0 ? null : assets[targetIndex];
            property.serializedObject.ApplyModifiedProperties();

            return true;
        }

        private static List<Object> FindFiltered(AssetFilterAttribute targetAttribute)
        {
            var isObject = targetAttribute.FilterType.IsAsset();
            var filterType = targetAttribute.FilterType;
            var folderFilter = string.IsNullOrEmpty(targetAttribute.FolderFilter) ? null : new[] {targetAttribute.FolderFilter};
            if (isObject) {
                return AssetEditorTools.GetAssets(filterType, folderFilter);
            }
            return AssetEditorTools.
                GetAssets(UnityTypeExtension.scriptableType,folderFilter).
                Where(x => filterType.IsInstanceOfType(x)).
                ToList();
        }
        
    }
}
