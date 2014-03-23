using UnityEngine;
using System.Collections;

public class GameMenuBehaviour : MonoBehaviour
{
    void OnMouseEnter()
    {
        if (!Utils.isTouchDevice)
        {
            renderer.material.color=Color.green;
        }
    }

    void OnMouseExit()
    {
        if (!Utils.isTouchDevice)
        {
            renderer.material.color=Color.white;
        }
    }

    void OnMouseDown()
    {
        animation.Play("GameMenuButtonPress");
    }
}
