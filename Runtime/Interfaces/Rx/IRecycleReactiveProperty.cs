namespace UniModules.UniGame.Core.Runtime.Interfaces.Rx
{
    using System;
    using UniRx;

    public interface IRecycleReactiveProperty<TValue> : 
        IReactiveProperty<TValue>,
        IReadonlyRecycleReactiveProperty<TValue>,
        IValueContainerStatus,
        IDisposable
#if UNITY_EDITOR
        ,IReadonlyObjectValue
        ,IObjectValue
#endif
        
    {
        new TValue Value { get; set; }

        void SetValueForce(TValue propertyValue);

        void SetValueSilence(TValue value);

        void RemoveValueSilence();
    }
}
