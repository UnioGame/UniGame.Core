namespace UniGame.Core.Editor
{
    using System.Collections.Generic;
    using UnityEngine;

    public class BaseEditorProcessorAsset<TData> : ScriptableObject, IEditorProcessor<TData>
    {
        public virtual void Dispose()
        {
            
        }

        public virtual void Proceed(IReadOnlyList<TData> data)
        {
            
        }

        public virtual void Initialize()
        {
            
        }

        public virtual void Start()
        {
            
        }

        public virtual void Disable()
        {
            
        }
    }
}