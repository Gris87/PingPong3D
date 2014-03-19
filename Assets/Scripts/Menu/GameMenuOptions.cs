using UnityEngine;
using System.Collections;

public class GameMenuOptions : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("OptionsScene");
    }
}
