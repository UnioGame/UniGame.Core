using UniModules.UniCore.Runtime.ReflectionUtils;

namespace UniGame.Core.Runtime.SerializableType.Editor.SerializableTypeEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public static class TypeDrawer
    {
        
        private const string filterLabel = "filter";
        private const string noneValue   = "none";
        
        private static List<string> popupValues = new List<string>();
        
        public static (string filter,Type type) DrawTypeFilterPopup(Rect position,string filter,GUIContent label, Type baseType, Type selectedType)
        {
            //all assignable types
            var types = baseType.GetAssignableTypes();

            var selectedIndex = 0;

            filter     =  EditorGUI.TextField(
                new Rect(position.position, new Vector2(position.width,
                        EditorGUIUtility.singleLineHeight)), filterLabel, filter);

            popupValues.Clear();
            popupValues.Add(noneValue);
            
            for (var i = 0; i < types.Count; i++) {
                var item      = types[i];
                if (!string.IsNullOrEmpty(filter) && item.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }
                var itemIndex = i + 1;
                
                popupValues.Add(item.Name);
                selectedIndex = item == selectedType ? 
                    itemIndex : selectedIndex;
            }

            var popupPosition = new Vector2(position.x, position.y + EditorGUIUtility.singleLineHeight);
            var popupSize     = new Vector2(position.width, EditorGUIUtility.singleLineHeight);
            var newSelection = EditorGUI.Popup(
                new Rect(popupPosition,popupSize), 
                label.text, selectedIndex, 
                popupValues.ToArray());

            var resultType = newSelection == 0 ? null : types[newSelection - 1];
            return (filter, resultType);
        }

        public static (string filter,Type type) DrawLayoutTypePopup(GUIContent label, Type baseType, Type selectedType)
        {
            //all assignable types
            var types = baseType.GetAssignableTypes();

            var selectedIndex = 0;
            
            popupValues.Clear();
            popupValues.Add(noneValue);
            for (var i = 0; i < types.Count; i++) {
                var item      = types[i];
                var itemIndex = i + 1;
                
                popupValues.Add(item.Name);
                selectedIndex = item == selectedType ? 
                    itemIndex : selectedIndex;
            }
            
            var newSelection = EditorGUILayout.Popup(label.text, selectedIndex, popupValues.ToArray());

            var resultType = newSelection == 0 ? null : types[newSelection - 1];
            return (string.Empty, resultType);
        }

        
        public static (string filter,Type type) DrawTypePopup(
            Rect position,
            GUIContent label, 
            Type baseType,
            Type selectedType,
            bool filterAbstract)
        {
            //all assignable types
            var types = baseType.GetAssignableTypes(filterAbstract);

            var selectedIndex = 0;
            
            popupValues.Clear();
            popupValues.Add(noneValue);
            for (var i = 0; i < types.Count; i++) {
                
                var item      = types[i];
                if(filterAbstract && (item.IsAbstract || item.IsInterface))
                    continue;
                
                var itemIndex = i + 1;
                
                popupValues.Add(item.Name);
                selectedIndex = item == selectedType ? 
                    itemIndex : selectedIndex;
            }
            
            var newSelection = EditorGUI.Popup(position, label.text, selectedIndex, popupValues.ToArray());

            var resultType = newSelection == 0 ? null : types[newSelection - 1];
            return (string.Empty, resultType);
        }

        
    }
}