namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using UniRx;

    public interface IWritableValue
    {
        void CopyTo(IMessagePublisher target);
        
    }
}
