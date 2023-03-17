namespace UniGame.Runtime.Common
{
    public interface ISignaleValueProperty<TValue>
    {
        bool Has { get; }

        void SetValue(TValue value);
        
        TValue Take();

        TValue Look();
    }
}