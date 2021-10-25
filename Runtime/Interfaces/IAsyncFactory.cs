using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface IAsyncFactory<TResult>
    {

        UniTask<TResult> Create();

    }
    
    public interface IAsyncFactory<TValue,TResult>
    {
        UniTask<TResult> Create(TValue value);
    }
}
