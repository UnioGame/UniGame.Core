namespace UniModules.UniCore.Runtime.Common
{
    using global::UniGame.Core.Runtime;

    public class CompletionSource : ICompletionSource
    {
        public bool IsComplete { get; protected set; }

        public void Complete()
        {
            IsComplete = true;
        }

        public void Dispose()
        {
            IsComplete = false;
        }
    }
}
