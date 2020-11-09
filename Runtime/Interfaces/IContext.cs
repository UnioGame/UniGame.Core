
namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;
    using UniRx;

    public interface IReadOnlyContext : 
        IMessageContext,
        ITypeDataConnector<IMessagePublisher>,
        IValueContainerStatus, 
        IReadOnlyData { }
    
    public interface IMessageContext : 
        IMessageBroker,
        ILifeTimeContext { }
    
    public interface IContext : 
        IReadOnlyContext,
        ITypeData,
        IDisposable{ }
}
