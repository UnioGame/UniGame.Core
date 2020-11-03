namespace UniModules.UniGame.Core.Runtime.Interfaces 
{
    public interface ITypeDataConnector<TConnection> : 
        IConnector<TConnection> 
    {
        int         ConnectionsCount { get; }
        void        Disconnect(TConnection connection);
    }
}