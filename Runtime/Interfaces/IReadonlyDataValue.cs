namespace UniGame.Core.Runtime
{
    public interface IReadonlyDataValue<out TData>
    {
        TData Value { get; }
    }
}