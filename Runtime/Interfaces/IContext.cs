
namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;
    using UniRx;

    public interface IReadOnlyContext : 
        IMessageBroker,
        ITypeDataConnector<IMessagePublisher>,
        IValueContainerStatus, 
        IReadOnlyData,
        ILifeTimeContext
    {
        
    }
    
    public interface IContext : 
        IReadOnlyContext,
        ITypeData,
        IDisposable
    {
        
    }
}
