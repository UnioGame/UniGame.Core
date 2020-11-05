namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniRx;

    public interface ITypeData : 
        IPoolable, 
        IMessageBroker,
        IValueContainerStatus, 
        IReadOnlyData
    {
        
        bool Remove<TData>();
        
    }
}