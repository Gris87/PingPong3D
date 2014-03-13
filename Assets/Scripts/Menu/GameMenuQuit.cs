using UnityEngine;
using System.Collections;

public class GameMenuQuit : MonoBehaviour
{
	void OnMouseUp()
	{
		Debug.Log("Application finished");
		Application.Quit();
	}
}
