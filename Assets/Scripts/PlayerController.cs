using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	[SerializeField] private float speed = 5.0f;
	[SerializeField] private float jumpHeight = 2.0f;
	[SerializeField] private float dashDistance = 5.0f;
	[SerializeField] private LayerMask ground;

	[SerializeField] private float groundCheckRadius = 0.5f;
	[SerializeField] private Vector3 groundCheckPosition;

	[SerializeField] private Transform playerCamera;
	[SerializeField] private float lookSensitivity; 

	private Rigidbody rb;
	private Vector3 input = Vector3.zero;
	private bool grounded = false;
	private float camRotX = 0.0f;
	public float lookXLimit = 80.0f;

	void Start() {
		rb = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		grounded = false;
		if (Physics.OverlapSphere(rb.position + groundCheckPosition, groundCheckRadius, ground).Length > 0) {
			grounded = true;
		}

		input = Vector3.zero;
		input.x = Input.GetAxis("Horizontal");
		input.z = Input.GetAxis("Vertical");

		if (Input.GetButtonDown("Jump") && grounded) {
			rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y), ForceMode.VelocityChange);
		}

		camRotX += -Input.GetAxis("Mouse Y") * lookSensitivity;
		camRotX = Mathf.Clamp(camRotX, -lookXLimit, lookXLimit);
		playerCamera.transform.localRotation = Quaternion.Euler(camRotX, 0, 0);
		transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSensitivity, 0);
	}

	void FixedUpdate() {
		Vector3 movement = transform.forward * input.z + transform.right * input.x; 

		rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(transform.position + groundCheckPosition, groundCheckRadius);
	}
}
