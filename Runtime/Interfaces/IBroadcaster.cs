namespace UniGame.Core.Runtime
{
    using System;

    public interface IBroadcaster<in T>
    {
        /// <summary>
        /// retranslate all data of connecter to target connection
        /// </summary>
        /// <param name="connection">target output channel</param>
        /// <returns>disposable connection token</returns>
        IDisposable Broadcast(T connection);
    }
}