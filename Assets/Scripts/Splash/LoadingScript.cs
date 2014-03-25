using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        guiText.fontSize=(int)(Screen.height*0.1);

        if (!Utils.isTouchDevice)
        {
            // Setup keys
            InputControl.setKey("Up",     KeyCode.W);
            InputControl.setKey("Down",   KeyCode.S);
            InputControl.setKey("Up 2",   KeyCode.UpArrow);
            InputControl.setKey("Down 2", KeyCode.DownArrow);
            
            // Setup axis
            InputControl.setAxis("Vertical",   "Down",   "Up");
            InputControl.setAxis("Vertical 2", "Down 2", "Up 2");
        }

        Options.load();

        SceneManager.LoadScene("GameMenu");
    }
}
