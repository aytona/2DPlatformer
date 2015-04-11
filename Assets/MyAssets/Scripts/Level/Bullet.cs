using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	/// <summary>
	/// The speed of the bullet.
	/// </summary>
	[SerializeField] private float speed = 3;

	/// <summary>
	/// The default lifespan of the bullet.
	/// </summary>
	[SerializeField] private float lifespan = 10;

	/// <summary>
	/// Bullet's direction.
	/// </summary>
	[SerializeField] private Vector3 myDirection = Vector3.up;
	
	private bool isDestroyed = false;
	
	void Awake ()
	{
		StartCoroutine(LifespanCoroutine());
		StartCoroutine(MovementCoroutine());
	}
	
	void LateUpdate ()
	{
		if (this.isDestroyed)
		{
			AudioManager.Instance.PlayImpactClip();
			Destroy(this.gameObject);
		}
	}
	
	private IEnumerator LifespanCoroutine ()
	{
		yield return new WaitForSeconds(this.lifespan);
		Destroy(this.gameObject);
	}
	
	private IEnumerator MovementCoroutine ()
	{
		while (true)
		{
			// Move the projectile in the set direction, at the set speed, smoothly through time, for each frame.
			this.transform.Translate(this.myDirection * this.speed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}
	
	void OnCollisionEnter2D (Collision2D other)
	{
		// Destroyed when it touches the player or the walls of the level.
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Level")
		{
			this.isDestroyed = true;
		}
	}
}
