using System;
using System.Collections.Generic;
using UniModules.UniCore.EditorTools.Editor.Utility;
using UniModules.UniCore.Runtime.Utils;
using UniModules.UniGame.Core.Runtime.DataStructure;
using UnityEditor;
using UnityEngine;

namespace UniModules.UniGame.Core.Editor.Controls.SerializableDictionary
{
    [CustomPropertyDrawer(typeof(SerializableDictionaryAttribute))]
    public class SerializableDictionaryEditor : PropertyDrawer
    {
        private static string keysField = "keys";
        private static string valuesField = "values";

        private string _key;
        private string _value;
        private int _removeIndex = -1;
        private List<SerializedProperty> _propertyItems = new List<SerializedProperty>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _removeIndex = -1;
            _propertyItems.Clear();
            
            var targetAttribute = attribute as SerializableDictionaryAttribute;

            var keys = property.FindPropertyRelative(keysField);
            var values = property.FindPropertyRelative(valuesField);

            EditorGUI.BeginProperty(position, label, property);
            
            if (GUILayout.Button("Add"))
                AddKeyValue(keys,values);
            if (GUILayout.Button("Reset"))
            {
                Reset(keys, values);
                property.Reset();
            }

            DrawKeyPairs(property, keys, values);
 
            if(_removeIndex>=0)
                Delete(_removeIndex,keys,values);
                       
            EditorGUI.EndProperty();
            
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        public void AddKeyValue( SerializedProperty keys,SerializedProperty values)
        {
            keys.InsertArrayElementAtIndex(0);
            values.InsertArrayElementAtIndex(0);
            keys.GetArrayElementAtIndex(0).ResetToDefault();
            values.GetArrayElementAtIndex(0).ResetToDefault();
        }

        public void Reset( SerializedProperty keys,SerializedProperty values)
        {
            if (keys.arraySize == 0)
                return;

            keys.ClearArray();
            values.ClearArray();

        }

        public void Delete(int index, SerializedProperty keys, SerializedProperty values)
        {
            keys.DeleteArrayElementAtIndex(index);
            values.DeleteArrayElementAtIndex(index);
            _removeIndex = -1;
        }

        public bool DrawKeyPairs(SerializedProperty property, SerializedProperty keys,SerializedProperty values)
        {
            if (keys.arraySize == 0)
                return false;
            
            var count = keys.arraySize;
            
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(keysField);
            EditorGUILayout.LabelField(valuesField);
            
            GUILayout.EndHorizontal();

            var changed = false;
            
            for (int i = 0; i < count; i++)
            {
                var key = keys.GetArrayElementAtIndex(i);
                var value = values.GetArrayElementAtIndex(i);
                
                GUILayout.BeginHorizontal();

                EditorGUIUtility.labelWidth = 10;
                changed |= EditorGUILayout.PropertyField(key,new GUIContent(i.ToStringFromCache()),true);
                changed |= EditorGUILayout.PropertyField(value, new GUIContent(string.Empty), true);
                EditorGUIUtility.labelWidth = 0;
                
                if (GUILayout.Button("-",GUILayout.Width(30)))
                {
                    _removeIndex = i;
                }
                    
                GUILayout.EndHorizontal();
            }

            return changed;
        }
    }

}
