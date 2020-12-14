namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;

    public interface IUpdatableJob
    {
        PlayerLoopTiming UpdateType { get; }
        
        void Update();
    }
    
    public interface IUpdatable
    {
        void Update();
    }
    
    public interface IUpdatable<TData>
    {
        void Update(TData data);
    }
    
    
}
