namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniRx;

    public interface ITypeData : 
        IPoolable, 
        IMessageBroker,
        IReadonlyTypeData
    {
        
        bool Remove<TData>();
        
    }

    public interface IReadonlyTypeData :
        IMessageReceiver,
        IReadOnlyData,
        IValueContainerStatus
    {
        
    }
}