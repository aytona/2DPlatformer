using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Text bomb = null;

	// Reset bomb count to 0 when level 3 is loaded
	void OnLevelWasLoaded ()
	{
		if (Application.loadedLevelName == "Level3"){
			Data.Instance.Bomb = 0;
		}
	}

	void Update()
	{
		this.bomb.text = Data.Instance.Bomb.ToString();
	}
}
