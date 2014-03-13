using UnityEngine;
using System.Collections;

public class GameMenuStart : MonoBehaviour
{
	void OnMouseUp()
	{
		Application.LoadLevel("MainScene");
	}
}
