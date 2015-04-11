using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	/// <summary>
	/// The gun cock back animation.
	/// </summary>
	[SerializeField] private Animator gunAnimator = null;

	/// <summary>
	/// The projectile prefab.
	/// </summary>
	[SerializeField] private GameObject projectilePrefab = null;

	/// <summary>
	/// The projectile spawn point.
	/// </summary>
	[SerializeField] private Transform projectileSpawnPoint = null;

	/// <summary>
	/// The field of view of the turret.
	/// </summary>
	[SerializeField] private bool playerOnSite = false;

	/// <summary>
	/// Checks if the turret already fired.
	/// </summary>
	[SerializeField] private bool shot = false;
	
	void LateUpdate()
	{
		if (playerOnSite == true)
		{
			Shoot();
			playerOnSite = false;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerOnSite = true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerOnSite = false;
			shot = false;
		}
	}

	/// <summary>
	/// Shoot the projectile from the projectile spawner.
	/// </summary>
	public void Shoot ()
	{
		if (shot == false)
		{
			instantiateProjectile();
			shot = true;
		}

		playerOnSite = false;
	}

	private void instantiateProjectile()
	{
		// Plays Fire sound.
		AudioManager.Instance.PlayFireClip();
		// Trigger the "Shoot" animation from our gun animator component.
		this.gunAnimator.SetTrigger("Shoot");
		// Spawn the projectile, setting its position and orientation to that of the spawner game object's transform.
		GameObject projectile = Instantiate(this.projectilePrefab) as GameObject;
		projectile.transform.position = this.projectileSpawnPoint.transform.position;
		projectile.transform.rotation = this.projectileSpawnPoint.transform.rotation;
		shot = false;

	}
}
