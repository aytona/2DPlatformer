using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public string levelName = null;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Application.LoadLevel(levelName);
		}
	}
}
