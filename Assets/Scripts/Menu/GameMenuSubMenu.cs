using UnityEngine;
using System.Collections;

public class GameMenuSubMenu : MonoBehaviour
{
	private static GameMenuSubMenu lastSelectedItem = null;
	public         Transform       items;
	public         float           animationDelay   = 0.2f;
	public         float           hideDelay        = 0.6f;

	IEnumerator OnMouseUp()
	{
		if (lastSelectedItem!=this)
		{
			if (lastSelectedItem!=null)
			{
				StartCoroutine(lastSelectedItem.hideItems());
				yield return new WaitForSeconds(hideDelay);
			}

			lastSelectedItem=this;
			StartCoroutine(showItems());
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
