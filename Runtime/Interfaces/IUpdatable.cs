namespace UniModules.UniGame.Core.Runtime.Interfaces
{

    
    public interface IUpdatable
    {
        void Update();
    }
    
    public interface IUpdatable<TData>
    {
        void Update(TData data);
    }
    
    
}
