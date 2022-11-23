
namespace UniGame.Core.Runtime 
{
    using UniGame.Core.Runtime.ObjectPool;

    public interface IActivatableObject : IPoolable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}