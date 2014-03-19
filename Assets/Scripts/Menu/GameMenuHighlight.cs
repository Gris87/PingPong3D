using UnityEngine;
using System.Collections;

public class GameMenuHighlight : MonoBehaviour
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
}
