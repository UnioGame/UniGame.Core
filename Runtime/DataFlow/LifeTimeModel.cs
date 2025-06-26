namespace UniGame.Runtime.DataFlow
{
    using Interfaces;
    using global::UniGame.Core.Runtime.ObjectPool;
    using global::UniGame.Core.Runtime;

    public class LifeTimeModel : ILifeTimeModel, IPoolable
    {
        
        private LifeTime lifeTimeDefinition = new();

        public ILifeTime LifeTime => lifeTimeDefinition;


        /// <summary>
        /// cleanup item without despawn
        /// </summary>
        public void Release()
        {
            lifeTimeDefinition.Release();
            OnCleanUp();
        }
        
        /// <summary>
        /// custom cleanup action
        /// </summary>
        protected virtual void OnCleanUp(){}

        /// <summary>
        /// despawn movel
        /// </summary>
        public void Dispose() => lifeTimeDefinition.Terminate();
    }
}
