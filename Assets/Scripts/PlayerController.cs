using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Rigidbody2D rb;

	public float moveForce;
	public float jumpForce;
	public float maxVelocity;

	public float friction;
	public float air_resistence;

	public float groundCheckRadius;
	public Vector2 groundCheckPosition;

	private Vector2 input;
	public bool grounded;

	private Collider2D col;

	public LayerMask groundLayer;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<CapsuleCollider2D>();
	}

	private void Update()
	{
		input.x = Input.GetAxis("Horizontal");

		if (Input.GetKeyDown(KeyCode.Z) && grounded)
		{
			rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
		}
	}

	private void FixedUpdate()
	{
		if ((input.x > 0.0f && rb.velocity.x < maxVelocity) || (input.x < 0.0f && rb.velocity.x > -maxVelocity))
		{
			rb.AddForce(new Vector2(input.x * moveForce, 0.0f));
		}

		grounded = false;
		if (Physics2D.OverlapCircle(new Vector2(transform.position.x + groundCheckPosition.x, transform.position.y + groundCheckPosition.y), groundCheckRadius, groundLayer))
		{
			grounded = true;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		/* Do friction or something. */
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(new Vector3(transform.position.x + groundCheckPosition.x, transform.position.y + groundCheckPosition.y, transform.position.z), groundCheckRadius);
	}
}
