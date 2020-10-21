namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface IValueWriter<in TValue>
    {
        void SetValue(TValue value);
    }
}
