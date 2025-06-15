namespace UniGame.Core.Runtime
{
    using Cysharp.Threading.Tasks;

    public interface IAsyncResettable
    {
        UniTask ResetAsync();
    }
}