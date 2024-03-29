namespace UniModules.UniResourceSystem.Runtime.Interfaces
{
    using global::UniGame.Core.Runtime;

    /// <summary>
    /// resource item wrapper
    /// </summary>
    public interface IResource : INamedItem
    {
        /// <summary>
        /// is value assigned
        /// </summary>
        /// <returns></returns>
        bool HasValue();

        /// <summary>
        /// load resource with type T
        /// </summary>
        T Load<T>() where T : class;
    }
}