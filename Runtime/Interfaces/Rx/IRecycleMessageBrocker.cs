namespace UniModules.UniGame.Core.Runtime.Interfaces.Rx
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniRx;

    public interface IRecycleMessageBrocker : IMessageBroker, IPoolable
    {
    }
}