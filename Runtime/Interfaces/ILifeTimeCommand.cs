namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using DataFlow.Interfaces;

    public interface ILifeTimeCommand
    {
        void Execute(ILifeTime lifeTime);
    }
}