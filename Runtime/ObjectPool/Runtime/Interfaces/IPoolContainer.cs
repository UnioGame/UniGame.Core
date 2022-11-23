namespace UniGame.Core.Runtime.ObjectPool
{
    public interface IPoolContainer
    {
        bool Contains<T>() where T : class;
        
        T Pop<T>() where T : class;

        void Push<T>(T item) where T : class;
    }
}