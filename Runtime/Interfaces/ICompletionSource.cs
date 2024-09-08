namespace UniGame.Core.Runtime
{
    public interface ICompletionSource : ICompletionStatus
    {
        public void Complete();
    }
}
