using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	[SerializeField] private float moveForce = 5.0f;
	[SerializeField] private float airControlForce = 5.0f;
	[SerializeField] private float friction = 2.0f;
	[SerializeField] private float jumpHeight = 2.0f;
	[SerializeField] private LayerMask ground;

	[SerializeField] private float groundCheckRadius = 0.5f;
	[SerializeField] private Vector3 groundCheckPosition;

	[SerializeField] private Transform playerCamera;
	[SerializeField] private float lookSensitivity;

	[SerializeField] private float gunPower = 3000.0f;
	[SerializeField] private LayerMask bloodSpawnLayer;
	[SerializeField] private GameObject bloodParticles;

	[SerializeField] private float crouchFactor = 0.5f;

	[SerializeField] private GameObject rocket;
	[SerializeField] private Transform rocketSpawnPoint;

	private float originalScale;

	private Rigidbody rb;
	private Vector3 input = Vector3.zero;
	private bool grounded = false;
	private float camRotX = 0.0f;
	public float lookXLimit = 80.0f;

	void Start() {
		rb = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;

		originalScale = transform.localScale.y;
	}

	public static T GetComponentInRootLevelParent<T>(GameObject go) {
		if (go.GetComponent<T>() != null) {
			return go.GetComponent<T>();
		}

		if (go.transform.parent != null) {
			return GetComponentInRootLevelParent<T>(go.transform.parent.gameObject);
		}

		return default(T);
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

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 100.0f)) {
				if (hitInfo.collider.GetComponent<Rigidbody>() != null) {
					var p =	GetComponentInRootLevelParent<RagdollActivate>(hitInfo.collider.gameObject);
					if (p != null) { p.Activate(); }

					hitInfo.collider.GetComponent<Rigidbody>().AddForceAtPosition(-hitInfo.normal * gunPower, hitInfo.point);

					if (hitInfo.collider.gameObject.layer == 7) {
						var go = Instantiate(bloodParticles);
						go.transform.position = hitInfo.point;
					}
				}
			}
		}

		if (Input.GetMouseButtonDown(1)) {
			var r = Instantiate(rocket, rocketSpawnPoint.position, rocketSpawnPoint.rotation);
		}

		if (Input.GetButtonDown("Crouch")) {
			transform.localScale = new Vector3(transform.localScale.x, originalScale * crouchFactor, transform.localScale.z);
		}

		if (Input.GetButtonUp("Crouch")) {
			transform.localScale = new Vector3(transform.localScale.x, originalScale, transform.localScale.z);
		}
	}

	void FixedUpdate()
	{
		Vector3 movement = transform.forward * input.z + transform.right * input.x;

		if (grounded) {
			rb.AddForce(movement * Time.fixedDeltaTime * moveForce);
			rb.AddForce(-rb.velocity * Time.fixedDeltaTime * friction);
		} else
		{

			rb.AddForce(movement * Time.fixedDeltaTime * airControlForce);
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(transform.position + groundCheckPosition, groundCheckRadius);
	}
}
