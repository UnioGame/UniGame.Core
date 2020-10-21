namespace UniModules.UniGame.Core.Runtime.Interfaces {
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IActivatableObject : IPoolable {
        bool IsActive { get; }
        void SetEnabled(bool state);
    }
}