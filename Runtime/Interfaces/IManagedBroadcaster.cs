namespace UniGame.Core.Runtime 
{
    public interface IManagedBroadcaster<in TConnection> : IBroadcaster<TConnection> 
    {
        int BindingsCount { get; }
        
        void Break(TConnection connection);
    }
}