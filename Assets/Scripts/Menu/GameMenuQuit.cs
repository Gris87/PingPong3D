using UnityEngine;
using System.Collections;

public class GameMenuQuit : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		if (Application.platform==RuntimePlatform.OSXWebPlayer || Application.platform==RuntimePlatform.WindowsWebPlayer)
		{
			Destroy(gameObject);
		}
	}

	void OnMouseUp()
	{
		Debug.Log("Application finished");
		Application.Quit();
	}
}
