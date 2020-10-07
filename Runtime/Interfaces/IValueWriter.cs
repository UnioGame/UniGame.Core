namespace UniModules.UniCore.Runtime.Interfaces
{
    public interface IValueWriter<in TValue>
    {
        void SetValue(TValue value);
    }
}
