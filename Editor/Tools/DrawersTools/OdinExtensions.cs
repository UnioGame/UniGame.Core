using System;
using System.Diagnostics;
using UniGame.Attributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModules.UniGame.Editor.DrawersTools
{
    using UniModules.Editor;
    using global::UniGame.Runtime.Utils;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
#endif
    
    /// <summary>
    /// Odin Inspector extensions methods
    /// </summary>
    public static class OdinExtensions
    {
#if ODIN_INSPECTOR
        public static MemorizeItem<object, PropertyTree> PropertyTreeFactory = MemorizeTool
            .Memorize<object, PropertyTree>(PropertyTree.Create, x => x.Dispose());
#endif
        
        public static Type UnityObjectType = typeof(Object);

#if ODIN_INSPECTOR
        public static PropertyTree GetPropertyTree(this object target)
        {
            return PropertyTreeFactory[target];
        }
#endif


        public static void DrawAssetChildWithOdin(this object asset, Type type, int childIndex)
        {
#if ODIN_INSPECTOR

            var propertyTree = asset.GetPropertyTree();
            
            for (var i = 0; i < propertyTree.RootPropertyCount; i++) {
                var p            = propertyTree.GetRootProperty(i);
                var info         = p.Info;
                if(info.GetAttribute<IgnoreDrawerAttribute>()!=null)
                    continue;

                var valueType    = info.TypeOfValue;

                if(valueType != type)
                    continue;
                var children   = p.Children;
                var childCount = children.Count;
                if (childCount == 0 || childIndex >= children.Count)
                    return;
                
                var child = children[childIndex];
                var items = child.Children;
                
                foreach (var property in items)
                {
                    if(property.GetAttribute<IgnoreDrawerAttribute>()!=null)
                        continue;
                    property.Update(true);
                    property.Draw();
                }

                break;
            }
            
            propertyTree.ApplyChanges();
            propertyTree.UpdateTree();
#endif
        }
        
        
        [Conditional("ODIN_INSPECTOR")]
        public static void DrawOdinPropertyInspector(this object asset)
        {
#if ODIN_INSPECTOR
            if (asset == null || EditorApplication.isCompiling || EditorApplication.isUpdating) {
                return;
            }
            
            var drawer = PropertyTreeFactory[asset];
            drawer.Draw(false);
#endif
        }

        public static Object DrawOdinPropertyField(
            this Object asset,
            Type type,
            Action<Object> onValueChanged = null,
            bool allowSceneObjects = true,
            string label = "")
        {
            var newValue = EditorGUILayout.ObjectField(label, asset, type, allowSceneObjects);
            if (newValue != asset) {
                onValueChanged?.Invoke(newValue);
            }

            try {
                newValue?.DrawOdinPropertyInspector();
            }
            catch (Exception r) {
            }

            return newValue;
        }

        public static bool DrawOdinPropertyWithFoldout(
            this Object asset,
            bool foldOut,
            string label = "",
            Action<Object> onValueChanged = null)
        {
            var targetType = asset == null ? null : asset.GetType();
            foldOut = EditorDrawerUtils.DrawObjectFoldout(asset, foldOut, targetType, label, x => {
                asset = x;
                onValueChanged?.Invoke(x);
            });

            try {
                if (foldOut) {
                    asset.DrawOdinPropertyInspector();
                }
            }
            catch (Exception r) {
            }

            return foldOut;
        }

        public static bool DrawOdinPropertyWithFoldout(
            this Object asset,
            Rect position,
            bool foldOut,
            string label = "")
        {
            var targetType = asset == null ? null : asset.GetType();
            if (targetType == null || asset == null) {
                EditorDrawerUtils.DrawDisabled(() => { EditorGUI.ObjectField(position, label, asset, UnityObjectType, true); });
                return false;
            }

            foldOut = EditorGUI.Foldout(position, foldOut, string.Empty);
            var rect = GUILayoutUtility.GetLastRect();
            rect.x += 14;

            EditorDrawerUtils.DrawDisabled(() => { EditorGUI.ObjectField(rect, label, asset, targetType, true); });

            try {
                if (foldOut) {
                    asset.DrawOdinPropertyInspector();
                }
            }
            catch (Exception r) {
            }

            return foldOut;
        }

        public static bool OdinFieldFoldout(
            this SerializedProperty property,
            bool foldout,
            GUIContent label,
            bool includeChildren)
        {
            foldout = EditorDrawerUtils.
                DrawFieldFoldout(
                    property,foldout, 
                    label, includeChildren);
            
            if (foldout) {
                switch (property.propertyType) {
                    case SerializedPropertyType.ObjectReference:
                        var value = property.objectReferenceValue;
                        value.DrawOdinPropertyInspector();
                        break;
                }
            }

            return foldout;
        }


        public static bool DrawOdinPropertyWithFoldout(
            this SerializedProperty property,
            Rect position,
            bool foldOut)
        {
            return DrawOdinPropertyWithFoldout(property.objectReferenceValue, position, foldOut, property.name);
        }
    }
}