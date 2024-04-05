namespace Game.Modules.UnioModules.UniGame.CoreModules.UniGame.Core.Runtime.Extension
{
    using global::UniGame.Runtime.ObjectPool.Extensions;
    using UnityEngine;
    using UnityEngine.Pool;

    public static class GameObjectExtensions
    {
        public static GameObject FindGameObject(this GameObject source,string name, bool inChildren = true)
        {
            return inChildren ? source.FindChildGameObject(name) : GameObject.Find(name);
        }
        
        public static GameObject FindChildGameObject(this GameObject fromGameObject, string withName)
        {
            var transforms = ListPool<Transform>.Get();
            fromGameObject.transform.GetComponentsInChildren(true,transforms);

            var result = (GameObject)null;
            
            foreach (var transform in transforms)
            {
                if(!transform.name.Equals(withName)) continue;
                result = transform.gameObject;
                break;
            }
            
            transforms.Despawn();
            return result;
        }
    }
}