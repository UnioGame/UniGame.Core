namespace UniModules.UniGame.Context.SerializableContext.Runtime.States
{
    using Cysharp.Threading.Tasks;

    public interface IAsyncStateCommand<TData, TResult>
    {
        UniTask<TResult> ExecuteStateAsync(TData value);
    }
}