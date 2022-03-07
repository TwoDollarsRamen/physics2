using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	[SerializeField] private float force;
	[SerializeField] private float explosionRadius;
	[SerializeField] private float explosionForce = 800.0f;

	[SerializeField] private GameObject explosionEffect;

	Rigidbody rb;
	
	void Start()
	{
		Physics.IgnoreLayerCollision(8, 3);

		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		rb.AddForce(force * transform.forward);
	}

	private void OnCollisionEnter(Collision collision)
	{
		var colliders = Physics.OverlapSphere(transform.position, explosionRadius);

		foreach (var collider in colliders) {
			if (collider.attachedRigidbody != null) {
				var p = PlayerController.GetComponentInRootLevelParent<RagdollActivate>(collider.gameObject);
				if (p != null) { p.Activate(); }

				collider.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
			}
		}

		Instantiate(explosionEffect, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
