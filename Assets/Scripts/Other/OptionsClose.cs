using UnityEngine;
using System.Collections;

public class OptionsClose : MonoBehaviour
{
    void OnOptionsClosed()
    {
        Debug.Log("Go to game menu");

        SceneManager.LoadScene("GameMenu");
    }
}
