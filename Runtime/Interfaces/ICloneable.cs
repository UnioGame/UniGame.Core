namespace UniGame.Core.Runtime
{
    public interface ICloneable<out TData>
    {
        TData Clone();
    }
}
