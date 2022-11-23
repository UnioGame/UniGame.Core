namespace UniGame.Core.Runtime
{
    public interface IPrototype<TValue>
    {
        TValue Create();
    }

}
