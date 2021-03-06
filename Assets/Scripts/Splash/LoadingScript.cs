#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
#define TOUCH_DEVICE
#endif

using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        guiText.fontSize=(int)(Screen.height*0.1);

#if !TOUCH_DEVICE
        // Setup keys
        InputControl.setKey("Up",     KeyCode.W,         new JoystickInput(JoystickAxis.Axis1Positive, Joystick.Joystick1));
        InputControl.setKey("Down",   KeyCode.S,         new JoystickInput(JoystickAxis.Axis1Negative, Joystick.Joystick1));
        InputControl.setKey("Up 2",   KeyCode.UpArrow,   new JoystickInput(JoystickAxis.Axis1Positive, Joystick.Joystick2));
        InputControl.setKey("Down 2", KeyCode.DownArrow, new JoystickInput(JoystickAxis.Axis1Negative, Joystick.Joystick2));

        // Setup axes
        InputControl.setAxis("Vertical",   "Down",   "Up");
        InputControl.setAxis("Vertical 2", "Down 2", "Up 2");
#endif

        Options.load();

        SceneManager.LoadScene("GameMenu");
    }
}
