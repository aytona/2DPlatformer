using UnityEngine;
using System.Collections;

public class BossTriggers : MonoBehaviour {

	/// <summary>
	/// The turret body.
	/// </summary>
	[SerializeField] private GameObject turretBody = null;

	void Awake()
	{
		this.gameObject.particleSystem.enableEmission = false;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		// Kills the boss when the bomb touches him
		if (other.gameObject.tag == "Bomb")
		{
			turretBody.SetActive(false);
			this.gameObject.particleSystem.enableEmission = true;
			this.gameObject.particleSystem.Play ();
			AudioManager.Instance.PlayBossDeathClip();
		}
	}
}
