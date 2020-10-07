
namespace UniModules.UniCore.Runtime.Interfaces
{
    using System;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public interface IContext : 
        ITypeData,
        IDisposable,
        ILifeTimeContext{
        
    }
}
