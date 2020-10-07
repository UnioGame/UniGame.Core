using UnityEngine;

namespace UniModules.UniCore.Runtime.Components
{
    public class Immortal : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
