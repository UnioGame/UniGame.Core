using UniGame.Runtime.ObjectPool.Extensions;
using UniGame.Core.Runtime;

namespace UniGame.Core.Runtime.Extension
{
    using UnityEngine;

    public static class ComponentExtensions
    {
        public static RectTransform RectTransform(this MonoBehaviour behaviour)
        {
            return behaviour.transform as RectTransform;
        }

        public static GameObject DespawnWith(this GameObject gameObject, ILifeTime lifeTime)
        {
            if (!gameObject) return gameObject;
            lifeTime.AddCleanUpAction(() => gameObject.DespawnAsset());
            return gameObject;
        }
        
        public static T DespawnWith<T>(this T component, ILifeTime lifeTime)
            where T : Component
        {
            if (!component) return component;
            lifeTime.AddCleanUpAction(() =>  component.DespawnAsset());
            return component;
        }
        
        public static Object GetRootAsset(this Object target)
        {
            if (target is Component component) return component.gameObject;
            return target;
        }
    }
}