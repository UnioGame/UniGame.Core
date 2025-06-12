﻿namespace UniGame.Runtime.Utils
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class MemoryUtils
    {

        public static IEnumerator CleanUpFullMemoryAsync() {

            Debug.Log("MemoryUtils.CleanUpAsync");
            yield return UnloadUnusedAssetsAsync();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            yield return UnloadUnusedAssetsAsync();
            GC.Collect();
        }

        public static void CleanUp() {

            Debug.Log("MemoryUtils.CleanUp");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }

        public static IEnumerator UnloadUnusedAssetsAsync() {
            
            Debug.Log("MemoryUtils.UnloadUnesedAssets");
#if UNITY_ANDROID
//// Not waiting for Resources.UnloadUnusedAssets(), because it is terribly slow as of Unity 5.6.2p2 (takes ~2 minutes sometimes).
			yield return null;
#else
            yield return Resources.UnloadUnusedAssets();
#endif
        }

    }
}
