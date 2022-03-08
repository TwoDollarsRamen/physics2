using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlaftorm : MonoBehaviour {
	[SerializeField] private float speed = 5.0f;

	[SerializeField] private Transform start;
	[SerializeField] private Transform end;

	bool moving = false;

	float t = 0.0f;
	float m = 1.0f;

	private Rigidbody rb;

	private void Start() {
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		rb.position = Vector3.Lerp(start.position, end.position, t);

		if (moving && t < 1.0f) {
			t += m * Time.fixedDeltaTime * speed;
		}

		if (m > 0.0f && t >= 1.0f) {
			m = -1.0f;
		}

		if (m < 0.0f && t <= 0.0f) {
			moving = false;
			m = 1.0f;
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.collider.gameObject.CompareTag("Player")) {
			moving = true;
		}
	}
}
