namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IState : 
        ICommand, 
        IEndPoint,
        ILifeTimeContext,
        IPoolable,
        IActiveStatus
    {
    }
    
    public interface IState<TResult,TValue> : 
        ICommand<TResult,TValue>, 
        IEndPoint,
        ILifeTimeContext,
        IPoolable,
        IActiveStatus
    {
    }
    

}