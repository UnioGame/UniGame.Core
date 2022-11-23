using System;

namespace UniGame.Core.Runtime.SerializableType.Attributes
{
    public interface ISerializedTypeFilter
    {
        Type Type { get; }
        string FieldName { get; }
        bool UseFilter { get; }
    }
}