namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;

    public interface ITypeDataObservable
    {
        
        IObservable<T> GetObservable<T>();
        
    }
}