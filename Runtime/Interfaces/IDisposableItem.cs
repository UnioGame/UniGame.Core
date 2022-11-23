namespace UniGame.Core.Runtime
{
    using System;

    public interface IDisposableItem : 
        IDisposable, 
        ICompletionSource
    {
    }
}
