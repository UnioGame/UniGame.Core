using UnityEngine;

namespace UniModules.UniGame.Core.Runtime.AsyncOperations
{
    using System;
    using Common;
    using Cysharp.Threading.Tasks;
    using Interfaces;

    
    [Serializable]
    public abstract class AsyncVariantCommand<TVariant,TData> :
        IAsyncCommand<TData, AsyncStatus> 
        where TVariant : IVariantValue<IAsyncCommand<TData, AsyncStatus>>
    {
        [SerializeField]
        public TVariant asyncCommand;

        public async UniTask<AsyncStatus> ExecuteAsync(TData value)
        {
            if (!asyncCommand.HasValue)
                return AsyncStatus.Succeeded;
            
            var result = await asyncCommand.Value.ExecuteAsync(value);
            return result;
        }
    }

}
