namespace UniModules.UniCore.Runtime.Interfaces
{
    using System;

    public interface ICompletionSource : ICompletionStatus
    {
        void Complete();
    }
}
