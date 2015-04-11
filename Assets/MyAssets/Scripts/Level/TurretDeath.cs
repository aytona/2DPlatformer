using UnityEngine;
using System.Collections;

public class TurretDeath : MonoBehaviour {
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Bomb")
		{
			AudioManager.Instance.PlayBossDeathClip();
			gameObject.collider2D.enabled = false;
		}
	}
}
