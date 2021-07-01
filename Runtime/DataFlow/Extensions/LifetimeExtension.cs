using System;
using System.Threading;
using UniGame.Core.Runtime.Common;
using UniGame.Core.Runtime.DataFlow;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniCore.Runtime.DataFlow.Interfaces;
using UniModules.UniCore.Runtime.ObjectPool.Runtime;
using UniModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
using UniModules.UniGame.Core.Runtime.DataFlow;
using UniModules.UniGame.Core.Runtime.DataFlow.Extensions;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.Core.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class LifetimeExtension
{

    public static ILifeTime DestroyWith(this ILifeTime lifeTime, GameObject gameObject)
    {
        if (!gameObject) return lifeTime;
        lifeTime.AddCleanUpAction(() => Despawn(gameObject,true));
        return lifeTime;
    }
    
    public static ILifeTime DestroyWith(this ILifeTime lifeTime, Component component)
    {
        if (!component) return lifeTime;
        lifeTime.AddCleanUpAction(() =>  Despawn(component.gameObject,true));
        return lifeTime;
    }
    
    public static ILifeTime DespawnWith(this ILifeTime lifeTime, GameObject gameObject)
    {
        if (!gameObject) return lifeTime;
        lifeTime.AddCleanUpAction(() => Despawn(gameObject,false));
        return lifeTime;
    }

    
    public static LifeTimeDefinition AddTo(this LifeTimeDefinition lifeTimeDefinition, ILifeTime lifeTime)
    {
        if (lifeTime == null)
            return lifeTimeDefinition;
        lifeTime.AddCleanUpAction(lifeTimeDefinition.Terminate);
        return lifeTimeDefinition;
    }

    
    public static LifeTimeDefinition ReleaseWith(this LifeTimeDefinition lifeTimeDefinition, ILifeTime lifeTime)
    {
        if (lifeTime == null)
            return lifeTimeDefinition;
        lifeTime.AddCleanUpAction(lifeTimeDefinition.Release);
        return lifeTimeDefinition;
    }
    
    public static TLifeTime AddCleanUpAction<TLifeTime>(this TLifeTime context,Action action)
        where TLifeTime : ILifeTimeContext
    {
        context.LifeTime.AddCleanUpAction(action);
        return context;
    }
    
    public static ILifeTimedAction CreateLifeTimedAction<TLifeTime>(
        this ILifeTime lifeTime,
        Action action,
        Action onLifetimeFinished = null)
    {
        var lifetimeAction = ClassPool.Spawn<LifeTimedAction>();
        lifetimeAction.Initialize(lifeTime,action,onLifetimeFinished);
        return lifetimeAction;
    }

    public static IDisposableCommand CreateLifeTimeCommand<TLifeTime>(this Action<ILifeTime> action)
    {
        var lifetimeAction = ClassPool.Spawn<LifeTimeContextCommand>();
        lifetimeAction.Initialize(action);
        return lifetimeAction;
    }
    
    public static TLifeTime AddDisposable<TLifeTime>(this TLifeTime context,IDisposable action)
        where TLifeTime : ILifeTimeContext
    {
        context.LifeTime.AddDispose(action);
        return context;
    }
       
    public static IComposedLifeTime Compose(this ILifeTime source, params  ILifeTime[] lifeTimes)
    {
        var composeAction = ClassPool.Spawn<ComposedLifeTime>();
        return composeAction.
            Bind(source).
            Bind(lifeTimes);
    }

    public static ILifeTime ComposeCleanUp(
        this ILifeTime source,
        ILifeTime additional,
        Action cleanup) => ComposeCleanUp(source, cleanup, additional);

    public static bool IsTerminatedLifeTime(this ILifeTime lifeTime)
    {
        return lifeTime == null || lifeTime.IsTerminated || lifeTime == LifeTime.TerminatedLifetime;
    }

    public static ILifeTime GetLifeTime(this object source)
    {
        if (source == null)
            return LifeTime.TerminatedLifetime;
        
        switch (source)
        {
            case Scene scene:
                return scene.GetSceneLifeTime();
            case ILifeTime lifeTime:
                return lifeTime;
            case ILifeTimeContext lifeTimeContext:
                return lifeTimeContext.LifeTime;
            case Component component:
                return component.GetAssetLifeTime();
            case GameObject gameObject:
                return gameObject.GetAssetLifeTime();
        }
        
        return LifeTime.TerminatedLifetime;
    }
    
    /// <summary>
    /// release lifetime when all dependencies released
    /// </summary>
    public static ILifeTime MergeLifeTime(
        this ILifeTime source, 
        Action cleanup,
        params ILifeTime[] additional)
    {
        var mergedLifeTime = source.MergeLifeTime(additional);
        mergedLifeTime.AddCleanUpAction(cleanup);
        
        return mergedLifeTime;
    }

    /// <summary>
    /// release lifetime when all dependencies released
    /// </summary>
    public static UnionLifeTime ToUnionLifeTime(this ILifeTime source)
    {
        var mergedLifeTime = new UnionLifeTime();
        mergedLifeTime.Add(source);
        
        return mergedLifeTime;
    }
    
    /// <summary>
    /// release lifetime when all dependencies released
    /// </summary>
    public static ILifeTime MergeLifeTime(
        this ILifeTime source, 
        params ILifeTime[] additional)
    {
        var mergedLifeTime = source.ToUnionLifeTime();
        mergedLifeTime.Add(additional);
        
        return mergedLifeTime;
    }
    
    /// <summary>
    /// call release with first released lifetime
    /// </summary>
    public static ILifeTime ComposeCleanUp(
        this ILifeTime source, 
        Action cleanup,
        params ILifeTime[] additional)
    {
        var composeAction = source.ComposeCleanUp(additional);
        composeAction.AddCleanUpAction(cleanup);
        
        return source;
    }

    /// <summary>
    /// call release with first released lifetime
    /// </summary>
    public static ILifeTime ComposeCleanUp(
        this ILifeTime source, 
        params ILifeTime[] additional)
    {
        var composeAction = new LifeTimeCompose();
        
        composeAction.Add(source);
        composeAction.Add(additional);
        
        return composeAction;
    }
    
    #region type convertion
    
    public static CancellationToken AsCancellationToken(this ILifeTime lifeTime)
    {
        return lifeTime.TokenSource;
    } 
    
    #endregion
    
    
    private static void Despawn(GameObject gameObject,bool destroy)
    {
        if (!gameObject) return;
        
        if (destroy)
        {
            Object.Destroy(gameObject);
            return;
        }
        
        gameObject.DespawnAsset();
    }
}
