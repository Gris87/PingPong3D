using UnityEngine;
using System.Collections;

public class GameMenuQuit : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        if (Application.platform==RuntimePlatform.OSXWebPlayer || Application.platform==RuntimePlatform.WindowsWebPlayer)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnMouseUp();
        }
    }

    void OnMouseUp()
    {
        Debug.Log("Application finished");
        Application.Quit();
    }
}
