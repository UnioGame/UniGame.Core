namespace UniGame.Core.Runtime
{
    using System;

    public interface ITypeDataObservable
    {
        
        IObservable<T> GetObservable<T>();
        
    }
}