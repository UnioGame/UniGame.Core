﻿namespace UniGame.Runtime.ObjectPool.Components
{
	using UnityEngine;

	// This component will automatically reset a Rigidbody2D when it gets spawned/despawned
	[RequireComponent(typeof(Rigidbody2D))]
	public class PooledRigidbody2D : MonoBehaviour
	{
		// This gets called as soon as the clone is spawned
		protected virtual void OnSpawn()
		{
			// Do nothing
		}
		
		// This gets called just before the clone is despawned
		protected virtual void OnDespawn()
		{
			var rigidbody2D = GetComponent<Rigidbody2D>();
			
			// Reset velocities
			rigidbody2D.velocity        = Vector2.zero;
			rigidbody2D.angularVelocity = 0.0f;
		}
	}
}