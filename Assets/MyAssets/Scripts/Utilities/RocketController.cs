using UnityEngine;
using System.Collections;

public class RocketController : MonoBehaviour {

	/// <summary>
	/// Checks if the rocket is on the player.
	/// </summary>
	[SerializeField] private bool onPlayer = false;

	/// <summary>
	/// The flame particle system.
	/// </summary>
	[SerializeField] private GameObject flame = null;

	/// <summary>
	/// The direction of the rocket.
	/// </summary>
	[SerializeField] private Vector2 direction;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			onPlayer = true;
		}
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			// Rocket stays on player as soon as he touches it.
			gameObject.transform.position = other.gameObject.transform.position;
		}
	}

	void Awake()
	{
		this.flame.SetActive(false);
	}
	
	void Update()
	{
		if (onPlayer == true)
		{
			this.direction = detectInput();
			Move(this.direction);
		}
	}

	private Vector2 detectInput ()
	{
		// Set direction vector to (0,0).
		Vector2 movementDirection = Vector2.zero;
	
		// If the player press up, it makes the direction vector to (0,1).
		if (Input.GetKey (KeyCode.UpArrow)) {
			movementDirection += Vector2.up;
		}
	
		return movementDirection;
	}

	private void Move (Vector2 movementDirection)
	{
		// When the player press up, the rockets gives force and play flame anim.
		if (movementDirection.y > 0)
		{
			this.flame.SetActive(true);
		}
		else
		{
			this.flame.SetActive(false);
		}
	}
}
