using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {

	void Restart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
