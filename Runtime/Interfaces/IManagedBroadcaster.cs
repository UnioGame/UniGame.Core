namespace UniModules.UniGame.Core.Runtime.Interfaces 
{
    public interface IManagedBroadcaster<in TConnection> : IBroadcaster<TConnection> 
    {
        int BindingsCount { get; }
        
        void Break(TConnection connection);
    }
}