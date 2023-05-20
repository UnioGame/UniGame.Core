using UniGame.CoreModules.Editor.SerializableTypeEditor;

namespace UniGame.Core.Runtime.SerializableType.Editor.SerializableTypeEditor
{
    using System;
    using Attributes;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(STypeFilterAttribute))]
    public class STypeInspector : PropertyDrawer
    {
        private Type selection = null;
        private string _filter = String.Empty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetAttribute = attribute as STypeFilterAttribute;
            var targetType      = targetAttribute?.type;

            if (targetType == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            selection = targetAttribute.GetSerializedType(property);

            var newSelection = targetAttribute.useFilter ?
                TypeDrawer.DrawTypeFilterPopup(position, _filter,label, targetType, selection) : 
                TypeDrawer.DrawTypePopup(position,label,targetType,selection,targetAttribute.excludeAbstract);
            
            _filter = newSelection.filter;
            
            if (newSelection.type == selection)
                return;

            selection                  = newSelection.type;
            
            targetAttribute.SetSerializedType(property, selection);
            
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight * 2;
            return height;
        }
    }
}