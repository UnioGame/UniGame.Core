namespace UniGame.Core.Runtime
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
