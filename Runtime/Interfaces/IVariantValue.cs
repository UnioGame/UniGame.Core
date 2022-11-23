namespace UniModules.UniGame.Core.Runtime.Common
{
    using global::UniGame.Core.Runtime;

    public interface IVariantValue<TApi> : 
        IValueContainerStatus
    {
        TApi Value { get; }
    }
}