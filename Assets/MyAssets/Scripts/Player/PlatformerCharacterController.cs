using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerCharacterController : MonoBehaviour 
{
	/// <summary>
	/// The animator reference.
	/// </summary>
	[SerializeField] private Animator animator = null;

	/// <summary>
	/// The sprite container reference.
	/// </summary>
	[SerializeField] private GameObject spriteContainer = null;

	/// <summary>
	/// The ground check reference.
	/// </summary>
	[SerializeField] private Transform groundCheck = null;

	/// <summary>
	/// The jump force applied to the character when jumping.
	/// </summary>
	[SerializeField] private float jumpForce = 1f;

	/// <summary>
	/// The walk force applied to the character when walking.
	/// </summary>
	[SerializeField] private float walkForce = 10f;	

	/// <summary>
	/// The max walk speed enforced for the character.
	/// </summary>
	[SerializeField] private float maxWalkSpeed = 4f;

	/// <summary>
	/// Whether the character is in contact with the ground.
	/// </summary>
	[SerializeField] private bool isOnGround = false;

	/// <summary>
	/// The character's movement direction.
	/// </summary>
	[SerializeField] private Vector2 direction = Vector2.zero;


	#region MonoBehaviour

	void FixedUpdate ()
	{
		OrientCharacter(this.direction);
		Walk(this.direction);
		Jump(this.direction);
	}

	void Update ()
	{
		ProcessInput();
		CheckGround();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag("MovingPlatform"))
		{
			this.transform.parent = other.transform;
		}

		if (other.gameObject.tag == "FirePit")
		{
			this.animator.SetTrigger("Death");
			this.rigidbody2D.isKinematic = true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag("MovingPlatform"))
		{
			this.transform.parent = null;
		}
	}

	#endregion MonoBehaviour


	#region Input

	private void ProcessInput ()
	{
		ProcessWalking();
		ProcessJump();
	}

	/// <summary>
	/// Processes the input for walking.
	/// </summary>
	private void ProcessWalking ()
	{
		this.direction = Vector2.zero;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.direction += new Vector2(-1, 0);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.direction += new Vector2(1, 0);
		}
	}

	/// <summary>
	/// Processes the input for jump.
	/// </summary>
	private void ProcessJump ()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isOnGround == true)
		{
			this.direction.y = 1;
		}
	}

	#endregion Input


	#region Activities

	/// <summary>
	/// Orients the character horizontally left or right based on the provided direction.
	/// </summary>
	/// <param name="direction">Direction.</param>
	private void OrientCharacter  (Vector2 direction)
	{
		Vector3 spriteScale = this.spriteContainer.transform.localScale;
		if (direction.x > 0)
		{
			spriteScale.x = 1;
		}
		else if (direction.x < 0)
		{
			spriteScale.x = -1;
		}
		this.spriteContainer.transform.localScale = spriteScale; 
	}

	/// <summary>
	/// Moves the player in the specified direction via physics forces.
	/// </summary>
	/// <param name="direction">Direction.</param>
	private void Walk (Vector2 direction)
	{
		this.rigidbody2D.AddForce(direction * this.walkForce);
		float horizontalSpeed = Mathf.Abs(this.rigidbody2D.velocity.x);

		if (Mathf.Abs(horizontalSpeed) > this.maxWalkSpeed)
		{
			Vector2 newVelocity = this.rigidbody2D.velocity;
			float multiplier = (this.rigidbody2D.velocity.x > 0) ? 1 : -1;
			newVelocity.x = multiplier * maxWalkSpeed;
			this.rigidbody2D.velocity = newVelocity;
		}

		this.animator.SetFloat("HorizontalSpeed", horizontalSpeed);
	}

	/// <summary>
	/// Raises the character upwards using physics forces, if the provided direction has a non-zero vertical component.
	/// </summary>
	/// <param name="direction">Direction.</param>
	private void Jump (Vector2 direction)
	{
		if (direction.y > 0)
		{
			this.animator.SetTrigger("Jumping");
			this.rigidbody2D.AddForce(Vector2.up * this.jumpForce);
			direction.y = 0;
		}
	}

	/// <summary>
	/// Checks whether the character is in contact with the ground.
	/// </summary>
	private void CheckGround ()
	{
		Collider2D collider = Physics2D.OverlapPoint(this.groundCheck.transform.position);
		this.isOnGround = (collider != null);
	}

	#endregion Activities
}
