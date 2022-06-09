namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;
    using DataFlow.Interfaces;

    public interface ILifeTimeCommand
    {
        UniTask Execute(ILifeTime lifeTime);
    }
}