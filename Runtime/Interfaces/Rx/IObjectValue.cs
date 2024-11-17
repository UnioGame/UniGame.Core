namespace UniGame.Core.Runtime.Rx
{
    using System;

    public interface IReadonlyObjectValue
    {
        Type Type { get; }
        
        object ObjectValue { get; }
    }
    
    public interface IObjectValue
    {
        void SetObjectValue(object value);
    }
}