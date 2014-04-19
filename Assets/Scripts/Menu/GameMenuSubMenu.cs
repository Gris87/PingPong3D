#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
#define TOUCH_DEVICE
#endif

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

    IEnumerator OnButtonPress()
    {
        // TODO: Try to use animator
        if (lastSelectedItem!=this && !isAnimated)
        {
            isAnimated=true;

            if (lastSelectedItem!=null)
            {
                #region Remove highlight for touch devices
#if TOUCH_DEVICE
                    lastSelectedItem.renderer.material.color=Color.white;
#endif
                #endregion

                #region Hide previous selected items
                StartCoroutine(lastSelectedItem.hideItems());
                yield return new WaitForSeconds(hideDelay);
                #endregion
            }

            #region Highlight button for touch devices
#if TOUCH_DEVICE
            renderer.material.color=Color.green;
#endif
            #endregion

            #region Show items
            lastSelectedItem=this;
            StartCoroutine(showItems());
            yield return new WaitForSeconds(showDelay);
            #endregion

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
