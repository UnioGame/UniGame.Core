namespace UniGame.Runtime.ObjectPool
{
    using UniGame.Core.Runtime.ObjectPool;

    public class DummyPoolContainer : IPoolContainer
    {

        public bool Contains<T>() where T : class => false;

        public T Pop<T>() where T : class => null;

        public void Push<T>(T item) where T : class
        {
        }
    }
}
