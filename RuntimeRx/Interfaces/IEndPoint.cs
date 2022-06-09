namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;

    public interface IEndPoint
    {
        void Exit();
    }
    
    public interface IAsyncEndPoint<TData>
    {
        UniTask ExitAsync(TData data);
    }
    
    public interface IAsyncEndPoint
    {
        UniTask ExitAsync();
    }
}