using UnityEngine;
using System.Collections;

public class MainMenuHighlight : MonoBehaviour
{
	private static MainMenuHighlight selectedItem=null;

	void OnMouseEnter()
	{
		if (selectedItem!=this)
		{
			renderer.material.color=Color.green;
		}
	}

	void OnMouseExit()
	{
		if (selectedItem!=this)
		{
			renderer.material.color=Color.white;
		}
	}

	void OnMouseUp()
	{
		if (selectedItem!=null)
		{
			selectedItem.renderer.material.color=Color.white;
		}

		selectedItem=this;
		renderer.material.color=Color.cyan;
	}
}
