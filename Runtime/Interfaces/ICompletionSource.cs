namespace UniGame.Core.Runtime
{
    public interface ICompletionSource : ICompletionStatus
    {
        void Complete();
    }
}
