using System.Threading.Tasks;

namespace UniModules.UniCore.Runtime.Interfaces
{
    public interface IAsyncFactory<TResult>
    {

        Task<TResult> Create();

    }
}
