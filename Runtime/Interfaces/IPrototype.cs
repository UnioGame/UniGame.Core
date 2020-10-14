using Cysharp.Threading.Tasks;

namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface IPrototype<TValue>
    {
        TValue Create();
    }
    
    public interface IAsyncPrototype<TValue>
    {
        UniTask<TValue> Create();
    }
}
