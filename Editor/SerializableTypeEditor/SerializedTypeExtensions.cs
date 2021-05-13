using System;
using Sirenix.OdinInspector.Editor.Drawers;
using UniModules.UniCore.Runtime.ReflectionUtils;
using UniModules.UniGame.Core.Runtime.SerializableType;
using UniModules.UniGame.Core.Runtime.SerializableType.Attributes;
using UniModules.UniGame.Core.Runtime.SerializableType.Editor.SerializableTypeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniModules.UniGame.CoreModules.UniGame.Core.Editor.SerializableTypeEditor
{
    public static class SerializedTypeExtensions
    {
        
        public static Type GetSerializedType(this SerializedProperty property)
        {
            var fieldName = nameof(SType.fullTypeName);
            return GetSerializedType(property, fieldName);
        }
        
        public static void SetSerializedType(this SerializedProperty property,Type type)
        {
            var fieldName = nameof(SType.fullTypeName);
            SetSerializedType(property,type,fieldName);
        }

        public static Type GetSerializedType(SerializedProperty property,string fieldName)
        {
            //get type name field
            var targetProperty = property.FindPropertyRelative(fieldName);
            if (targetProperty == null)
            {
                Debug.LogError($"property field with name {fieldName} not found");
                return null;
            }

            var selection = ReflectionTools.ConvertType(targetProperty.stringValue);
            return selection;
        }
        
        public static Type GetSerializedType(this ISerializedTypeFilter filter,SerializedProperty property)
        {
            return GetSerializedType(property, filter.FieldName);
        }
        
        public static SerializedProperty SetSerializedType(this SerializedProperty property, Type type, string fieldName)
        {
            //get type name field
            var targetProperty = property.FindPropertyRelative(fieldName);
            if (targetProperty == null)
            {
                Debug.LogError($"property field with name {fieldName} not found");
                return null;
            }
            
            targetProperty.stringValue = type == null ? 
                string.Empty : 
                type.AssemblyQualifiedName;

            property.serializedObject
                .ApplyModifiedProperties();
            
            return property;
        }
        
        public static SerializedProperty SetSerializedType(this ISerializedTypeFilter filter,SerializedProperty property,Type selection)
        {
            var fieldName = filter.FieldName;
            //get type name field
            return SetSerializedType(property, selection, fieldName);
        }
    }
}