using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {

	/// <summary>
	/// Return to MainMenu after death.
	/// </summary>
	void Restart()
	{
		Application.LoadLevel("MainMenu");
	}
}
