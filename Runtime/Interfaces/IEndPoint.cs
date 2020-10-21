namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;

    public interface IEndPoint
    {
        void Exit();
    }
    
    public interface IAsyncEndPoint
    {
        UniTask Exit();
    }
}