namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using UniRx;

    public interface ITypeData :
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