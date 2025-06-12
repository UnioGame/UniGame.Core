﻿using UniModules.UniCore.EditorTools.Editor;
using UniGame.Runtime.ReflectionUtils;

namespace UniModules.Editor {
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class EditorValueItem<TValue> 
    {
        public string Name = string.Empty;
        public bool IsOpen = false;
        public TValue Value;

        public void Release()
        {
            Value = default(TValue);
            Name = string.Empty;
        }
            
    }
    
    public static class EditorDrawerUtils {
        
        public static Type UnityObjectType = typeof(Object);
        
        public static GUIStyle EmptyFoldOutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fixedWidth = 10,
        };

        public static GUIStyle BoxStyle = new GUIStyle(EditorStyles.helpBox);

        public static bool DrawButton(string label,Action action,params GUILayoutOption[] options)
        {
            if (GUILayout.Button(label, options))
            {
                action?.Invoke();
                return true;
            }

            return false;
        }

        public static bool DrawObjectFoldout(Object asset, bool foldout, Type targetType, string label = "", Action<Object> onValueChanged = null)
        {
            if (targetType == null || asset == null) {
                var target = EditorGUILayout.ObjectField(label, asset, UnityObjectType, true);
                if (target != asset) {
                    asset = target;
                    onValueChanged?.Invoke(target);
                }
                return false;
            }

            var rect = GUILayoutUtility.GetLastRect();
            //rect.x -= 16;
            //foldout = EditorGUI.Foldout(rect, foldout, string.Empty);
            foldout = EditorGUILayout.Foldout(foldout,string.Empty);            
            rect = GUILayoutUtility.GetLastRect();

            var newAsset = EditorGUI.ObjectField(rect,label, asset, targetType, true);
            if (newAsset != asset) {
                asset = newAsset;
                onValueChanged?.Invoke(newAsset);
            }

            return foldout;
        }

        public static bool DrawFieldFoldout(
            SerializedProperty property, 
            bool foldOut, 
            GUIContent label,
            bool includeChildren = true)
        {
            EditorGUILayout.PropertyField(property,label, includeChildren);
            var rect = GUILayoutUtility.GetLastRect();
            rect.x -= 14;
            foldOut = EditorGUI.Foldout(rect,foldOut,string.Empty);            
            return foldOut;
        }
        
        public static void DrawDisabled(Action drawerAction, bool disabled = true)
        {
            EditorGUI.BeginDisabledGroup(disabled);
            drawerAction?.Invoke();
            EditorGUI.EndDisabledGroup();
        }
        
        public static bool ActionDrawerSwitcher(bool selection,string label,Action selectedDrawer,Action unselectedDrawer)
        {
            var result = EditorGUILayout.Toggle(label, selection);
            
            if (result)
            {
                selectedDrawer?.Invoke();
            }
            else
            {
                unselectedDrawer?.Invoke();
            }

            return result;
        }
        
        public static void DrawVertialLayout(Action action,GUIStyle style, params GUILayoutOption[] options) {
            
            if (action == null) return;

            try {
                if (style != null && options != null) {
                    EditorGUILayout.BeginVertical(style,options);               
                }
                else if(style!=null){
                    EditorGUILayout.BeginVertical(style);  
                }
                else if (options != null) {
                    EditorGUILayout.BeginVertical(options);
                }
                else {
                    EditorGUILayout.BeginVertical();
                }

                action();

                EditorGUILayout.EndVertical();
            }
            catch (Exception e) {
                Debug.LogError(e);
                GUIUtility.ExitGUI();
            }

            
        }

        public static void DrawVertialLayout(Action action, params GUILayoutOption[] options) {
            
            DrawVertialLayout(action, null, options);

        }

        public static void DrawLabelField(GUIContent label,GUIStyle style, params GUILayoutOption[] options)
        {
            if (style == null) {
                EditorGUILayout.LabelField(label,GUILayout.MinWidth(30));
                return;
            }
            EditorGUILayout.LabelField(label, style, options);
        }

        public static void WrapDrawer(this object target,Action drawerAction,bool warningMode = false)
        {
            try {
                drawerAction?.Invoke();
            }
            catch (Exception e) {
                if (warningMode) {
                    Debug.LogWarning(e);
                }
                else {
                    Debug.LogError(e.Message);
                }
                
                GUIUtility.ExitGUI();
                throw e;
            }
        }
        
        public static void DrawArrayProperty(SerializedProperty property, Action<SerializedProperty> drawer)
        {

            if (property == null || drawer == null || property.isArray == false)
                return;

            for (int i = 0; i < property.arraySize; i++)
            {
                var item = property.GetArrayElementAtIndex(i);
                drawer(item);
            }
            
        }

        public static void DrawArrayProperty<TContent>(SerializedProperty property, Action<SerializedProperty,TContent> drawer)
            where TContent : class
        {
            DrawArrayProperty(property, x =>
            {
                var value = property.objectReferenceValue as TContent;
                if (value == null)
                    return;
                drawer(x, value);
            });
        }

        public static void DrawVertialLayout(Action action, Color color, params GUILayoutOption[] options) {
            var prevColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            DrawVertialLayout(action, options);
            GUI.backgroundColor = prevColor;
        }

        public static void DrawHorizontalLayout(Action action, params GUILayoutOption[] guiLayoutOptions) {
            
            if (action == null) return;

            try {
                if (guiLayoutOptions == null) {
                    EditorGUILayout.BeginHorizontal();
                }
                else {
                    EditorGUILayout.BeginHorizontal(guiLayoutOptions);
                }

                action();
            }
            catch (Exception e) {
                Debug.LogError(e);
                GUIUtility.ExitGUI();
            }
            
        }

        public static void DrawZoom(Action action,Rect rect, float zoom, float topPadding)
        {
            BeginZoom(rect, zoom, topPadding);

            action?.Invoke();

            EndZoom(rect, zoom, topPadding);
        }

        public static void BeginZoom(Rect rect, float zoom, float topPadding)
        {
            GUI.EndClip();

            GUIUtility.ScaleAroundPivot(Vector2.one / zoom, rect.size * 0.5f);
            var padding = new Vector4(0, topPadding, 0, 0);
            padding *= zoom;
            
            GUI.BeginClip(new Rect(-((rect.width * zoom) - rect.width) * 0.5f,
                -(((rect.height * zoom) - rect.height) * 0.5f) + (topPadding * zoom),
                rect.width * zoom,
                rect.height * zoom));
        }

        public static void EndZoom(Rect rect, float zoom, float topPadding)
        {
            GUIUtility.ScaleAroundPivot(Vector2.one * zoom, rect.size * 0.5f);
            var offset = new Vector3(
                (((rect.width * zoom) - rect.width) * 0.5f),
                (((rect.height * zoom) - rect.height) * 0.5f) + (-topPadding * zoom) + topPadding,
                0);
            GUI.matrix = Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);
        }
        
        public static void DrawAndRevertColor(Action action) {
            var defaultBackColor = GUI.backgroundColor;
            var defaultGuiColor = GUI.color;
            var defaultContentColor = GUI.contentColor;
            action();
            GUI.backgroundColor = defaultBackColor;
            GUI.color = defaultGuiColor;
            GUI.contentColor = defaultContentColor;
        }

        public static void DrawVecticalBox(Action action) {

            DrawVertialLayout(action,GUI.skin.box);
            
        }
        
        public static void DrawWithContentColor(Color color,Action action) {

            if (action == null) return;

            DrawAndRevertColor(() => {
                
                GUI.contentColor = color;
                action();

            });
            
        }
        
        public static void DrawWithBackgroundColor(Color color,Action action) {

            if (action == null) return;

            DrawAndRevertColor(() => {
                
                GUI.backgroundColor = color;
                action();

            });
            
        }
        
        public static void DrawWithGuiColor(Color color,Action action) {

            if (action == null) return;

            DrawAndRevertColor(() => {
                
                GUI.color = color;
                action();

            });
            
        }

        public static void DrawWithColors(Color color,Color background, Color content,Action action) {

            if (action == null) return;

            DrawAndRevertColor(() => {
                
                GUI.color = color;
                GUI.backgroundColor = background;
                GUI.contentColor = content;
                
                action();

            });
            
        }
        
        public static bool DrawFoldout(bool visibility,string label,Action drawer)
        {

            visibility = EditorGUILayout.Foldout(visibility, label);

            if (visibility)
            {
                drawer();
            }
            
            return visibility;
            
        }

        public static void DrawListItems<TItem>(List<EditorValueItem<TItem>> items, 
            Action<int,EditorValueItem<TItem>> itemDrawer = null, Action<int,EditorValueItem<TItem>> onRemoveItem = null)
        {
            if (items == null)
                return;
            EditorGUILayout.BeginVertical();
            
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                EditorGUILayout.BeginHorizontal();
                {
                    item.IsOpen = EditorGUILayout.Foldout(item.IsOpen, "", EmptyFoldOutStyle);

                    var objectItem = item.Value as Object;
                    if (objectItem)
                    {
                        EditorGUILayout.ObjectField(objectItem, objectItem.GetType(), true);
                    }

                    EditorGUILayout.BeginHorizontal(GUILayout.Width(60f));

                    if (i != 0)
                    {
                        if (GUILayout.Button("up", EditorStyles.miniButtonLeft))
                        {
                            SwapItems(item, items[i - 1]);
                        }
                    }

                    if (i != (items.Count - 1))
                    {
                        if (GUILayout.Button("down", EditorStyles.miniButtonMid))
                        {
                            SwapItems(item, items[i + 1]);
                        }
                    }

                    if (GUILayout.Button("-", EditorStyles.miniButtonRight))
                    {
                        onRemoveItem?.Invoke(i, item);
                    }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();

                if (item.IsOpen)
                {
                    itemDrawer?.Invoke(i,item);
                }
            }

            
            EditorGUILayout.EndVertical();
            
        }

        private static void SwapItems<TItem>(
            EditorValueItem<TItem> first,
            EditorValueItem<TItem> second )
        {
            var name = first.Name;
            var value = first.Value;
            var open = first.IsOpen;
            
            first.Value = second.Value;
            first.Name = second.Name;
            first.IsOpen = second.IsOpen;

            second.Value = value;
            second.Name = name;
            second.IsOpen = open;
        }
        
        public static Vector2 DrawScroll(Vector2 scroll,Action drawer,params GUILayoutOption[] options) {
            
            if (drawer == null)
                return scroll;
            
            scroll = EditorGUILayout.BeginScrollView(scroll,options);
            
            drawer();

            EditorGUILayout.EndScrollView();

            return scroll;
        }

        public static T DrawObjectField<T>(this T target, string label)
            where T : Object  
        {

            var asset = EditorGUILayout.ObjectField(label, target, typeof(T), false) as T;
            return asset;
            
        }

        public static T DrawObjectLayout<T>(this T target, 
            string label,bool drawRemove = true, 
            bool allowSceneObjects = false)
            where T : Object {
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            var asset = EditorGUILayout.ObjectField(label, target, typeof(T), allowSceneObjects) as T;
            if (drawRemove && GUILayout.Button("-",EditorStyles.miniButtonRight, GUILayout.Width(20f))) {
                return null;
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            return asset;
        }
        
        public static Object DrawObject(Object item, bool showObjectField = true) {
            var editor = item.GetEditor();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            var result = item;
            if (showObjectField)
            {
                result = EditorGUILayout.ObjectField(item, item.GetType(),true);
            }
            editor.OnInspectorGUI();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            return result;
        }


        public static SerializedProperty ResetToDefault(this SerializedProperty property)
        {

            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = 0;
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = false;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = 0f;
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = String.Empty;
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = Color.white;
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = null;
                    break;
                case SerializedPropertyType.Enum:
                    property.enumValueIndex = 0;
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = Vector2.zero;
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = Vector3.zero;
                    break;
                case SerializedPropertyType.Vector4:
                    property.vector4Value = Vector4.zero;
                    break;
                case SerializedPropertyType.Rect:
                    property.rectValue = Rect.zero;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    property.animationCurveValue = new AnimationCurve();
                    break;
                case SerializedPropertyType.Bounds:
                    property.boundsValue = new Bounds();
                    break;
                case SerializedPropertyType.Quaternion:
                    property.quaternionValue = Quaternion.identity;
                    break;
                case SerializedPropertyType.ExposedReference:
                    property.exposedReferenceValue = null;
                    break;
                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = Vector2Int.zero;
                    break;
                case SerializedPropertyType.Vector3Int:
                    property.vector3IntValue = Vector3Int.zero;
                    break;
                case SerializedPropertyType.RectInt:
                    property.rectIntValue = new RectInt();
                    break;
                case SerializedPropertyType.BoundsInt:
                    property.boundsIntValue = new BoundsInt();
                    break;
                case SerializedPropertyType.ManagedReference:
                    property.managedReferenceValue = property.
                        GetTypeReflection().
                        GetDefaultInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return property;
        }
        
    }
}