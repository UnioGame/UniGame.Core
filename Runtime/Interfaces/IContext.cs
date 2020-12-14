
namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;
    using UniRx;

    public interface IReadOnlyContext :
        ILifeTimeContext,
        IReadonlyTypeData
    {
        
    }
    
    public interface IMessageContext : 
        IReadOnlyContext,
        IMessageBroker
    { }
    
    public interface IContext :
        IMessageContext,
        IManagedBinder<IMessagePublisher>,
        ITypeData
    {
        
    }

    public interface IDisposableContext : 
        IContext, 
        IDisposable
    {
        
    }
}
