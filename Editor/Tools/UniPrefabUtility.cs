using UnityEditor;
using UnityEngine;

namespace UniModules.Editor
{
    public class UniPrefabUtility 
    {

        public static GameObject LoadPrefabContents(GameObject target)
        {

            var path = AssetDatabase.GetAssetPath(target);
        
            if (string.IsNullOrEmpty(path))
            {
                return target;
            }

            return PrefabUtility.LoadPrefabContents(path);

        }
    
    }
}
