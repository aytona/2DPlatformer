using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour 
{
	[SerializeField] private Vector3 movementDirection = Vector3.zero;
	[SerializeField] private float speed;
	[SerializeField] private bool isDestroyed = false;

	void Awake()
	{
		this.gameObject.GetComponent<ParticleSystem>().Stop ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player" && isDestroyed == false)
		{
			speed = 1;
		}
		if (other.tag == "DeActivation")
		{
			AudioManager.Instance.PlayImpactClip();
			this.gameObject.GetComponent<ParticleSystem>().Play();
			this.gameObject.GetComponent<Collider2D>().enabled = false;
			this.gameObject.GetComponent<Renderer>().enabled = false;
			this.gameObject.GetComponent<ParticleSystem>().enableEmission = true;
			Destroy(other.gameObject);
			isDestroyed = true;
			speed = 0;
		}
	}

	void FixedUpdate ()
	{
		Vector3 normalizedDirection = (Vector3)this.movementDirection.normalized;
		this.transform.Translate(normalizedDirection * this.speed * Time.deltaTime);
	}
}
