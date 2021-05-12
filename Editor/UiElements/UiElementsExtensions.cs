using System;
using System.Collections.Generic;
using UniModules.UniCore.EditorTools.Editor.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UniModules.UniGame.CoreModules.UniGame.Core.Editor.UiElements
{
    public static class UiElementsExtensions
    {

        public static VisualElement DrawAllChildren(this SerializedObject target, bool drawChildren = true)
        {
            var visualContainer = new VisualElement();

            foreach (var child in target.GetVisibleChildren(drawChildren))
            {
                var view = new PropertyField(child);
                visualContainer.Add(view);
            }

            return visualContainer;
        }

        public static DropdownField CreateDropDownValue(this List<string> items,string selection, Func<string,int,string> selectionFunction)
        {
            if (items == null || items.Count <= 0)
                return new DropdownField();
            
            var selectionIndex = items.IndexOf(selection);
            selectionIndex = selectionIndex < 0 ? 0 : selectionIndex;
            
            var menu = new DropdownField(items,selectionIndex,
                newValue => selectionFunction(newValue,items.IndexOf(newValue)));
           
            return menu;
        }
        
    }
}
