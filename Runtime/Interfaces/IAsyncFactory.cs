namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System.Threading.Tasks;

    public interface IAsyncFactory<TResult>
    {

        Task<TResult> Create();

    }
}
