namespace UniGame.Core.Runtime.Rx
{
    using System;

    public interface IReadonlyObjectValue
    {
        Type Type { get; }
        object GetValue();
    }
    
    public interface IObjectValue
    {
        void SetObjectValue(object value);
    }
}