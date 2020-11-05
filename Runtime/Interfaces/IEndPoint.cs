namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;

    public interface IEndPoint
    {
        void Exit();
    }
    
    public interface IAsyncEndPoint<TData>
    {
        UniTask Exit(TData data);
    }
    
    public interface IAsyncEndPoint
    {
        UniTask Exit();
    }
}