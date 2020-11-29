namespace UniModules.UniGame.Core.Runtime.SerializableType.Editor.SerializableTypeEditor
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
            var targetType      = targetAttribute?.Type;

            if (targetType == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var fieldName = targetAttribute.FieldName;
            //get type name field
            var targetProperty = property.FindPropertyRelative(fieldName);
            if (targetProperty == null)
            {
                Debug.LogError($"property field with name {fieldName} not found");
                return;
            }

            selection = Type.GetType(targetProperty.stringValue, false, true);

            var newSelection = targetAttribute.UseFilter ?
                TypeDrawer.DrawTypeFilterPopup(position, _filter,label, targetType, selection) : 
                TypeDrawer.DrawTypePopup(position,label,targetType,selection);
            
            _filter = newSelection.filter;
            
            if (newSelection.type == selection)
            {
                return;
            }

            selection                  = newSelection.type;
            targetProperty.stringValue = selection == null ? string.Empty : selection.AssemblyQualifiedName;
            
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight * 2;
            return height;
        }
    }
}