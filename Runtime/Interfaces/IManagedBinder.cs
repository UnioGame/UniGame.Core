namespace UniModules.UniGame.Core.Runtime.Interfaces 
{
    public interface IManagedBinder<TConnection> : 
        IBinder<TConnection> 
    {
        int         BindingsCount { get; }
        void        Break(TConnection connection);
    }
}