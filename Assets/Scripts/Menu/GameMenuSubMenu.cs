using UnityEngine;
using System.Collections;

public class GameMenuSubMenu : MonoBehaviour
{
	public         Transform       items;
	public         float           animationDelay   = 0.2f;
	public         float           hideDelay        = 0.6f;
	public         float           showDelay        = 0.6f;

	private static GameMenuSubMenu lastSelectedItem;
	private static bool            isAnimated;

	void Start()
	{
		lastSelectedItem = null;
		isAnimated=false;
	}

	IEnumerator OnMouseUp()
	{
		if (lastSelectedItem!=this && !isAnimated)
		{
			isAnimated=true;

			if (lastSelectedItem!=null)
			{
				StartCoroutine(lastSelectedItem.hideItems());
				yield return new WaitForSeconds(hideDelay);
			}

			lastSelectedItem=this;
			StartCoroutine(showItems());
			yield return new WaitForSeconds(showDelay);

			isAnimated=false;
		}
	}

	private IEnumerator showItems()
	{
		for (int i=0; i<items.childCount; ++i)
		{
			items.GetChild(i).animation.Play("GameMenuItemUp");
			yield return new WaitForSeconds(animationDelay);
		}
	}

	private IEnumerator hideItems()
	{
		for (int i=0; i<items.childCount; ++i)
		{
			items.GetChild(i).animation.Play("GameMenuItemDown");
			yield return new WaitForSeconds(animationDelay);
		}
	}
}
