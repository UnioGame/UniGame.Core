namespace UniGame.Core.Runtime
{
    public interface IValueWriter<in TValue>
    {
        void SetValue(TValue value);
    }
}
