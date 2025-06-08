namespace UniGame.Core.Editor
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class BaseEditorProcessor<TData> :  IEditorProcessor<TData> 
    {
        public virtual void Dispose()
        {
        }

        public virtual void Add(TData view)
        {
        }

        public virtual void Remove(TData view)
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
