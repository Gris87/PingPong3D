using UnityEngine;
using System.Collections;

public class GameMenuQuit : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
#if UNITY_WEBPLAYER
        Destroy(gameObject);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (InputControl.GetKeyDown(KeyCode.Escape))
        {
            OnButtonPress();
        }
    }

    void OnButtonPress()
    {
        Debug.Log("Application finished");
        Application.Quit();
    }
}
