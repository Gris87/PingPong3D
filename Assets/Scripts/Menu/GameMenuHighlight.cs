using UnityEngine;
using System.Collections;

public class GameMenuHighlight : MonoBehaviour
{
	void OnMouseEnter()
	{
		renderer.material.color=Color.green;
	}

	void OnMouseExit()
	{
		renderer.material.color=Color.white;
	}
}
