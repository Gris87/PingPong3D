using UnityEngine;
using System.Collections;

public class GameMenuOptions : MonoBehaviour
{
    void OnButtonPress()
    {
        SceneManager.LoadScene("OptionsScene");
    }
}
