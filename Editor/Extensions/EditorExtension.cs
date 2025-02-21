﻿namespace UniModules.Editor {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UniModules.Editor;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class EditorExtension
    {
        
        public static bool OpenEditorScript(this Type type,params string[] folders) => AssetEditorTools.OpenScript(type,folders);
        
        public static bool OpenEditorScript<T>(this Type type,params string[] folders) => AssetEditorTools.OpenScript<T>(folders);

        public static string AssetGuidToPath(this string guid) => AssetDatabase.GUIDToAssetPath(guid);
        
        public static string AssetPathToGuid(this string path) => AssetDatabase.AssetPathToGUID(path);
        
        
        /// <summary>
        /// Gets all childrens of `SerializedObjects`
        /// at 1 level depth if includeChilds == false.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetChildren(this SerializedObject serializedProperty)
        {
            var property = serializedProperty.GetIterator();
            return property.GetChildrens();
        }

        public static bool IsEquals(this SerializedProperty source, SerializedProperty target)
        {
            return SerializedProperty.EqualContents(source, target);
        }

        public static int GetArrayPropertyIndex(this SerializedProperty source, SerializedProperty target)
        {
            if (source.isArray == false)
                return -1;
            for (int i = 0; i < source.arraySize; i++) {
                var element = source.GetArrayElementAtIndex(i);
                if (element.IsEquals(target)) {
                    return i;
                }
            }

            return -1;
        }
        
        public static SerializedProperty GetNextArrayProperty(this SerializedProperty source, SerializedProperty target)
        {
            if (source.isArray == false)
                return null;
            
            var found = false;
            for (int i = 0; i < source.arraySize; i++) {
                var element = source.GetArrayElementAtIndex(i);
                if (found) return element;
                if (element.IsEquals(target)) {
                    found = true;
                }
                
            }

            return null;
        }
        
        
        public static Object AssetPathToAsset(this string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return null;
            return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
        }
        
        public static Object MarkDirty(this Object asset)
        {
            if (asset == null) return asset;
            EditorUtility.SetDirty(asset);
            return asset;
        }

        public static TAsset CreateAsset<TAsset>(this Object assetLocation,string assetName)
            where TAsset : ScriptableObject
        {
            if(!assetLocation) return default;
            var thisPath = AssetDatabase.GetAssetPath(assetLocation);
            if (string.IsNullOrEmpty(thisPath)) return default;
            var directory = thisPath.GetDirectoryPath();
            var resultStatesPath = directory.CombinePath($"{assetName}.asset");
            var resultAsset = ScriptableObject.CreateInstance<TAsset>();
            AssetDatabase.CreateAsset(resultAsset, resultStatesPath);
            AssetDatabase.Refresh();
            resultAsset = AssetDatabase.LoadAssetAtPath<TAsset>(resultStatesPath);
            return resultAsset;
        }
        
        public static Object SaveAsset(this Object asset)
        {
            if (asset == null) return asset;
            asset.MarkDirty();
            AssetDatabase.SaveAssetIfDirty(asset);
            return asset;
        }
        
        /// <summary>
        /// Gets all childrens of `SerializedObjects`
        /// at 1 level depth if includeChilds == false.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(
            this SerializedObject serializedProperty, 
            bool includeChilds = false)
        {
            var property = serializedProperty.GetIterator();
            return property.GetVisibleChildren(includeChilds);
        }

        /// <summary>
        /// Gets all children of `SerializedProperty`
        /// at 1 level depth if includeChilds == false.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetChildrens(
            this SerializedProperty serializedProperty)
        {
            var currentProperty     = serializedProperty.Copy();
            
            if (!currentProperty.Next(true)) {
                yield break;
            }
            do
            {
                yield return currentProperty;
            }
            while (currentProperty.Next(false));
        }
        
        /// <summary>
        /// Gets visible children of `SerializedProperty`
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetChildren(
            this SerializedProperty serializedProperty, 
            bool includeChilds)
        {
            foreach (var property in GetVisibleChildren(serializedProperty)) {
                yield return property;
                if(!includeChilds) continue;
                foreach (var visibleChild in property.GetChildren(includeChilds)) {
                    yield return visibleChild;
                }
            }
        }
        
        /// <summary>
        /// Gets visible children of `SerializedProperty`
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(
            this SerializedProperty serializedProperty, 
            bool includeChilds)
        {
            foreach (var property in GetVisibleChildren(serializedProperty)) {
                yield return property;
                if(!includeChilds) continue;
                foreach (var visibleChild in property.GetVisibleChildren(includeChilds)) {
                    yield return visibleChild;
                }
            }
        }

        /// <summary>
        /// Gets visible children of `SerializedProperty`
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <param name="includeChilds"></param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(this SerializedProperty serializedProperty)
        {
            var currentProperty     = serializedProperty.Copy();
            
            if (!currentProperty.NextVisible(true)) {
                yield break;
            }
            do
            {
                yield return currentProperty;
            }
            while (currentProperty.NextVisible(false));
        }
        
        
        public static void AddToEditorSelection(this object item, bool add)
        {
            if (!(item is Object asset)) return;
            
            if (add) {
                if (Selection.objects.Contains(item))
                    return;
                var selection = new List<Object>(Selection.objects);
                selection.Add(asset);
                Selection.objects = selection.ToArray();
            }
            else {
                if (Selection.objects.Length == 1 && Selection.objects[0] == item)
                    return;
                Selection.objects = new[] {asset};
            }
        }

        public static void SetDirty(this object asset)
        {
            if(asset is Object assetObject)
                EditorUtility.SetDirty(assetObject);
        }

        public static bool IsSelected(this Object item) => item != null && Selection.Contains(item);
        
        public static bool IsSelected(this object item) => (item as Object).IsSelected();

        public static void PingInEditor(this Object item,bool markAsActive = true)
        {
            if (!item) return;
            
            EditorGUIUtility.PingObject( item );
            if (markAsActive) {
                Selection.activeObject = item;
            }
            
        }
        
        public static void DeselectFromEditor(this Object item)
        {
            if (item == null) return;
            var selection = new List<Object>(Selection.objects);
            selection.Remove(item);
            Selection.objects = selection.ToArray();
        }

        public static IEnumerable<Type> GetClassItems(Type type) {
            return Assembly.GetAssembly(type).GetTypes().Where(t => t.IsSubclassOf(type));
        }
        
        public static void DestroyNestedAsset(this Object asset) {
            
            Object.DestroyImmediate(asset,true);
            AssetDatabase.SaveAssets();
            
        }
        
        public static TTarget AddNested<TTarget>(this ScriptableObject root, string name = null)
            where TTarget : ScriptableObject {
            return AssetEditorTools.SaveAssetAsNested<TTarget>(root, name);
        }
        
        public static Object AddNested(this ScriptableObject root,Type assetType, string name = null)
        {
            return AssetEditorTools.SaveAssetAsNested(root,assetType, name);
        }

        public static Editor ShowCustomEditor(this Object target) {
            if (!target) return null;
            var editor = Editor.CreateEditor(target);
            editor.OnInspectorGUI();
            return editor;
        }
        
        public static Editor GetEditor(this Object target) {
            if (!target) return null;
            var editor = Editor.CreateEditor(target);
            return editor;
        }
        
        public static void DrawDefaultEditor(this GameObject target) {
            if (!target) return;
            var editor = Editor.CreateEditor(target);
            editor.DrawDefaultInspector();
        }

        public static Editor ShowDefaultEditor(this Object target) {
            if (!target) return null;
            var editor = Editor.CreateEditor(target);
            editor.DrawDefaultInspector();
            return editor;
        }

        public static void Save(this SerializedObject target) {
            target.ApplyModifiedProperties();
            EditorUtility.SetDirty(target.targetObject);
        }
    }
}