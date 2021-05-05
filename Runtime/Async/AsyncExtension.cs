using UniModules.UniCore.Runtime.ObjectPool.Runtime;
using UniModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.CoreModules.UniGame.Core.Runtime.Async;

namespace UniModules.UniCore.Runtime.Extension
{
    using System;
    using Cysharp.Threading.Tasks;
    using UniGame.Core.Runtime.Interfaces;
    using UnityEngine;

    public static class AsyncExtension
    {
        public static async UniTask WaitUntil(this object source, Func<bool> waitFunc,PlayerLoopTiming timing = PlayerLoopTiming.Update)
        {
            while (waitFunc() == false)
            {
                await UniTask.Yield(timing);
            }
        }

        public static async UniTask WaitUntil(this AsyncOperation source)
        {
            while (source.isDone == false)
            {
                await UniTask.DelayFrame(1);
            }

        }

        public static async UniTask WaitUntil(this ICompletionStatus status)
        {
            while (status.IsComplete == false) {
                await UniTask.DelayFrame(1);
            }
        }
        
        public static async UniTask<TValue> AwaitFirstAsync<TValue>(this IObservable<TValue> value, ILifeTime lifeTime,
            Func<TValue, bool> predicate = null)
        {
            var firstAwaiter = ClassPool.Spawn<AwaitFirstAsyncOperation<TValue>>();
            var result = await firstAwaiter.AwaitFirstAsync(value, lifeTime, predicate);
            firstAwaiter.Despawn();
            return result;
        }
    }
}
