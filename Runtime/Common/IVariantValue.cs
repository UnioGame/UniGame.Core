namespace UniModules.UniGame.Core.Runtime.Common
{
    using Interfaces;

    public interface IVariantValue<TApi> : 
        IValueContainerStatus
    {
        TApi Value { get; }
    }
}