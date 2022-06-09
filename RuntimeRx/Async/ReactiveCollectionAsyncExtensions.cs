using System;
using Cysharp.Threading.Tasks;
using UniModules.UniCore.Runtime.Extension;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniRx;

public static class ReactiveCollectionAsyncExtensions 
{

    public static async UniTask<TValue> AwaitValue<TValue>(this IReadOnlyReactiveCollection<TValue> collection, Func<TValue, bool> predicate, ILifeTime lifeTime)
    {

        foreach (var value in collection)
        {
            if (predicate(value))
                return value;
        }

        var result = await collection
            .ObserveAdd()
            .Where(x => predicate(x.Value))
            .AwaitFirstAsync(lifeTime)
            .AttachExternalCancellation(lifeTime.AsCancellationToken());

        return result.Value;

    }
    
}
