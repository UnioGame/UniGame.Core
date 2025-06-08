namespace UniGame.Core.Editor
{
    using System;
    using System.Collections.Generic;

    public interface IEditorProcessor<TData> : IDisposable
    {
        void Proceed(IReadOnlyList<TData> data);

        void Start();
        
        void Disable();
    }
    
}