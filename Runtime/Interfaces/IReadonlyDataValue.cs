namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface IReadonlyDataValue<out TData>
    {
        TData Value { get; }
    }
}