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
            InputControl.setKey("Up",     KeyCode.W,         new JoystickInput(Joystick.Joystick1, JoystickAxis.Axis1Positive));
            InputControl.setKey("Down",   KeyCode.S,         new JoystickInput(Joystick.Joystick1, JoystickAxis.Axis1Negative));
            InputControl.setKey("Up 2",   KeyCode.UpArrow,   new JoystickInput(Joystick.Joystick2, JoystickAxis.Axis1Positive));
            InputControl.setKey("Down 2", KeyCode.DownArrow, new JoystickInput(Joystick.Joystick2, JoystickAxis.Axis1Negative));
            
            // Setup axes
            InputControl.setAxis("Vertical",   "Down",   "Up");
            InputControl.setAxis("Vertical 2", "Down 2", "Up 2");
        }

        Options.load();

        SceneManager.LoadScene("GameMenu");
    }
}
