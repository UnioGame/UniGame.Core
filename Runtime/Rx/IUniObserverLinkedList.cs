namespace UniModules.UniGame.Core.Runtime.Rx
{
    public interface IUniObserverLinkedList<T>
    {
        void UnsubscribeNode(UniObserverNode<T> node);
    }
}