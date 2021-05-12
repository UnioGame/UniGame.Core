using System.Linq;
using UniModules.UniGame.CoreModules.UniGame.Core.Editor.UiElements;
using UnityEngine.UIElements;

namespace UniModules.UniGame.Core.Runtime.SerializableType.Editor.SerializableTypeEditor
{
    using System;
    using System.Collections.Generic;
    using UniCore.Runtime.ReflectionUtils;
    using UniCore.Runtime.Utils;
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

        public static VisualElement DrawVisualElementTypePopup(string label, Type baseType, Type selectedType,Action<Type> typeSelected)
        {
            //all assignable types
            var types = baseType.GetAssignableTypes();
            var typeNames = types.Select(x => x.Name).ToList();
            var selectedTypeItem = types.FirstOrDefault(x => x == selectedType) ?? types.FirstOrDefault();
            var selectedName = selectedTypeItem == null ? string.Empty : selectedTypeItem.Name;
            
            var field = typeNames.CreateDropDownValue(selectedName, (x, index) =>
            {
                typeSelected?.Invoke(types[index]);
                return x;
            });

            field.label = label;
            
            return field;
        }
        
        public static (string filter,Type type) DrawTypePopup(Rect position,GUIContent label, Type baseType, Type selectedType)
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
            
            var newSelection = EditorGUI.Popup(position, label.text, selectedIndex, popupValues.ToArray());

            var resultType = newSelection == 0 ? null : types[newSelection - 1];
            return (string.Empty, resultType);
        }
        
        
        
    }
}