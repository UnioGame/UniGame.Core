namespace UniModules.UniGame.Core.Runtime.Extension
{
    using UnityEngine;

    public static class ComponentExtensions
    {
        public static RectTransform RectTransform(this MonoBehaviour behaviour)
        {
            return behaviour.transform as RectTransform;
        }
        
        
        public static Object GetRootAsset(this Object target)
        {
            if (target is Component component) return component.gameObject;
            return target;
        }
    }
}