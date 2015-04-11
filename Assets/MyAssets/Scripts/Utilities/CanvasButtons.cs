using UnityEngine;
using System.Collections;

public class CanvasButtons : MonoBehaviour {

	public void Level1()
	{
		Application.LoadLevel("Level1");
	}

	public void Level2()
	{
		Application.LoadLevel("Level2");
	}

	public void Level3()
	{
		Application.LoadLevel("Level3");
	}

	public void MainMenu()
	{
		Application.LoadLevel("MainMenu");
	}
}
