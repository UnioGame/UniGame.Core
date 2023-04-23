namespace UniGame.Runtime.Common
{
    public interface ISignaleValueProperty<TValue>
    {
        bool Has { get; }

        void SetValue(TValue value);
        
        TValue Take();
        
        bool Take(out TValue result);

        TValue Look();
    }
}