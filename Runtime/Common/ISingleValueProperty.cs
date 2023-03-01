namespace UniGame.Runtime.Common
{
    public interface ISingleValueProperty<TValue>
    {
        bool Has { get; }

        void SetValue(TValue value);
        
        TValue Take();

        TValue Look();
    }
}