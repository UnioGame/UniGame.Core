namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface ICloneable<out TData>
    {
        TData Clone();
    }
}
