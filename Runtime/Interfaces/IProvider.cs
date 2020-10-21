namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface ISelector<TSource,TResult>
    {

        TResult Select(TSource source);

    }
}
