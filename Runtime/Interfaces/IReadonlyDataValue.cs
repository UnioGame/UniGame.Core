namespace UniModules.UniCore.Runtime.Interfaces
{
    public interface IReadonlyDataValue<out TData>
    {
        TData Value { get; }
    }
}