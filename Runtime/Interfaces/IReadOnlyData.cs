namespace UniGame.Core.Runtime
{
    public interface IReadOnlyData
    {
        TData Get<TData>();
        bool Contains<TData>();
    }
}