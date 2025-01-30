﻿namespace UniGame.Core.Runtime.SerializableType
{
    using System;
    using GameRuntime.Types;
    using UnityEngine;

    [Serializable]
    public class SType : ISerializationCallbackReceiver, 
        IReadOnlyType, 
        IEquatable<SType>,
        IEquatable<Type>
#if ODIN_INSPECTOR
    ,Sirenix.OdinInspector.ISearchFilterable
#endif
    {
        public string name = string.Empty;
        public string fullTypeName= string.Empty;
        public Type type;

        public Type Type {
            get => GetItemType();
            set {
                type = value;
                name = value.Name;
#if UNITY_EDITOR
                fullTypeName = type == null ? 
                    string.Empty : type.AssemblyQualifiedName;
#endif
            }
        }

        public string Name => type == null ? string.Empty : type.Name;

        public string TypeName {
            get => fullTypeName;
            set => type = Type.GetType(value, false, true);
        }
		
        public Type GetItemType()
        {
            if (type != null) return type;
            if (string.IsNullOrEmpty(fullTypeName)) return type;
            type = Type.GetType(fullTypeName, false, false);
            return type;
        }

        public bool Equals(SType stype) => Type == stype.Type;
        
        public bool Equals(Type stype) => Type == stype;

        public override bool Equals(object obj)
        {
            switch (obj) {
                case Type objectType:
                    return Type == objectType;
                case SType stype:
                    return Type == stype.Type;
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
           var typeValue = Type;
           return typeValue == null ? 
               0 : 
               typeValue.GetHashCode();
        }

        public bool IsMatch(string searchString)
        {
            if(string.IsNullOrEmpty(searchString)) return true;
            if(Type?.FullName == null) return false;
            return Type.FullName.Contains(searchString);
        }

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
            //fullTypeName = type?.FullName;
        }

        public void OnAfterDeserialize()
        {
            type = Type.GetType(fullTypeName, false, true);
            name = type == null ? string.Empty : type.Name;
        }

        #endregion
        
        public static implicit operator Type(SType type) => type?.Type;

        public static implicit operator SType(Type type) => new SType(){Type = type};

    }
}
