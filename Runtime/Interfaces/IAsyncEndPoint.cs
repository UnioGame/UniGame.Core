namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;

    public interface IAsyncEndPoint<T>
    {
        UniTask Exit();
    }

}