namespace UniGame.Core.Runtime
{
    public interface ISelector<TSource,TResult>
    {

        TResult Select(TSource source);

    }
}
