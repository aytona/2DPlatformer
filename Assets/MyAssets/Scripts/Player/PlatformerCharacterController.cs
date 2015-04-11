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

	/// <summary>
	/// The impact tolerance of the player before dying.
	/// </summary>
	[SerializeField] private float impactTolerance = 10f;

	/// <summary>
	/// Checks if the players has the rocket.
	/// </summary>
	[SerializeField] private bool hasRocket = false;

	/// <summary>
	/// Checks if the player picked up the bombs.
	/// </summary>
	[SerializeField] private bool hasBomb = false;

	/// <summary>
	/// The bomb prefab.
	/// </summary>
	[SerializeField] private GameObject bombPrefab = null;

	/// <summary>
	/// The rocket force.
	/// </summary>
	[SerializeField] private float rocketForce = 10f;

	/// <summary>
	/// The bomb spawn point.
	/// </summary>
	[SerializeField] private Transform bombSpawn = null;

	/// <summary>
	/// The player is running.
	/// </summary>
	[SerializeField] private bool isRunning = false;

	/// <summary>
	/// The bomb count.
	/// </summary>
	[SerializeField] private int bombCount = 0;

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

		if (other.gameObject.tag == "FirePit" || other.gameObject.tag == "Bullet")
		{
			DeathAnim();
		}

		if (other.gameObject.tag == "Levitation")
		{
			this.gameObject.rigidbody2D.gravityScale = -0.5f;
		}

		if (other.gameObject.tag == "Rocket")
		{
			hasRocket = true;
		}

		if (other.gameObject.tag == "BombCrate")
		{
			AudioManager.Instance.PlayBompPickUpClip();
			Data.Instance.Bomb = 5;
			bombCount = 5;
			hasBomb = true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag("MovingPlatform"))
		{
			this.transform.parent = null;
		}

		if (other.gameObject.tag == "Levitation")
		{
			this.gameObject.rigidbody2D.gravityScale = 1;
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.relativeVelocity.magnitude > impactTolerance)
		{
			DeathAnim();
		}
	}

	void DeathAnim()
	{
		this.animator.SetTrigger("Death");
		this.rigidbody2D.isKinematic = true;
		AudioManager.Instance.PlayPlayerDeathClip();
	}

	#endregion MonoBehaviour


	#region Input

	private void ProcessInput ()
	{
		ProcessWalking();
		ProcessJump();
		ProcessRocket();
		ProcessBomb();
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
		if (Input.GetKeyDown(KeyCode.Space) && isOnGround == true && hasRocket == false)
		{
			this.direction.y = 1;
		}
	}

	/// <summary>
	/// Processes the input for rocket.
	/// </summary>
	private void ProcessRocket()
	{
		if (Input.GetButton("Vertical") && hasRocket == true)
		{
			Vector2 globalDirection = this.transform.TransformDirection(Vector2.up);
			this.gameObject.rigidbody2D.AddForce(globalDirection * rocketForce);
		}
	}

	private void ProcessBomb()
	{
		if (Input.GetKeyDown(KeyCode.B) && hasBomb == true && bombCount > 0)
		{
			GameObject bomb = Instantiate(this.bombPrefab) as GameObject;
			bomb.transform.position = this.bombSpawn.transform.position;
			bomb.transform.rotation = this.bombSpawn.transform.rotation;

			bombCount--;
			Data.Instance.Bomb--;
			AudioManager.Instance.PlayFireClip();
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
		
		if (Mathf.Abs(horizontalSpeed) > 1.5)
		{
			isRunning = true;
		}
		else{
			isRunning = false;
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
			// Plays Roll Clip if the player is running
			// Else play normal Jump Clip
			if (isRunning == true)
			{
				AudioManager.Instance.PlayRollClip();
			}
			else if (isRunning == false)
			{
				AudioManager.Instance.PlayJumpClip();
			}
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
