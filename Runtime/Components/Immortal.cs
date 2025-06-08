using UnityEngine;

namespace UniGame.Runtime.Components
{
    public class Immortal : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
