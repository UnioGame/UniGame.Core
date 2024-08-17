namespace UniGame.Runtime.ObjectPool.Components
{
	using UnityEngine;

	// This component will automatically reset a Rigidbody when it gets spawned/despawned
	[RequireComponent(typeof(Rigidbody))]
	public class PooledRigidbody : MonoBehaviour
	{
		protected virtual void OnSpawn()
		{
			// Do nothing
		}
		
		protected virtual void OnDespawn()
		{
			var rigidbody = GetComponent<Rigidbody>();
			
			// Reset velocities
#if UNITY_6000_0_OR_NEWER
			rigidbody.linearVelocity = Vector3.zero;
#else
			rigidbody.velocity = Vector2.zero;
#endif
			rigidbody.angularVelocity = Vector3.zero;
		}
	}
}